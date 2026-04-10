using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookModel = LibraryManagementSystem.Models.Book;

namespace LibraryManagementSystem.Pages.Books;

[Authorize(Roles = "Admin,Librarian")]
public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    public BookModel Book { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public EditModel(AppDbContext db) => _db = db;

    public IActionResult OnGet(int id)
    {
        var book = _db.Books.Find(id);
        if (book == null) return RedirectToPage("/Books/Index");
        Book = book;
        return Page();
    }

    public IActionResult OnPost(int id, string title, string author,
                                 string isbn, string genre, int totalCopies)
    {
        var book = _db.Books.Find(id);
        if (book == null) return RedirectToPage("/Books/Index");

        var diff = totalCopies - book.TotalCopies;
        book.Title = title;
        book.Author = author;
        book.ISBN = isbn;
        book.Genre = genre;
        book.TotalCopies = totalCopies;
        book.AvailableCopies = Math.Max(0, book.AvailableCopies + diff);

        _db.SaveChanges();
        return RedirectToPage("/Books");
    }
}