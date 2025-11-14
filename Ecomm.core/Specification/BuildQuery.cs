using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Ecomm.core.Specification;
using Microsoft.EntityFrameworkCore;
using Ecomm.core.Entities;
namespace Ecomm.core.Specification
{
    public  class BuildQuery
    {
        public IQueryable<T> GetQuery<T>(IQueryable<T> basepart, ISpecification<T> specification) where T : class
        {
            var query = basepart;

            if(specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            query=specification.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;

        }
    }
}
