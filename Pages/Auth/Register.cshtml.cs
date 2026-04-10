using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserModel = LibraryManagementSystem.Models.User;

namespace LibraryManagementSystem.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _db;
    public string ErrorMessage { get; set; } = string.Empty;

    public RegisterModel(AppDbContext db) => _db = db;

    public void OnGet() { }

    public IActionResult OnPost(string fullName, string email, string password, string role)
    {
        if (_db.Users.Any(u => u.Email == email))
        {
            ErrorMessage = "Email already registered.";
            return Page();
        }

        var user = new UserModel
        {
            FullName = fullName,
            Email = email,
            Password = password,
            Role = role == "Librarian" ? "Librarian" : "Student"
        };

        _db.Users.Add(user);
        _db.SaveChanges();
        return RedirectToPage("/Auth/Login");
    }
}