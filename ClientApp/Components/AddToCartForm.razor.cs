using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using ClientApp.Services;
using MicroServices.Grpc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using ShoppingCartItem = MicroServices.Grpc.ShoppingCartItem;

namespace ClientApp.Components
{
  public partial class AddToCartForm : ComponentBase
  {
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationState { get; set; }

    [Inject]
    public IShoppingCartItemService ShoppingCartItemService { get; set; }

    [Inject]
    public IProductService ProductService { get; set; }

    [Inject]
    public ShoppingCartItems.ShoppingCartItemsClient CartItemsClient { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Parameter]
    public PieDto? Pie { get; set; }

    public ShoppingCartItemDto? ShoppingCartItem { get; set; } = new ShoppingCartItemDto();

    protected string Amount = string.Empty;

    protected override async Task OnInitializedAsync()
    {
      ShoppingCartItem.Pie = Pie;
      ShoppingCartItem.PieId = Pie.PieId;
      ShoppingCartItem.Price = Pie.Price;

      ClaimsPrincipal? currentUser = null;
      if (AuthenticationState is not null)
      {
        var authenticationState = await AuthenticationState;

        currentUser = authenticationState.User;
      }

      ShoppingCartItemService.SetCart(currentUser);

      Amount = "1";

      await base.OnInitializedAsync();
    }


    public async Task AddToShoppingCart()
    {
      try
      {
        var selectPie = await ProductService.GetPieById(ShoppingCartItem.PieId);

        if (selectPie != null && !string.IsNullOrEmpty(ShoppingCartItemService.ShoppingCartId))
        {
          var request = new MicroServices.Grpc.AddToCartRequest
          {
            ShoppingCartItem = new ShoppingCartItem()
            {
              ShoppingCartId = ShoppingCartItemService.ShoppingCartId,
              PieId = ShoppingCartItem.PieId,
              Amount = int.Parse(Amount),
              Price = Decimal.ToDouble(ShoppingCartItem.Price),
              IsOrdered = false
            }
        };

          var response = await CartItemsClient.AddToCartAsync(request);

          if (response == null || !response.IsSuccess)
          {
            throw new Exception("Do not receive the response or the response is null for adding to the cart.");
          }

          NavigationManager.NavigateTo("/shoppingcart", true);
        }

        throw new Exception("Do not receive the response for the Pie or the response is null for the Pie to adding to the cart.");
      }
      catch (Exception ex)
      {
        NavigationManager.NavigateTo("404page");
      }
    }
  }
}
