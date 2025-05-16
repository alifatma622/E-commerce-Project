using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public virtual List<PhotoDTO> Images { get; set; }
        public string CategoryName { get; set; } 

    }
    public class PhotoDTO
    {
        public string ImgURL { get; set; }
        public string Description { get; set; }
        public string ProductId { get; set; }
    }

    public class AddProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
 
        public int CategoryId { get; set; }
        public IFormFileCollection Photo { get; set; }

    }
}
