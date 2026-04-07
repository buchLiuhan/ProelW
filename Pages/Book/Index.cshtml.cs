using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookModel = LibraryManagementSystem.Models.Book;

namespace LibraryManagementSystem.Pages.Books;

[Authorize]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public List<BookModel> Books { get; set; } = new();
    public string Search { get; set; } = string.Empty;

    public IndexModel(AppDbContext db) => _db = db;

    public void OnGet(string search = "")
    {
        Search = search;
        var query = _db.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b =>
                b.Title.Contains(search) ||
                b.Author.Contains(search) ||
                b.ISBN.Contains(search));

        Books = query.OrderBy(b => b.Title).ToList();
    }
}