using E_commerce.Core.Entites.Product;
using E_commerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using E_commerce.Core.DTO;
using AutoMapper;
using E_commerce.infrastructure.Repositories;
using E_commerce.API.Helper;

namespace E_commerce.API.Controllers
{
    // api/category  >>is the route
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        //get
        //first end point 
        // api/category/getAll
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
                if (categories == null)
                {
                    return BadRequest(new ResponseAPI(400, "Category not found "));
                }
                else
                {
                    var categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
                    return Ok(categoryDTOs);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //api/category/GetById
        [HttpGet("getbyId/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return BadRequest(new ResponseAPI(400, ($"Category with id {id} not found ")));

                    
                }
                else
                {
                    var categoryDTO = _mapper.Map<CategoryDTO>(category);
                    return Ok(categoryDTO);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //post
        //api/category/addCategory
        [HttpPost("addCategory")]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null)
                {

                    return BadRequest(new ResponseAPI(400, ("Category That you want to added is null")));
                }
                else
                {
                    var category = _mapper.Map<Category>(categoryDTO);
                    await _unitOfWork.CategoryRepository.AddAsync(category);
                    await _unitOfWork.CategoryRepository.SaveChangesAsync();
                    //return Ok(_mapper.Map<CategoryDTO>(category));
                    return Ok(new ResponseAPI(200, "Category is added successfully"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //update
        //api/category/updateCategory
        [HttpPut("updateCategory")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDTO updateCategoryDTO)
        {
           try
            {  
               
                if (updateCategoryDTO == null)
                {

                    return BadRequest(new ResponseAPI(400, $"Category with ID {updateCategoryDTO?.Id} not found (Null)"));

                }

                var existingCategory = _mapper.Map<Category>(updateCategoryDTO);

                await _unitOfWork.CategoryRepository.UpdateAsync(existingCategory);
                await _unitOfWork.CategoryRepository.SaveChangesAsync();

                //return Ok(_mapper.Map<CategoryDTO>(existingCategory));
                return Ok(new ResponseAPI(200, "Category is updated successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        


        //update
        //api/category/updateCategory/{id}
        [HttpPut("updateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDTO updateCategoryDTO)
        {
            try
            {

                if (id != updateCategoryDTO.Id)
                {
                    return BadRequest(new ResponseAPI(400,("The ID in the URL does not match the ID in the request body.")));

                }

                var existingCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (existingCategory == null)
                {
                 
                    return BadRequest(new ResponseAPI(400, $"Category with ID {id} not found (Null)"));

                }
                //map with auto mapper 
                _mapper.Map(updateCategoryDTO, existingCategory);
                //update
                await _unitOfWork.CategoryRepository.UpdateAsync(existingCategory);
                await _unitOfWork.CategoryRepository.SaveChangesAsync();
                //return Ok(_mapper.Map<CategoryDTO>(existingCategory));
                return Ok(new ResponseAPI(200, $"Category with Id {id} is update successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

     

        //delete
        //api/category/deleteCategory
        [HttpDelete("deleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return BadRequest(new ResponseAPI(400, $"Category with ID {id} not found (Null)"));

                }
                else
                {
                    await _unitOfWork.CategoryRepository.DeleteAsync(id);
                    await _unitOfWork.CategoryRepository.SaveChangesAsync();
                    //return Ok("Category deleted successfully"); //using Response Api class that handle staus code istead
                    return Ok(new ResponseAPI(200, "Category is deleted successfully"));

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}