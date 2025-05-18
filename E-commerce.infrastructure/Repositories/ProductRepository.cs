 using AutoMapper;
using E_commerce.Core.DTO;
using E_commerce.Core.Entites.Product;
using E_commerce.Core.Interfaces;
using E_commerce.Core.Services;
using E_commerce.Core.Sharing;
using E_commerce.infrastructure.Data;
using E_commerce.infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            _context = context;
            _mapper = mapper;
            this.imageManagementService = imageManagementService;
        }


        // E-commerce.infrastructure/Repositories/ProductRepository.cs
        public async Task<IEnumerable<ProductDTO>> GetALLAsync(ProductParms parms)
        {
            var query = _context.Products
                .Include(Product => Product.Category)
                .Include(Product => Product.Images)
                .AsNoTracking();

            // Filter by category id if it exists; will return products with this if not will return all
            if (parms.CategoryId.HasValue)
            {
                query = query.Where(Product => Product.CategoryId == parms.CategoryId.Value);
            }

            // Sort default by name, else PriceAsn, PriceDes
            if (!string.IsNullOrEmpty(parms.Sort))
            {
                query = parms.Sort switch
                {
                    "PriceAsn" => query.OrderBy(Product => Product.NewPrice),
                    "PriceDes" => query.OrderByDescending(Product => Product.NewPrice),
                    _ => query.OrderBy(Product => Product.Name),
                };
            }

            //filter by word
            // Filter by search words if search parameter is provided
            if (!string.IsNullOrEmpty(parms.Search))
            {
                var searchWords = parms.Search.Split(' ');

                query = query.Where(product =>
                    searchWords.All(word =>
                        product.Name.ToLower().Contains(word.ToLower()) ||
                        product.Description.ToLower().Contains(word.ToLower())
                    )
                );
            }

            // Default value for pagination
            parms.PageNumber = parms.PageNumber > 0 ? parms.PageNumber : 1;
            parms.PageSize = parms.PageSize > 0 ? parms.PageSize : 3;

            // Apply pagination
            query = query.Skip((parms.PageNumber - 1) * parms.PageSize).Take(parms.PageSize);

            var products = await query.ToListAsync();
            var result = _mapper.Map<List<ProductDTO>>(products);

            return result;
        }




        #region
        //public async Task<IEnumerable<ProductDTO>> GetALLAsync(int? CategoryId, string? sort, int pageNumber, int pageSize)
        //{
        //    var query = _context.Products
        //        .Include(Product => Product.Category)
        //        .Include(Product => Product.Images)
        //        .AsNoTracking();
        //    //Fillter with category id >> if exit will return product with this if not will returnn all 
        //    if (CategoryId.HasValue)
        //    {
        //         query = query.Where(Product => Product.CategoryId == CategoryId.Value);

        //    }
        //    if (!string.IsNullOrEmpty(value: sort))
        //    {
        //        query = sort switch
        //        {
        //            "PriceAsn" => query.OrderBy(Product => Product.NewPrice),
        //            "PriceDes" => query.OrderByDescending(Product => Product.NewPrice),
        //            _ => query.OrderBy(Product => Product.Name),
        //        };
        //    }
        //    //pageNumber = pageNumber > 0 ? pageNumber : 1;
        //    //pageSize = pageSize > 0 ? pageSize : 3;
        //    query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);


        //    var products = await query.ToListAsync();
        //    var result = _mapper.Map<List<ProductDTO>>(products);

        //    return result;
        //}
        #endregion

        public async Task<bool> AddAsync(AddProductDTO ProductDTO)
        {
            if (ProductDTO == null) return false;

            var product = _mapper.Map<Product>(ProductDTO);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var imagePath = await imageManagementService.AddPhotoAsync(ProductDTO.Photo, ProductDTO.Name);

            var photo = imagePath.Select(path => new Photo
            {
                ImgURL = path,
                ProductId = product.Id,
                Description = ProductDTO.Name
            }).ToList();

            await _context.images.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO UpdateProductDTO)
        {
            if (UpdateProductDTO == null)
            {
                return false;
            }

            // Get the product from the database
            var findproduct = await _context.Products
                .Include(c => c.Category)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(p => p.Id == UpdateProductDTO.Id);

            if (findproduct == null)
            {
                return false;
            }

            // Save the old product name before updating
            var oldName = findproduct.Name;

            // Update product data
            _mapper.Map(UpdateProductDTO, findproduct);

            // If the product name has changed, move images and update paths
            if (oldName != UpdateProductDTO.Name)
            {
                var oldDir = Path.Combine("wwwroot", "Images", oldName);
                var newDir = Path.Combine("wwwroot", "Images", UpdateProductDTO.Name);

                if (Directory.Exists(oldDir))
                {
                    if (!Directory.Exists(newDir))
                        Directory.CreateDirectory(newDir);

                    foreach (var file in Directory.GetFiles(oldDir))
                    {
                        var fileName = Path.GetFileName(file);
                        var newFilePath = Path.Combine(newDir, fileName);
                        File.Move(file, newFilePath);

                        // Update the image path in the database
                        var photo = findproduct.Images.FirstOrDefault(img => img.ImgURL.Contains(oldName) && img.ImgURL.Contains(fileName));
                        if (photo != null)
                        {
                            photo.ImgURL = $"/Images/{UpdateProductDTO.Name}/{fileName}";
                        }
                    }

                    // Delete the old folder if it is empty
                    if (!Directory.EnumerateFileSystemEntries(oldDir).Any())
                        Directory.Delete(oldDir);
                }
            }

            // If new images are uploaded, delete old ones and add new ones
            if (UpdateProductDTO.Photo != null && UpdateProductDTO.Photo.Any())
            {
                // Delete old images from storage
                foreach (var photo in findproduct.Images)
                {
                    await imageManagementService.DeletePhotoAsync(photo.ImgURL);
                }

                // Remove old image records from the database
                _context.images.RemoveRange(findproduct.Images);

                // Add new images
                var newImagePaths = await imageManagementService.AddPhotoAsync(UpdateProductDTO.Photo, UpdateProductDTO.Name);
                var newPhotos = newImagePaths.Select(path => new Photo
                {
                    ImgURL = $"/Images/{UpdateProductDTO.Name}/{path}",
                    ProductId = findproduct.Id,
                    Description = UpdateProductDTO.Name
                }).ToList();

                await _context.images.AddRangeAsync(newPhotos);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        async Task<bool> IProductRepository.DeleteAsync(Product product)
        {
            if (product == null)
            {
                return false;
            }

            // 1. Get the folder name 
            var folderPath = Path.Combine("wwwroot", "Images", product.Name);

            // 2. Delete the folder if it exists
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true); 
            }

            var photo = await _context.images
                .Where(p => p.ProductId == product.Id)
                .ToListAsync();

            // Remove the product from machine  
            foreach (var item in photo)
            {
                await imageManagementService.DeletePhotoAsync(item.ImgURL);
            }

            // Remove the product from database  
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
