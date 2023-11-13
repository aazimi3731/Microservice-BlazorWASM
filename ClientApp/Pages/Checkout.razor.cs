//using MicroServices.Grpc;
using AutoMapper;
using ClientApp.Models.Dtos;
using ClientApp.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using MicroServices.Grpc;
using System.Web.WebPages.Html;

namespace ClientApp.Pages
{
  public partial class Checkout : ComponentBase
  {
    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Inject]
    private Orders.OrdersClient OrdersClient { get; set; }

    [Inject]
    private IShoppingCartItemService ShoppingCartItemService { get; set; }

    [Inject]
    private ShoppingCartItems.ShoppingCartItemsClient ShoppingCartItemsClient { get; set; }

    [Inject]
    private IMapper Mapper { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    public OrderDto Order { get; set; } = new OrderDto();

    private ClaimsPrincipal? CurrentUser;

    //used to store state of screen
    protected string Message = string.Empty;
    protected string StatusClass = string.Empty;
    protected bool Saved;

    protected override async Task OnInitializedAsync()
    {
      Saved = false;

      if (AuthenticationState is not null)
      {
        var authenticationState = await AuthenticationState;

        CurrentUser = authenticationState.User;

        ShoppingCartItemService.SetCart(CurrentUser);
      }

      await base.OnInitializedAsync();
    }

    public async void HandleValidSubmit()
    {
      Saved = false;

      try
      {
//        await ShoppingCartItemService.SetCartForCurrentUser(CurrentUser);

        var shoppingCartItems = await ShoppingCartItemService.GetShoppingCartItems();

        if (!shoppingCartItems.Any())
        {
          throw new Exception("There is no any shopping cart.");
        }

        if (string.IsNullOrEmpty(Order.AddressLine2))
        {
          Order.AddressLine2 = string.Empty;
        }

        var orderRequest = new CreateOrderRequest
        {
          Order = Mapper.Map<Order>(Order),
          ShoppingCartId = ShoppingCartItemService.ShoppingCartId
        };

        await OrdersClient.CreateOrderAsync(orderRequest);

        var orderedItemsRequest = new OrderedItemsRequest()
        {
          ShoppingCartId = ShoppingCartItemService.ShoppingCartId
        };

        var orderedItemsResponse = await ShoppingCartItemsClient.OrderedItemsAsync(orderedItemsRequest);

        if (orderedItemsResponse == null || !orderedItemsResponse.IsSuccess)
        {
          throw new Exception("Do not receive the response or the response is null for setting the items as ordered.");
        }

        StatusClass = "alert-success";
        Message = "New employee added successfully.";
        Saved = true;

        NavigationManager.NavigateTo("/checkoutcomplete", true);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        StatusClass = "alert-danger";
        Message = "Something went wrong adding the new employee. Please try again.";
        Saved = false;
      }
    }

    protected void HandleInvalidSubmit()
    {
      StatusClass = "alert-danger";
      Message = "There are some validation errors. Please try again.";
    }

    protected void NavigateToHomePage()
    {
      NavigationManager.NavigateTo("/employeeslist");
    }
  }
}
