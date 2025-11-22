using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; set; }
        List<Expression<Func<T, object>>> Includes { get; set; }
        Expression<Func<T, object>> OrderByAsyn { get; set; }
        Expression<Func<T, object>> OrderByDes { get; set; }
        int Skip { get; set; }
        int Take { get; set; }
        bool IsPagingEnabled { get; set; }

    }
}
