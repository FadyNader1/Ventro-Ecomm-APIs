using AutoMapper;
using Ecomm.core.Entities;
using Ecomm.DTOs.ProductDTOs;

namespace Ecomm.Helper
{
    public class ResolveImage : IValueResolver<Product, ProductDto, List<string>>
    {
        private readonly IConfiguration configuration;

        public ResolveImage(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
       
        public List<string> Resolve(Product source, ProductDto destination, List<string> destMember, ResolutionContext context)
        {
            var imanges= new List<string>();
            var hostUrl = configuration["baseUrl"];

            if(source.Photos==null || source.Photos.Count==0)
                return imanges;
            foreach (var photo in source.Photos)
            {
                var fullpath = $"{hostUrl}/{photo.ImageName}";
                imanges.Add(fullpath);
            }
            return imanges;

        }
    }
}
