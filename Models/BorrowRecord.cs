namespace LibraryManagementSystem.Models;

public class BorrowRecord
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public DateTime BorrowedAt { get; set; } = DateTime.Now;

    public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);

    public DateTime? ReturnedAt { get; set; }

    public bool IsReturned => ReturnedAt.HasValue;
}