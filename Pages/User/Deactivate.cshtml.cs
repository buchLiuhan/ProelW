using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserModel = LibraryManagementSystem.Models.User;

namespace LibraryManagementSystem.Pages.Users;

[Authorize(Roles = "Admin")]
public class DeactivateModel : PageModel
{
    private readonly AppDbContext _db;
    public UserModel SelectedUser { get; set; } = new();
    public bool HasActiveBorrows { get; set; }

    public DeactivateModel(AppDbContext db) => _db = db;

    public IActionResult OnGet(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null) return Redirect("/Users");
        SelectedUser = user;
        HasActiveBorrows = _db.BorrowRecords
            .Any(b => b.UserId == id && b.ReturnedAt == null);
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null) return Redirect("/Users");

        bool hasActiveBorrows = _db.BorrowRecords
            .Any(b => b.UserId == id && b.ReturnedAt == null);

        if (hasActiveBorrows)
        {
            TempData["Error"] = "Cannot deactivate — user has unreturned books.";
            return Redirect("/Users");
        }

        user.IsActive = false;
        _db.SaveChanges();

        TempData["Success"] = $"{user.FullName}'s account has been deactivated.";
        return Redirect("/Users");
    }
}