using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LibraryManagementSystem.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly AppDbContext _db;
    public string ErrorMessage { get; set; } = string.Empty;

    public LoginModel(AppDbContext db) => _db = db;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(string email, string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        if (user == null)
        {
            ErrorMessage = "Invalid email or password.";
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return RedirectToPage("/Dashboard/Index");
    }
}