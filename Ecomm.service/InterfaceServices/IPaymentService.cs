using Ecomm.core.Entities.BasketEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.InterfaceServices
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreatePaymentIntentAsync(string BasketId);
    }
}
