using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.DTO
{
    //for update product
    public class UpdateProductDTO:AddProductDTO
    {
        public int Id { get; set; }
        
    }
} 