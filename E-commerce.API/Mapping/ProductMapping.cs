using AutoMapper;
using E_commerce.Core.DTO;
using E_commerce.Core.Entites.Product;

namespace E_commerce.API.Mapping
{
    public class ProductMapping: Profile
    {
        public ProductMapping()
        {
            // mapping the product entity to product dto
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                 .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();

            // mapping the product entity to product dto
            CreateMap<Photo, PhotoDTO>()
                .ForMember(dest=>dest.ImgURL,opt=>opt.MapFrom(src=>src.ImgURL))
                .ReverseMap();
          

            // mapping the product entity to product dto
            //will ignore the images when adding a new product because it string in  product entity and iformfilecollection in addproductdto
            CreateMap<AddProductDTO,Product>()
                .ForMember(m=>m.Images,x=>x.Ignore())
                .ReverseMap();


            //mapping the product entity to product dto
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(m=>m.Images,x=>x.Ignore())
                .ReverseMap();
        }
    }
  
}


