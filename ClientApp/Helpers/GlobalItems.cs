using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using System.Security.Claims;

namespace ClientApp.Helpers
{
  public class GlobalItems
  {
    private readonly IShoppingCartItemService _shoppingCartItemService;

    public GlobalItems(IShoppingCartItemService shoppingCartItemService_)
    {
      _shoppingCartItemService = shoppingCartItemService_;
    }

    public async Task<List<ShoppingCartItemDto>> ShoppingCartItems(ClaimsPrincipal currentUser_) 
    {
      await _shoppingCartItemService.SetCart(currentUser_);

      var cartItems = await _shoppingCartItemService.GetShoppingCartItems();

      return cartItems ?? new List<ShoppingCartItemDto>();
    }
  }
}
