using AutoMapper;
using Ecomm.core.DTOs.Order;
using Ecomm.core.Entities;
using Ecomm.core.Entities.OrderEntities;
using Ecomm.core.Entities.WishListEntities;
using Ecomm.DTOs.CategoryDTOs;
using Ecomm.DTOs.DeliveryMethodDTOs;
using Ecomm.DTOs.OrderDTOs;
using Ecomm.DTOs.ProductDTOs;
using Ecomm.DTOs.WishListDTOs;

namespace Ecomm.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AddCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.Photos, o => o.MapFrom<ResolveImage>());

            CreateMap<AddProductDto, Product>()
                .ForMember(x => x.Photos, o => o.Ignore());

            CreateMap<UpdateProdcutDto, Product>()
               .ForMember(x => x.Photos, o => o.Ignore());

            CreateMap<DeliveryMethodDto, DeliveryMethod>().ReverseMap();
            CreateMap<AddDeliveryMethodDto, DeliveryMethod>().ReverseMap();
            CreateMap<UpdateDeliveryMethodDto, DeliveryMethod>().ReverseMap();

            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();
            CreateMap<Order, OrderDtoResponse>()
                .ForMember(x => x.DeliveryMethod, o => o.MapFrom(x => x.DeliveryMethod.Name));
            CreateMap<OrderItem, OrderItemDtoResponse>().ReverseMap();

            CreateMap<WishList, WishListDto>().ReverseMap();


            CreateMap<Product, HomeProductToReturnDto>()
                    .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));
                   


        }
    }
}
