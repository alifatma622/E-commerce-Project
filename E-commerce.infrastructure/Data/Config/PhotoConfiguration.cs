using E_commerce.Core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.infrastructure.Data.Config
{
    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {

            builder.HasData(
                new Photo
                {
                    Id = 1,
                    ImgURL = "imgtest",
                    Description = "Electronic devices ",
                    ProductId = 1
                }
            );


        }
    }
}
