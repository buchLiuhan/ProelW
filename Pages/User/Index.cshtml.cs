using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserModel = LibraryManagementSystem.Models.User;

namespace LibraryManagementSystem.Pages.Users;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public List<UserModel> Users { get; set; } = new();
    public string Search { get; set; } = string.Empty;
    public bool ShowInactive { get; set; }

    public IndexModel(AppDbContext db) => _db = db;

    public void OnGet(string search = "", bool showInactive = false)
    {
        Search = search;
        ShowInactive = showInactive;

        var query = _db.Users.AsQueryable();

        if (!showInactive)
            query = query.Where(u => u.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u =>
                u.FullName.Contains(search) ||
                u.Email.Contains(search));

        Users = query.OrderBy(u => u.FullName).ToList();
    }
}