using SharedModels.Entities;
using OrderShoppingService.Models.Interfaces;

namespace OrderShoppingService.Models.Repositories
{
  public class OrderRepository : IOrderRepository
  {
    private readonly OrderShoppingServiceDbContext _orderShoppingServiceDbContext;

    public OrderRepository(OrderShoppingServiceDbContext orderShoppingServiceDbContext_)
    {
      _orderShoppingServiceDbContext = orderShoppingServiceDbContext_;
    }

    public async Task CreateOrder(Order order_, List<ShoppingCartItem> shoppingCartItems_)
    {
      order_.OrderPlaced = DateTime.Now;

      order_.OrderTotal = order_.OrderTotal;

      order_.OrderDetails = new List<OrderDetail>();

      //adding the order with its details

      foreach (ShoppingCartItem? shoppingCartItem in shoppingCartItems_)
      {
        var orderDetail = new OrderDetail
        {
          Amount = shoppingCartItem.Amount,
          PieId = shoppingCartItem.PieId,
          Price = shoppingCartItem.Price
        };

        order_.OrderDetails.Add(orderDetail);
      }

      await _orderShoppingServiceDbContext.Orders.AddAsync(order_);

      await _orderShoppingServiceDbContext.SaveChangesAsync();
    }
  }
}
