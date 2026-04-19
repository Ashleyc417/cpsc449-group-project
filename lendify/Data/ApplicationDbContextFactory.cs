using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace lendify.Data
{
  public class ApplicationDbContextFactory: IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    public ApplicationDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
      optionsBuilder.UseSqlite("Data Source=bookmanager.db");

      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }
}

