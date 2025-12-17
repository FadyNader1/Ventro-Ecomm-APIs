using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.DTOs.DeliveryMethodDTOs
{
    public class DeliveryMethodDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DeleveryDays { get; set; }
        public string Description { get; set; }
    }
    public class UpdateDeliveryMethodDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DeleveryDays { get; set; }
        public string Description { get; set; }
    }
    public class AddDeliveryMethodDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DeleveryDays { get; set; }
        public string Description { get; set; }
    }
 
}
