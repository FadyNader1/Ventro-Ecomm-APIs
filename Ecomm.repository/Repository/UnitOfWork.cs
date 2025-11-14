using Ecomm.core.Interfaces;
using Ecomm.repository.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext context;
        private Hashtable repositories;

        public UnitOfWork(DBContext context)
        {
            this.context = context;
            this.repositories= new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;
            if(!repositories.ContainsKey(type))
            {
                var repo= new GenericRepository<T>(context);
                repositories.Add(type, repo);
            }
            return (IGenericRepository<T>) repositories[type];

        }   
        
    }
}
