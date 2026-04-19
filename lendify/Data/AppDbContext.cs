using Microsoft.EntityFrameworkCore;
using lendify.Models;

namespace lendify.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BorrowRecord>()
            .HasOne<Book>()
            .WithMany()
            .HasForeignKey(b => b.BookId);

        modelBuilder.Entity<BorrowRecord>()
            .HasOne<Member>()
            .WithMany()
            .HasForeignKey(b => b.MemberId);
    }
}
