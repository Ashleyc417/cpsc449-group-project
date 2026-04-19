using Microsoft.EntityFrameworkCore;
using lendify.Models;

namespace lendify.Data
{
  public class ApplicationDbContext: DbContext
  {

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ): base(options) {}
    
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();
  }
}
