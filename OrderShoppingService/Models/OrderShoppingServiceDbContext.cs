using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;

namespace OrderShoppingService.Models
{
  public class OrderShoppingServiceDbContext : DbContext
  {
    public OrderShoppingServiceDbContext(DbContextOptions<OrderShoppingServiceDbContext> options)
      : base(options)
    {
    }

    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
  }
}
