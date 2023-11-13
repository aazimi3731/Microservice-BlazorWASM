using ClientApp.Models.Dtos;
using System.Security.Claims;

namespace ClientApp.Interfaces
{
  public interface IShoppingCartItemService
  {
    string? ShoppingCartId { get; }

    Task SetCart(ClaimsPrincipal? currentUser_);

    Task<bool> SetCartForCurrentUser(ClaimsPrincipal currentUser_);

    Task<string> GetShoppingCart(ClaimsPrincipal currentUser_);

    Task<List<ShoppingCartItemDto>> GetShoppingCartItems();
  }
}
