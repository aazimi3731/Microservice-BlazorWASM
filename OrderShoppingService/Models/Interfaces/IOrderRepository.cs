using SharedModels.Entities;

namespace OrderShoppingService.Models.Interfaces
{
  public interface IOrderRepository
  {

    Task CreateOrder(Order order_, List<ShoppingCartItem> shoppingCartItems_);
  }
}
