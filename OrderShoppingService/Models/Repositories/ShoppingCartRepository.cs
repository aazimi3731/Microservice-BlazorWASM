using Microsoft.EntityFrameworkCore;
using SharedModels.Entities;
using OrderShoppingService.Models.Interfaces;

namespace OrderShoppingService.Models.Repositories
{
  public class ShoppingCartRepository : IShoppingCartRepository
  {
    private readonly OrderShoppingServiceDbContext _orderShoppingServiceDbContext;

    public ShoppingCartRepository(OrderShoppingServiceDbContext orderShoppingServiceDbContext_)
    {
      _orderShoppingServiceDbContext = orderShoppingServiceDbContext_;
    }

    public async Task<bool> CreateShoppingCart(ShoppingCart cart_)
    {
      var shoppingCart = new ShoppingCart
      {
        ShoppingCartId = cart_.ShoppingCartId,
        UserId = cart_.UserId
      };

      await _orderShoppingServiceDbContext.ShoppingCarts.AddAsync(shoppingCart);

      return await _orderShoppingServiceDbContext.SaveChangesAsync() > 0;
    }

    public async Task<ShoppingCart?> GetShoppingCart(string userId_) => await _orderShoppingServiceDbContext.ShoppingCarts
    .Where(s => s.UserId == userId_).FirstOrDefaultAsync();

    public async Task AddToCart(ShoppingCartItem shoppingCartItem_)
    {
      var shoppingCartItem = await GetCartItem(shoppingCartItem_.ShoppingCartId, shoppingCartItem_.PieId);

      if (shoppingCartItem != null && shoppingCartItem.GetType().GetProperties().Any())
      {
        shoppingCartItem.Amount++;
      }
      else
      {
        shoppingCartItem = new ShoppingCartItem
        {
          ShoppingCartId = shoppingCartItem_.ShoppingCartId,
          PieId = shoppingCartItem_.PieId,
          Amount = shoppingCartItem_.Amount,
          Price = shoppingCartItem_.Price,
          IsOrdered = shoppingCartItem_.IsOrdered
        };

        await _orderShoppingServiceDbContext.ShoppingCartItems.AddAsync(shoppingCartItem);
      }

      await _orderShoppingServiceDbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateCartItem(string shoppingCartId_, int pieId_, int amount_)
    {
      var shoppingCartItem = await GetCartItem(shoppingCartId_, pieId_);

      var localAmount = 0;

      if (shoppingCartItem != null)
      {
        shoppingCartItem.Amount = amount_;
        localAmount = shoppingCartItem.Amount;

        await _orderShoppingServiceDbContext.SaveChangesAsync();
      }

      return await Task.FromResult(localAmount);
    }

    public async Task<bool> OrederedShoppingCartItems(string shoppingCartId_)
    {
      try
      {
        await Task.Run(() => GetShoppingCartItems(shoppingCartId_).Result.ForEach(sh => sh.IsOrdered = true));

        await _orderShoppingServiceDbContext.SaveChangesAsync();

        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public async Task<List<ShoppingCartItem>> GetShoppingCartItems(string shoppingCartId_) => await _orderShoppingServiceDbContext.ShoppingCartItems
        .Where(s => s.ShoppingCartId == shoppingCartId_ && !s.IsOrdered).ToListAsync();

    public async Task RemoveCartItems(string shoppingCartId_, int pieId_)
    {
      await Task.Run(() => _orderShoppingServiceDbContext.ShoppingCartItems.RemoveRange(GetShoppingCartItems(shoppingCartId_)
        .Result.Where(sh => sh.PieId == pieId_)));

      await _orderShoppingServiceDbContext.SaveChangesAsync();
    }

    private async Task<ShoppingCartItem?> GetCartItem(string shoppingCartId_, int pieId_) => await _orderShoppingServiceDbContext.ShoppingCartItems
        .SingleOrDefaultAsync(s => s != null && s.PieId == pieId_ && s.ShoppingCartId == shoppingCartId_ && !s.IsOrdered);
  }
}
