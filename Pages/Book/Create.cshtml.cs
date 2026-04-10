using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookModel = LibraryManagementSystem.Models.Book;

namespace LibraryManagementSystem.Pages.Books;

[Authorize(Roles = "Admin,Librarian")]
public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(AppDbContext db) => _db = db;

    public void OnGet() { }

    public IActionResult OnPost(string title, string author, string isbn,
                                 string genre, int totalCopies)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
        {
            ErrorMessage = "Title and Author are required.";
            return Page();
        }

        var book = new BookModel
        {
            Title = title,
            Author = author,
            ISBN = isbn,
            Genre = genre,
            TotalCopies = totalCopies,
            AvailableCopies = totalCopies
        };

        _db.Books.Add(book);
        _db.SaveChanges();
        return RedirectToPage("/Books");
    }
}


