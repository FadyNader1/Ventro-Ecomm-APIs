using Ecomm.core.Entities.OrderEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.repository.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.BuyerEmail).IsRequired().HasMaxLength(100);
            builder.OwnsOne(x => x.shippingAddress, os => os.WithOwner());
            builder.Property(x => x.Status).HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o));
            builder.Property(x => x.OrderDate).HasDefaultValueSql("getutcdate()");
            builder.HasMany(x => x.OrderItems).WithOne(x=>x.order).HasForeignKey(x=>x.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.Subtotal).HasColumnType("decimal(18,2)");
        }
    }
}
