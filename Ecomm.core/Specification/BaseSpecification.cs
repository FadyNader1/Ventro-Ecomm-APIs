using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public Expression<Func<T, object>> OrderByAsyn { get; set; }
        public Expression<Func<T, object>> OrderByDes { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagingEnabled { get; set; }

        public BaseSpecification()
        {
            Includes = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Includes = new List<Expression<Func<T, object>>>();
            Criteria = criteria;
        }
        public void AddOrderByAsyn(Expression<Func<T, object>> orderBy)
        {
            OrderByAsyn = orderBy;
        }
        public void AddOrderByDes(Expression<Func<T, object>> orderBydes)
        {
            OrderByDes = orderBydes;
        }
        public void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
