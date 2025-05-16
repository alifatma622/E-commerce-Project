using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Core.Entites.Product;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_commerce.Core.Entites.Product
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public decimal Price { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        // add virtual because of lazy loading >> category not load without req and overloading 
        //linking to category 
        public virtual Category Category { get; set; }
        //linking to Image 
        public virtual ICollection<Photo> Images { get; set; } = new List<Photo>();
    }
}
