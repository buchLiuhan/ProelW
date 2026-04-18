using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryManagementSystem.Pages;

[Authorize]
public class PolicyModel : PageModel
{
    public void OnGet() { }
}