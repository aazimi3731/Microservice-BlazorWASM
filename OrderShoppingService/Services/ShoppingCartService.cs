using OrderShoppingService.Models.Interfaces;
using MicroServices.Grpc;
using Grpc.Core;
using AutoMapper;
using ShoppingCartModel = SharedModels.Entities.ShoppingCart;
using ShoppingCartItemModel = SharedModels.Entities.ShoppingCartItem;
using ShoppingCart = MicroServices.Grpc.ShoppingCart;
using ShoppingCartItem = MicroServices.Grpc.ShoppingCartItem;

namespace OrderShoppingService.Services
{
  public class ShoppingCartService : ShoppingCartItems.ShoppingCartItemsBase
  {
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly IMapper _mapper;

    public ShoppingCartService(
      IShoppingCartRepository shoppingCartRepository_,
      IMapper mapper_
    ) {
      _shoppingCartRepository = shoppingCartRepository_;
      _mapper = mapper_;
    }

    public override async Task<CreateCartResponse> CreateCart(CreateCartRequest request_, ServerCallContext context)
    {
      var response = new CreateCartResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        response.IsSuccess = await _shoppingCartRepository.CreateShoppingCart(_mapper.Map<ShoppingCartModel>(request_.ShoppingCart));
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<GetCartResponse> GetCart(GetCartRequest request_, ServerCallContext context) 
    {
      var response = new GetCartResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        var shoppingCart = await _shoppingCartRepository.GetShoppingCart(request_.UserId);

        response.IsSuccess = false;
        if(shoppingCart != null && shoppingCart.GetType().GetProperties().Any())
        {
          response.ShoppingCart = _mapper.Map<ShoppingCart>(shoppingCart);
          response.IsSuccess = true;
        }
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<AddToCartResponse> AddToCart(AddToCartRequest request_, ServerCallContext context)
    {
      var response = new AddToCartResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        await _shoppingCartRepository.AddToCart(_mapper.Map<ShoppingCartItemModel>(request_.ShoppingCartItem));

        response.IsSuccess = true;
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<UpdateCartItemResponse> UpdateCartItem(UpdateCartItemRequest request_, ServerCallContext context)
    {
      var response = new UpdateCartItemResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }

      try
      {
        var amount = await _shoppingCartRepository.UpdateCartItem(request_.ShoppingCartId, request_.PieId, request_.Amount);

        response.IsSuccess = true;
        response.Amount = amount;
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<OrderedItemsResponse> OrderedItems(OrderedItemsRequest request_, ServerCallContext context)
    {
      var response = new OrderedItemsResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }

      try
      {
        response.IsSuccess =  await _shoppingCartRepository.OrederedShoppingCartItems(request_.ShoppingCartId);
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<GetCartItemseByCartIDResponse> GetCartItemsByCartID(GetCartItemsByCartIDRequest request_, ServerCallContext context)
    {
      var response = new GetCartItemseByCartIDResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }

      try
      {
        var shoppingCartItems = await _shoppingCartRepository.GetShoppingCartItems(request_.ShoppingCartId);

        if (shoppingCartItems.Any())
        {
          response.ShoppingCartItems.AddRange(_mapper.Map<List<ShoppingCartItem>>(shoppingCartItems));
        }

        response.IsSuccess = true;
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }

    public override async Task<RemoveItemsResponse> RemoveItems(RemoveItemsRequest request_, ServerCallContext context)
    {
      var response = new RemoveItemsResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }

      try
      {
        await _shoppingCartRepository.RemoveCartItems(request_.ShoppingCartId, request_.PieId);

        response.IsSuccess = true;
      }
      catch (Exception ex)
      {
        response.IsSuccess = false;
      }

      return response;
    }
  }
}
