﻿using E_commerce.Core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.Description).IsRequired();
            builder.Property(c => c.NewPrice).HasColumnType("Decimal(18,2)");
            //seed data( inserting initial data ) when the database is created and is empty
            builder.HasData(
                new Product
                {
                    Id = 1,
                    Name = "test",
                    Description = "test",
                    NewPrice = 100,
                    CategoryId = 1
                }
            );

        }
    }
}
