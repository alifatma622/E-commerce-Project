using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Entites.Product
{
    public class Photo : BaseEntity<int>
    {
        public string ImgURL { get; set; }
        public string Description { get; set; }
  
        public int ProductId { get; set; }
        //[ForeignKey("ProductId")]
        //public virtual Product Product { get; set; }
    }

}
