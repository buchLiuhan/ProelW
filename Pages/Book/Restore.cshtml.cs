using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookModel = LibraryManagementSystem.Models.Book;

namespace LibraryManagementSystem.Pages.Books;

[Authorize(Roles = "Admin,Librarian")]
public class RestoreModel : PageModel
{
    private readonly AppDbContext _db;
    public BookModel Book { get; set; } = new();

    public RestoreModel(AppDbContext db) => _db = db;

    public IActionResult OnGet(int id)
    {
        var book = _db.Books.Find(id);
        if (book == null) return Redirect("/Books");
        Book = book;
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        var book = _db.Books.Find(id);
        if (book == null) return Redirect("/Books");

        book.IsActive = true;
        book.AvailableCopies = book.TotalCopies;
        _db.SaveChanges();

        TempData["Success"] = $"'{book.Title}' has been restored.";
        return Redirect("/Books");
    }
}