using Ecomm.core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.InterfaceServices
{
    public interface IPdfGenerator
    {
        byte[] GenerateInvoicePdf(Order order);
    }
}
