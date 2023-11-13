using SharedModels.Entities;

namespace OrderShoppingService.Models.Interfaces
{
  public interface IShoppingCartRepository
  {
    Task<bool> CreateShoppingCart(ShoppingCart cart_);

    Task<ShoppingCart?> GetShoppingCart(string userId_);

    Task AddToCart(ShoppingCartItem shoppingCartItem_);

    Task<int> UpdateCartItem(string shoppingCartId_, int pieId_, int amount_);

    Task<bool> OrederedShoppingCartItems(string shoppingCartId_);

    Task<List<ShoppingCartItem>> GetShoppingCartItems(string shoppingCartId_);

    Task RemoveCartItems(string shoppingCartId_, int pieId_);
  }
}
