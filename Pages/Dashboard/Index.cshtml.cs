using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Pages.Dashboard;

[Authorize]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public int ActiveBorrows { get; set; }
    public int TotalUsers { get; set; }
    public List<BorrowRecord> RecentBorrows { get; set; } = new();
    public List<BorrowRecord> OverdueBorrows { get; set; } = new();

    public IndexModel(AppDbContext db) => _db = db;

    public void OnGet()
    {
        TotalBooks = _db.Books.Count();
        AvailableBooks = _db.Books.Any() ? _db.Books.Sum(b => b.AvailableCopies) : 0;
        ActiveBorrows = _db.BorrowRecords.Count(b => b.ReturnedAt == null);
        TotalUsers = _db.Users.Count();

        RecentBorrows = _db.BorrowRecords
            .Include(b => b.User)
            .Include(b => b.Book)
            .OrderByDescending(b => b.BorrowedAt)
            .Take(8)
            .ToList();

        OverdueBorrows = _db.BorrowRecords
            .Include(b => b.User)
            .Include(b => b.Book)
            .Where(b => b.ReturnedAt == null && b.DueDate < DateTime.Now)
            .ToList();
    }
}