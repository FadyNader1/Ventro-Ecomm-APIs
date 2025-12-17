using Ecomm.core.DTOs.Order;

namespace Ecomm.DTOs.OrderDTOs
{
    public class OrderDtoResponse
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
        public IReadOnlyList<OrderItemDtoResponse> OrderItems { get; set; }
        public string DeliveryMethod { get; set; }
        public string Status { get; set; }
    }

    public class OrderItemDtoResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string MainImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
