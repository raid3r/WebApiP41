using AutoMapper;
using WebApiP41.Models;
using WebApiP41.Models.DTO;

namespace WebApiP41.Models.MappingProfiles;


public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductView>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMapForOrderToOrderViewModel();

        CreateMap<CreaterOrderRequest, Order>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom<ProductResolver>())
            .ForMember(dest => dest.ProductQuantity, opt => opt.MapFrom(src => src.ProductQuantity))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.ProductPrice))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.OrderDateTime, opt => opt.Ignore())
            .ForMember(dest => dest.IsCompleted, opt => opt.Ignore())
            .ForMember(dest => dest.OrderNumber, opt => opt.Ignore());
    }


    private void CreateMapForOrderToOrderViewModel()
    {
        CreateMap<Order, OrderViewModel>()
            
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrderNumberFormatted, opt => opt.MapFrom(src => "ORD-" + src.OrderNumber.ToString("D6")))
            .ForMember(dest => dest.ProductQuantity, opt => opt.MapFrom(src => src.ProductQuantity))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.ProductPrice))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.ProductQuantity * src.ProductPrice))
            .ForMember(dest => dest.OrderDateTime, opt => opt.MapFrom(src => src.OrderDateTime))
            .ForMember(dest => dest.CompletedStatus, opt => opt.MapFrom(src => src.IsCompleted ? "Completed" : "Pending"))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
            ;
    }
}
