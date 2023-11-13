using ClientApp.Models.Dtos;
using ClientApp.Interfaces;
using MicroServices.Grpc;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Blazored.SessionStorage;

namespace ClientApp.Services
{
    public class ShoppingCartItemService : IShoppingCartItemService
    {
        private static IServiceProvider _services;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ShoppingCartItems.ShoppingCartItemsClient _cartItemsClient;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public string? ShoppingCartId { get; private set; }

        public ShoppingCartItemService(
          UserManager<IdentityUser> userManager_,
          ShoppingCartItems.ShoppingCartItemsClient cartItemsClient_,
          IProductService productService_,
          IMapper mapper_
        )
        {
            _userManager = userManager_;
            _cartItemsClient = cartItemsClient_;
            _productService = productService_;
            _mapper = mapper_;
        }

        public static ShoppingCartItemService SetServices(
          IServiceProvider services_,
          UserManager<IdentityUser> userManager_,
          ShoppingCartItems.ShoppingCartItemsClient cartItemsClient_,
          IProductService productService_,
          IMapper mapper_
        )
        {
            _services = services_;

            return new ShoppingCartItemService(userManager_, cartItemsClient_, productService_, mapper_);
        }

        public async Task SetCart(ClaimsPrincipal? currentUser_)
        {
            var session = _services.GetRequiredService<ISessionStorageService>();

            string? cartId = await session.GetItemAsStringAsync("CartId");

            if (string.IsNullOrEmpty(cartId))
            {
                var shoppingCartId = string.Empty;
                if (currentUser_ != null)
                {
                    shoppingCartId = await GetShoppingCart(currentUser_);
                }

                cartId = !string.IsNullOrEmpty(shoppingCartId) ? shoppingCartId : Guid.NewGuid().ToString();

                await session.SetItemAsStringAsync("CartId", cartId);
            }

            ShoppingCartId = cartId;
        }

        public async Task<bool> SetCartForCurrentUser(ClaimsPrincipal currentUser_)
        {
            if (currentUser_ != null)
            {
                try
                {
                    var userShoppingCartId = await GetShoppingCart(currentUser_);

                    var userName = _userManager.GetUserAsync(currentUser_).Result?.UserName;
                    if (string.IsNullOrEmpty(userShoppingCartId) && !string.IsNullOrEmpty(userName))
                    {
                        await CreateCart(currentUser_);
                    }

                    var isOrderContinued = true;
                    if (!string.IsNullOrEmpty(ShoppingCartId) && !string.IsNullOrEmpty(userShoppingCartId) && !ShoppingCartId.Equals(userShoppingCartId))
                    {
                        var shoppingCartItems = await GetShoppingCartItems();

                        foreach (var item in shoppingCartItems)
                        {
                            try
                            {
                                var selectPie = _productService.GetPieById(item.PieId);

                                if (selectPie == null)
                                {
                                    throw new Exception("Do not receive any response or the response is null for the Pie to be added to the cart.");
                                }
                                var request = new AddToCartRequest
                                {
                                    ShoppingCartItem = new ShoppingCartItem()
                                    {
                                        ShoppingCartId = userShoppingCartId,
                                        PieId = item.PieId,
                                        Amount = item.Amount,
                                        Price = decimal.ToDouble(item.Price),
                                        IsOrdered = false
                                    }
                                };

                                var response = await _cartItemsClient.AddToCartAsync(request);

                                if (response == null || !response.IsSuccess)
                                {
                                    throw new Exception("Do not receive any response or the response is null for adding to the cart.");
                                }

                                var removeRequest = new RemoveItemsRequest()
                                {
                                    ShoppingCartId = ShoppingCartId,
                                    PieId = item.PieId
                                };

                                var removeResponse = await _cartItemsClient.RemoveItemsAsync(removeRequest);

                                if (removeResponse == null || !removeResponse.IsSuccess)
                                {
                                    throw new Exception("Do not receive any response or the response is null for removing the shopping cart item.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                isOrderContinued = false;
                            }
                        }

                        if (isOrderContinued)
                        {
                            ShoppingCartId = userShoppingCartId;

                            var session = _services.GetRequiredService<ISessionStorageService>();
                            await session.SetItemAsStringAsync("CartId", ShoppingCartId);
                        }

                        return isOrderContinued;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            return true;
        }

        public async Task<string> GetShoppingCart(ClaimsPrincipal currentUser_)
        {
            var userName = _userManager.GetUserAsync(currentUser_).Result?.UserName;
            if (string.IsNullOrEmpty(userName))
            {
                return string.Empty;
            }

            var request = new GetCartRequest
            {
                UserId = userName
            };

            try
            {
                var response = await _cartItemsClient.GetCartAsync(request);

                if (response == null)
                {
                    throw new Exception("Do not receive the response or the response is null.");
                }

                if (response.IsSuccess)
                {
                    return response.ShoppingCart.ShoppingCartId;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return string.Empty;
            }
        }

        public async Task<List<ShoppingCartItemDto>> GetShoppingCartItems()
        {
            if (string.IsNullOrEmpty(ShoppingCartId) || ShoppingCartId.Equals(Guid.Empty.ToString()))
            {
                return new List<ShoppingCartItemDto>();
            }

            var request = new GetCartItemsByCartIDRequest
            {
                ShoppingCartId = ShoppingCartId
            };

            try
            {
                var response = await _cartItemsClient.GetCartItemsByCartIDAsync(request);

                if (response == null)
                {
                    throw new Exception("Do not receive the response or the response is null.");
                }

                if (response.IsSuccess && response.ShoppingCartItems.Any())
                {
                    var shoppingCartItems = _mapper.Map<List<ShoppingCartItemDto>>(response.ShoppingCartItems);

                    foreach (var item in shoppingCartItems)
                    {
                        var selectPie = await _productService.GetPieById(item.PieId);

                        if (selectPie != null)
                        {
                            item.Pie = selectPie;
                        }
                    }

                    return shoppingCartItems;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new List<ShoppingCartItemDto>();
        }

        private async Task CreateCart(ClaimsPrincipal currentUser_)
        {
            try
            {
                var userName = _userManager.GetUserAsync(currentUser_).Result?.UserName;

                var createCartRequest = new CreateCartRequest
                {
                    ShoppingCart = new ShoppingCart()
                    {
                        ShoppingCartId = ShoppingCartId,
                        UserId = _userManager.GetUserAsync(currentUser_).Result?.UserName
                    }
                };

                var cartResponse = await _cartItemsClient.CreateCartAsync(createCartRequest);

                if (cartResponse == null || !cartResponse.IsSuccess)
                {
                    throw new Exception("Do not receive the response or the response is null for creating shopping cart.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
