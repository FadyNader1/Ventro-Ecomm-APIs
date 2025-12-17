namespace Ecomm.DTOs.ProductDTOs
{
    public class HomeProductsDto
    {
        public IReadOnlyList<HomeProductToReturnDto> LatestProducts { get; set; }
        public IReadOnlyList<HomeProductToReturnDto> FeaturedProducts { get; set; }
        public IReadOnlyList<HomeProductToReturnDto> OfferProducts { get; set; }
    }
}
