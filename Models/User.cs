using System.ComponentModel.DataAnnotations;
namespace LibraryManagementSystem.Models;
public class User
{
    public int Id { get; set; }
    [Required]
    public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Student";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}