using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagementSystem.Controllers;

[Authorize(Roles = "Admin,Librarian")]
public class BorrowsController : Controller
{
    private readonly AppDbContext _db;

    public BorrowsController(AppDbContext db) => _db = db;

    // GET: /Borrows
    public IActionResult Index(string search = "")
    {
        var query = _db.BorrowRecords
            .Include(b => b.User)
            .Include(b => b.Book)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b =>
                b.User.FullName.Contains(search) ||
                b.Book.Title.Contains(search));

        ViewBag.Search = search;
        var records = query.OrderByDescending(b => b.BorrowedAt).ToList();
        return View(records);
    }

    // GET: /Borrows/Create
    public IActionResult Create()
    {
        ViewBag.Users = new SelectList(
            _db.Users.Where(u => u.Role == "Student").ToList(),
            "Id", "FullName");

        ViewBag.Books = new SelectList(
           _db.Books.Where(b => b.AvailableCopies > 0 && b.IsActive).ToList(),
           "Id", "Title");

        return View();
    }

    // POST: /Borrows/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(int userId, int bookId, DateTime dueDate)
    {
        var book = _db.Books.Find(bookId);
        if (book == null || book.AvailableCopies <= 0)
        {
            TempData["Error"] = "Book is not available.";
            return RedirectToAction("Create");
        }

        var alreadyBorrowed = _db.BorrowRecords.Any(b =>
            b.UserId == userId && b.BookId == bookId && b.ReturnedAt == null);

        if (alreadyBorrowed)
        {
            TempData["Error"] = "This student already has this book borrowed.";
            return RedirectToAction("Create");
        }

        var record = new BorrowRecord
        {
            UserId = userId,
            BookId = bookId,
            BorrowedAt = DateTime.Now,
            DueDate = dueDate
        };

        book.AvailableCopies--;
        _db.BorrowRecords.Add(record);
        _db.SaveChanges();

        TempData["Success"] = "Book successfully issued.";
        return RedirectToAction("Index");
    }

    // GET: /Borrows/Return/5
    public IActionResult Return(int id)
    {
        var record = _db.BorrowRecords
            .Include(b => b.User)
            .Include(b => b.Book)
            .FirstOrDefault(b => b.Id == id);

        if (record == null) return RedirectToAction("Index");
        return View(record);
    }

    // POST: /Borrows/Return/5
    [HttpPost, ActionName("Return")]
    [ValidateAntiForgeryToken]
    public IActionResult ReturnConfirmed(int id)
    {
        var record = _db.BorrowRecords
            .Include(b => b.Book)
            .FirstOrDefault(b => b.Id == id);

        if (record == null) return RedirectToAction("Index");

        record.ReturnedAt = DateTime.Now;
        record.Book.AvailableCopies++;
        _db.SaveChanges();

        TempData["Success"] = "Book successfully returned.";
        return RedirectToAction("Index");
    }
}