using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using MicroServices.Grpc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static MicroServices.Grpc.ShoppingCartItems;
using ShoppingCartItemModel = SharedModels.Entities.ShoppingCartItem;

namespace ClientApp.Pages
{
  public partial class ShoppingCart : ComponentBase
  {
    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Inject]
    private IShoppingCartItemService ShoppingCartItemService { get; set; }

    [Inject]
    private IProductService ProductService { get; set; }

    //[Inject]
    //private UserManager<IdentityUser> UserManager { get; set; }

    [Inject]
    private ShoppingCartItems.ShoppingCartItemsClient CartItemsClient { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    private ClaimsPrincipal? _currentUser;

    public IEnumerable<ShoppingCartItemDto>? Items { get; set; }

    public decimal Total { get; set; }

    protected override async Task OnInitializedAsync()
    {
      if(AuthenticationState is not null)
      {
        var authenticationState = await AuthenticationState;

        _currentUser = authenticationState.User;
      }

      await ShoppingCartItemService.SetCart(_currentUser);

      var cartItems = await ShoppingCartItemService.GetShoppingCartItems();

      Items = cartItems.Where(c => !c.IsOrdered).ToList();

      Total = await ShoppingCartTotal();

      await base.OnInitializedAsync();
    }

    public async Task<decimal> ShoppingCartTotal()
    {
      var shoppingCartItems = await ShoppingCartItemService.GetShoppingCartItems();

      if (shoppingCartItems.Any())
      {
        foreach (var item in shoppingCartItems)
        {
          item.Pie = await ProductService.GetPieById(item.PieId);
        }

        return shoppingCartItems.Select(c => c.Pie.Price * c.Amount).Sum();
      }

      return decimal.Zero;
    }

    //private async Task CreateCart()
    //{
    //  var userName = UserManager.GetUserAsync(_currentUser).Result?.UserName;

    //  if (!string.IsNullOrEmpty(userName))
    //  {
    //    var shoppingCart = await ShoppingCartItemService.GetShoppingCart(_currentUser);

    //    if (string.IsNullOrEmpty(shoppingCart))
    //    {
    //      var createCartRequest = new CreateCartRequest
    //      {
    //        ShoppingCart = new MicroServices.Grpc.ShoppingCart
    //        {
    //          ShoppingCartId = ShoppingCartItemService.ShoppingCartId,
    //          UserId = UserManager.GetUserAsync(_currentUser).Result?.UserName
    //        }
    //      };

    //      var cartResponse = await CartItemsClient.CreateCartAsync(createCartRequest);

    //      if (cartResponse == null || !cartResponse.IsSuccess)
    //      {
    //        throw new Exception("Do not receive the response or the response is null for creating shopping cart.");
    //      }
    //    }
    //  }
    //}
  }
}
