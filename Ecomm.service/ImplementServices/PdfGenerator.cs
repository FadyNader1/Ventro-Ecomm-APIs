using Ecomm.core.Entities.OrderEntities;
using Ecomm.service.HelpersService;
using Ecomm.service.InterfaceServices;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.ImplementServices
{
    public class PdfGenerator : IPdfGenerator
    {
        public PdfGenerator()
        {
            
        }
        public byte[] GenerateInvoicePdf(Order order)
        {
            
            var document = new InvoiceDocument(order); 
            return document.GeneratePdf();
        }
    }
}
