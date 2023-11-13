using ClientApp.Interfaces;
using ClientApp.Models.Dtos;
using ClientApp.Services;
using MicroServices.Grpc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharedModels.Entities;
using System.Security.Claims;
using ShoppingCartItemModel = SharedModels.Entities.ShoppingCartItem;

namespace ClientApp.Components
{
  public partial class UpdateCartForm : ComponentBase
  {
    [Inject]
    public IShoppingCartItemService ShoppingCartItemService { get; set; }

    [Inject]
    public IProductService ProductService { get; set; }

    [Inject]
    public ShoppingCartItems.ShoppingCartItemsClient CartItemsClient { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Parameter]
    public ShoppingCartItemDto? ShoppingCartItem { get; set; } = new ShoppingCartItemDto();

    protected string Amount = string.Empty;

    protected override async Task OnInitializedAsync()
    {
      Amount = ShoppingCartItem.Amount.ToString();

      await base.OnInitializedAsync();
    }

    public async Task UpdateShoppingCart()
    {
      try
      {
        var selectPie = await ProductService.GetPieById(ShoppingCartItem.PieId);

        if (selectPie != null && !string.IsNullOrEmpty(ShoppingCartItem.ShoppingCartId))
        {
          var request = new UpdateCartItemRequest()
          {
            ShoppingCartId = ShoppingCartItem.ShoppingCartId,
            PieId = ShoppingCartItem.PieId,
            Amount = int.Parse(Amount)
          };

          var response = await CartItemsClient.UpdateCartItemAsync(request);

          if (response == null || !response.IsSuccess)
          {
            throw new Exception("Do not receive the response or the response is null.");
          }

          NavigationManager.NavigateTo("/shoppingcart", true);
        }

        throw new Exception("Do not receive the response for the Pie or the response is null for the Pie.");
      }
      catch (Exception ex)
      {
        NavigationManager.NavigateTo("404page");
      }
    }

    public async Task RemoveShoppingCart()
    {
      try
      {
        var request = new RemoveItemsRequest()
        {
          ShoppingCartId = ShoppingCartItem.ShoppingCartId,
          PieId = ShoppingCartItem.PieId
        };

        var response = await CartItemsClient.RemoveItemsAsync(request);

        if (response == null || !response.IsSuccess)
        {
          throw new Exception("Do not receive the response or the response is null.");
        }

        var items = await ShoppingCartItemService.GetShoppingCartItems();

        if (items.Any())
        {
          NavigationManager.NavigateTo("/shoppingcart");
        }

        NavigationManager.NavigateTo("/");
      }
      catch (Exception ex)
      {
        NavigationManager.NavigateTo("404page");
      }
    }
  }
}
