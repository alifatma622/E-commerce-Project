using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Core.Interfaces;

namespace E_commerce.Core.Interfaces
{
    //all repositories will implement this interface
    public interface IUnitOfWork
    {

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        
    }
}
