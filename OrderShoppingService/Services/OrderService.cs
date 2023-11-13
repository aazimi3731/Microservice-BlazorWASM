using OrderShoppingService.Models.Interfaces;
using MicroServices.Grpc;
using OrderModel = SharedModels.Entities.Order;
using Grpc.Core;
using AutoMapper;

namespace OrderShoppingService.Services
{
  public class OrderService : Orders.OrdersBase
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly IMapper _mapper;

    public OrderService(
      IOrderRepository orderRepository_,
      IShoppingCartRepository shoppingCartRepository_,
      IMapper mapper_
    ) {
      _orderRepository = orderRepository_;
      _shoppingCartRepository = shoppingCartRepository_;
      _mapper = mapper_;
    }

    public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request_, ServerCallContext context)
    {
      var response = new CreateOrderResponse();

      if (request_ == null)
      {
        response.IsSuccess = false;

        return response;
      }
      try
      {
        var shoppingCartItems = await _shoppingCartRepository.GetShoppingCartItems(request_.ShoppingCartId);

        await _orderRepository.CreateOrder(_mapper.Map<OrderModel>(request_.Order), shoppingCartItems);

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