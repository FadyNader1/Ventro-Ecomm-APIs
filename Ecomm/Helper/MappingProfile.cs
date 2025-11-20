using AutoMapper;
using Ecomm.core.Entities;
using Ecomm.DTOs.CategoryDTOs;
using Ecomm.DTOs.ProductDTOs;

namespace Ecomm.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AddCategoryDto,Category>();
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<Product, ProductDto>()
                .ForMember(x => x.Photos, o => o.MapFrom<ResolveImage>());

            CreateMap<AddProductDto, Product>()
                .ForMember(x => x.Photos, o => o.Ignore());

            CreateMap<UpdateProdcutDto, Product>()
               .ForMember(x => x.Photos, o => o.Ignore());

               

        }
    }
}
