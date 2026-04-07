using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LibraryManagementSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed default admin
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            FullName = "System Admin",
            Email = "admin@libman.com",
            Password = "admin123",
            Role = "Admin",
            CreatedAt = new DateTime(2025, 1, 1)
        });
    }
}