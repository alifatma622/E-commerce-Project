using E_commerce.Core.Entites.Product;
using E_commerce.infrastructure.Data;
using E_commerce.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.infrastructure.Repositories
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        private readonly AppDbContext _context;
        public PhotoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
