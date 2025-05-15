using AutoMapper;
using E_commerce.Core.DTO;
using E_commerce.Core.Entites.Product;

namespace E_commerce.API.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<UpdateCategoryDTO, Category>().ReverseMap();

        }
    }
}
