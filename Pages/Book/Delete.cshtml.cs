using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookModel = LibraryManagementSystem.Models.Book;

namespace LibraryManagementSystem.Pages.Books;

[Authorize(Roles = "Admin,Librarian")]
public class DeleteModel : PageModel
{
    private readonly AppDbContext _db;
    public BookModel Book { get; set; } = new();

    public DeleteModel(AppDbContext db) => _db = db;

    public IActionResult OnGet(int id)
    {
        var book = _db.Books.Find(id);
        if (book == null) return RedirectToPage("/Books/Index");
        Book = book;
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        var book = _db.Books.Find(id);
        if (book != null)
        {
            _db.Books.Remove(book);
            _db.SaveChanges();
        }
        return Redirect("/Books");
    }
}