using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public class ProductParams
    {
        public string? Sort { get; set; }
        public string? SearchByName { get; set; }
        public int? CategoryId { get; set; }
        public int PageIndex { get; set; } = 1;
        private int MaxPageSize { get; set; } = 10;
        private int pageSize { get; set; } = 5;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}