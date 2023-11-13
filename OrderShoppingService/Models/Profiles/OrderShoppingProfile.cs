using AutoMapper;
using MicroServices.Grpc;
using OrderModel = SharedModels.Entities.Order;
using OrderDetailModel = SharedModels.Entities.OrderDetail;
using ShoppingCartItemModel = SharedModels.Entities.ShoppingCartItem;
using ShoppingCartModel = SharedModels.Entities.ShoppingCart;

namespace OrderShoppingService.Models.Profiles
{
    public class OrderShoppingProfile : Profile
  {
    public OrderShoppingProfile()
    {
      CreateMap<OrderDetail, OrderDetailModel>()
        .ForMember(dest => dest.Price, opts => opts.MapFrom(src => (decimal)src.Price));

      CreateMap<Order, OrderModel>()
        .ForMember(dest => dest.OrderTotal, opts => opts.MapFrom(src => (decimal)src.OrderTotal))
        .ForMember(dest => dest.OrderPlaced, opts => opts.MapFrom(src => src.OrderPlaced.ToDateTime()));

      CreateMap<ShoppingCartModel, ShoppingCart>().ReverseMap();
      CreateMap<ShoppingCartItemModel, ShoppingCartItem>().ReverseMap();
    }
  }
}
