using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryManagementSystem.Pages.Users;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public List<User> Users { get; set; } = new();
    public string Search { get; set; } = string.Empty;

    public IndexModel(AppDbContext db) => _db = db;

    public void OnGet(string search = "")
    {
        Search = search;
        var query = _db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u =>
                u.FullName.Contains(search) ||
                u.Email.Contains(search));

        Users = query.OrderBy(u => u.FullName).ToList();
    }

    public IActionResult OnPostDelete(int id)
    {
        var user = _db.Users.Find(id);
        if (user != null && user.Email != "admin@libman.com")
        {
            _db.Users.Remove(user);
            _db.SaveChanges();
            TempData["Success"] = $"{user.FullName} has been deleted.";
        }
        return RedirectToPage();
    }

    //dddddd
}