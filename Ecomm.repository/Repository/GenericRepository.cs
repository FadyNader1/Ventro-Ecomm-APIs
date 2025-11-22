using Ecomm.core.Entities;
using Ecomm.core.Interfaces;
using Ecomm.core.Specification;
using Ecomm.repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DBContext context;

        public GenericRepository(DBContext context)
        {
            this.context = context;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return new BuildQuery().GetQuery(context.Set<T>(), spec);
        }
        public async Task AddAsync(T entity)
        {
             await context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetBySpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }

        public async Task AddRangeAsync(List<T> entities)
        {
           await context.Set<T>().AddRangeAsync(entities);
        }

        public  void DeleteRangeAsync(List<T> entities)
        {
             context.Set<T>().RemoveRange(entities);
        }

        public async Task<int> GetCountSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
    }
}
