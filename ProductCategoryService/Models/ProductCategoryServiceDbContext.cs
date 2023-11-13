using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;

namespace ProductCategoryService.Models
{
  public class ProductCategoryServiceDbContext : DbContext
  {
    public ProductCategoryServiceDbContext(DbContextOptions<ProductCategoryServiceDbContext> options)
      : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Pie> Pies { get; set; }
  }
}
