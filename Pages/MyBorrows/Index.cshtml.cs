using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagementSystem.Pages.MyBorrows;

[Authorize(Roles = "Student")]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public List<BorrowRecord> Records { get; set; } = new();
    public int ActiveCount { get; set; }
    public int ReturnedCount { get; set; }
    public int OverdueCount { get; set; }

    public IndexModel(AppDbContext db) => _db = db;

    public void OnGet()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out int userId)) return;

        Records = _db.BorrowRecords
            .Include(b => b.Book)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BorrowedAt)
            .ToList();

        ActiveCount = Records.Count(r => !r.IsReturned);
        ReturnedCount = Records.Count(r => r.IsReturned);
        OverdueCount = Records.Count(r => !r.IsReturned && r.DueDate < DateTime.Now);
    }
}