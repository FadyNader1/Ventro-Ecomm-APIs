using Ecomm.core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Interfaces
{
    public interface IDeliveryMethodRepo
    {
        Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync();
        Task<DeliveryMethod> GetDeliveryMethodByIdAsync(int id);
        Task AddDeliveryMethodAsync(DeliveryMethod deliveryMethod);
        Task DeleteDeliveryMethodAsync(DeliveryMethod deliveryMethod);
        Task UpdateDeliveryMethodAsync(DeliveryMethod deliveryMethod);
    }
}
