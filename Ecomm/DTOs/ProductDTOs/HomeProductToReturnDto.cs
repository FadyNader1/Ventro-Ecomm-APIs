namespace Ecomm.DTOs.ProductDTOs
{
    public class HomeProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }

        public string PictureUrl { get; set; }
        public int Rating { get; set; }

        public string CategoryName { get; set; }
        public int inventoryQuantity { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsNewArrival { get; set; }
    }
}
