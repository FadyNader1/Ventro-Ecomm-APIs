using Ecomm.core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllSpecAsync(ISpecification<T> spec);
        Task<T> GetByIdAsync(int id);
        Task<T> GetBySpecAsync(ISpecification<T> spec);
        Task AddAsync(T entity);
        void Updaet(T entity);
        void Delete(T entity);


    }
}
