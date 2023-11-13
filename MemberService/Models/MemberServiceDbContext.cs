using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;

namespace MemberService.Models
{
  public class MemberServiceDbContext : DbContext
  {
    public MemberServiceDbContext(DbContextOptions<MemberServiceDbContext> options)
      : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }
    public DbSet<Country> Countries { get; set; }
  }
}
