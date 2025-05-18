using E_commerce.Core.DTO;
using E_commerce.Core.Entites.Product;
using E_commerce.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //future use
        Task<IEnumerable<ProductDTO>> GetALLAsync(ProductParms parameters);
        Task<bool> AddAsync(AddProductDTO ProductDTO);
        Task<bool> UpdateAsync(UpdateProductDTO UpdateProductDTO);
        Task<bool> DeleteAsync(Product product);

    }
}
