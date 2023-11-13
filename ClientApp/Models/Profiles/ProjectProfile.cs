using AutoMapper;
using ClientApp.Models.Dtos;
using MicroServices.Grpc;
using PieModel = SharedModels.Entities.Pie;
using CategoryModel = SharedModels.Entities.Category;
using Timestamp = Google.Protobuf.WellKnownTypes.Timestamp;
using AutoMapper.Extensions.EnumMapping;

namespace ClientApp.Models.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<CategoryModel, CategoryDto>().ReverseMap();
            CreateMap<PieModel, PieDto>()
              .ForMember(dest => dest.CategoryId, opts => opts.MapFrom(src => src.Category.CategoryId)).ReverseMap();

            CreateMap<DateTime, Timestamp>().ConvertUsing(x => Timestamp.FromDateTime(x.ToUniversalTime()));
            CreateMap<Timestamp, DateTime>().ConvertUsing(x => x.ToDateTime());

            CreateMap<OrderDetailDto, OrderDetail>()
              .ForMember(dest => dest.Price, opts => opts.MapFrom(src => (double)src.Price));
            CreateMap<OrderDto, Order>()
              .ForMember(dest => dest.OrderTotal, opts => opts.MapFrom(src => (double)src.OrderTotal))
              .ForMember(dest => dest.OrderPlaced, opts => opts.MapFrom(src => src.OrderPlaced));

            CreateMap<OrderDetail, OrderDetailDto>()
        .ForMember(dest => dest.Price, opts => opts.MapFrom(src => (decimal)src.Price));
            CreateMap<Order, OrderDto>()
              .ForMember(dest => dest.OrderTotal, opts => opts.MapFrom(src => (decimal)src.OrderTotal))
              .ForMember(dest => dest.OrderPlaced, opts => opts.MapFrom(src => src.OrderPlaced));

            CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();
            CreateMap<ShoppingCartItemDto, ShoppingCartItem>().ReverseMap();

            CreateMap<GenderDto, Gender>().ConvertUsingEnumMapping(opt => opt.MapValue(GenderDto.Other, Gender.Other)).ReverseMap();
            CreateMap<MaritalStatusDto, MaritalStatus>().ConvertUsingEnumMapping(opt => opt.MapValue(MaritalStatusDto.Other, MaritalStatus.Other)).ReverseMap();
            CreateMap<CountryDto, Country>().ReverseMap();
            CreateMap<JobCategoryDto, JobCategory>().ReverseMap();
            CreateMap<EmployeeDto, Employee>().ReverseMap();
        }
    }
}
