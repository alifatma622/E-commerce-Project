using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// to determone the properties of the category 
namespace E_commerce.Core.DTO
{
    //for create category OR for get category
    public class CategoryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string ImgURL { get; set; }

    }
}
