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
    public bool ShowInactive { get; set; }

    public IndexModel(AppDbContext db) => _db = db;

    public void OnGet(string search = "", bool showInactive = false)
    {
        Search = search;
        ShowInactive = showInactive;

        var query = _db.Books.AsQueryable();

        if (!showInactive)
            query = query.Where(b => b.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b =>
                b.Title.Contains(search) ||
                b.Author.Contains(search) ||
                b.ISBN.Contains(search));

        Books = query.OrderBy(b => b.Title).ToList();
    }
}