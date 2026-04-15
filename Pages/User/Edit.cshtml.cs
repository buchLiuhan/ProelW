using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserModel = LibraryManagementSystem.Models.User;

namespace LibraryManagementSystem.Pages.Users;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    public UserModel SelectedUser { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public EditModel(AppDbContext db) => _db = db;

    public IActionResult OnGet(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null) return RedirectToPage("/Users/Index");
        SelectedUser = user;
        return Page();
    }

    public IActionResult OnPost(int id, string role, string? newPassword)
    {
        var user = _db.Users.Find(id);
        if (user == null) return RedirectToPage("/Users/Index");

        user.Role = role;

        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            if (newPassword.Length < 6)
            {
                SelectedUser = user;
                ErrorMessage = "Password must be at least 6 characters.";
                return Page();
            }
            user.Password = newPassword;
        }

        _db.SaveChanges();
        TempData["Success"] = $"{user.FullName}'s profile has been updated.";
        return Redirect("/Users");
    }
}