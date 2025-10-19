using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Project.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Persistence.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(P =>  P.Name).HasColumnType("varchar").HasMaxLength(256);
            builder.Property(P => P.Description).HasColumnType("varchar").HasMaxLength(512);
            builder.Property(P => P.PictureUrl).HasColumnType("varchar").HasMaxLength(256);
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");


            builder.HasOne(P => P.Brand).WithMany().HasForeignKey(b => b.BrandId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(P => P.Type).WithMany().HasForeignKey(b => b.TypeId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
