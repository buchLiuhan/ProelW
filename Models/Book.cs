using System.ComponentModel.DataAnnotations;
namespace LibraryManagementSystem.Models;
public class Book
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int TotalCopies { get; set; } = 1;
    public int AvailableCopies { get; set; } = 1;
    public DateTime AddedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}