using E_commerce.Core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.infrastructure.Data.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        // This congig to validate the category entity 
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //name prop in category entity
           builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            // Description prop 
            builder.Property(c => c.Description)
                .HasMaxLength(500);
            builder.Property(c => c.Id)
                .IsRequired();
            builder.HasData(
                new Category
                {
                    Id = 1,
                    Name = "test",
                    Description = "Electronic devices "
                }

            );

        }
    }
}
