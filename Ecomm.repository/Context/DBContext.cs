using Ecomm.core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Context
{
    public class DBContext:DbContext
    {

        public DbSet<Product> products=> Set<Product>();
        public DbSet<Category> categories=> Set<Category>();
        public DbSet<Photo> photos=> Set<Photo>();
        public DBContext(DbContextOptions<DBContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
