using AutoMapper;
using E_commerce.Core.Interfaces;
using E_commerce.Core.Services;
using E_commerce.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService imageManagementService;

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }

        //will register all repositories here  
        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService)
        {
            _context = context;
            _mapper = mapper;
            this.imageManagementService = imageManagementService;

            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context, _mapper, imageManagementService);
            PhotoRepository = new PhotoRepository(_context);
        }
    }
}
