using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagementSystem.Pages.Dashboard;

[Authorize]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    // Admin/Librarian stats
    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public int ActiveBorrows { get; set; }
    public int TotalUsers { get; set; }
    public List<BorrowRecord> RecentBorrows { get; set; } = new();
    public List<BorrowRecord> OverdueBorrows { get; set; } = new();

    // Student stats
    public int MyActiveBorrows { get; set; }
    public int MyOverdue { get; set; }
    public List<BorrowRecord> MyBorrows { get; set; } = new();

    public IndexModel(AppDbContext db) => _db = db;

    public void OnGet()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int.TryParse(userIdStr, out int userId);

        if (User.IsInRole("Student"))
        {
            // Student only sees their own data
            TotalBooks = _db.Books.Count(b => b.IsActive);

            MyBorrows = _db.BorrowRecords
                .Include(b => b.Book)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BorrowedAt)
                .Take(10)
                .ToList();

            MyActiveBorrows = MyBorrows.Count(b => !b.IsReturned);
            MyOverdue = MyBorrows.Count(b => !b.IsReturned && b.DueDate < DateTime.Now);
        }
        else
        {
            // Admin and Librarian see full stats
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
}