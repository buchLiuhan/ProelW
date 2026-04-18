using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserModel = LibraryManagementSystem.Models.User;

namespace LibraryManagementSystem.Pages.Users;

[Authorize(Roles = "Admin")]
public class RestoreModel : PageModel
{
    private readonly AppDbContext _db;
    public UserModel SelectedUser { get; set; } = new();

    public RestoreModel(AppDbContext db) => _db = db;

    public IActionResult OnGet(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null) return Redirect("/Users");
        SelectedUser = user;
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null) return Redirect("/Users");

        user.IsActive = true;
        _db.SaveChanges();

        TempData["Success"] = $"{user.FullName}'s account has been restored.";
        return Redirect("/Users");
    }
}