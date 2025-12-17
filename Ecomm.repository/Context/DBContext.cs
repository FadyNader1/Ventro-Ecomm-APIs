using Ecomm.core.Entities;
using Ecomm.core.Entities.IdentityModle;
using Ecomm.core.Entities.OrderEntities;
using Ecomm.core.Entities.WishListEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Context
{
    public class DBContext:IdentityDbContext<AppUser>
    {

        public DbSet<Product> products=> Set<Product>();
        public DbSet<Category> categories=> Set<Category>();
        public DbSet<Photo> photos=> Set<Photo>();
        public DbSet<AppUser> AppUsers=> Set<AppUser>();
        public DbSet<RefreshToken> RefreshTokens=> Set<RefreshToken>();
        public DbSet<Order> Orders=> Set<Order>();
        public DbSet<OrderItem> OrderItems=> Set<OrderItem>();
        public DbSet<DeliveryMethod> DeliveryMethods=> Set<DeliveryMethod>();
        public DbSet<WishList> WishLists=> Set<WishList>();
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
