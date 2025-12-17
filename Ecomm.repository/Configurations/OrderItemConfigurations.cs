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
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.ProductName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x=>x.Description).IsRequired().HasMaxLength(500);


        }
    }
}
