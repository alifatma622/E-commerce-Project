using AutoMapper;
using E_commerce.API.Helper;
using E_commerce.Core.DTO;
using E_commerce.Core.Entites.Product;
using E_commerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using E_commerce.Core.Sharing;

namespace E_commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        //get  
        //first end point   
        // api/Product/getAll
        // //sort="Name"
        [HttpGet("getAll")]
        //415 Unsupported Media Type >> wnen send ProductParms parameters >> use [FromQuery]

        public async Task<IActionResult> GetAll([FromQuery] ProductParms parameters)
        {
            try
            {

                IEnumerable<ProductDTO>? products = await _unitOfWork.ProductRepository.GetALLAsync(parameters);
                var  totalcount = await _unitOfWork.ProductRepository.CounAsync();
                return Ok(new Pagination<ProductDTO>(parameters.PageNumber,parameters.PageSize, totalcount, products));
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Internal server error");
            }
        }


        //api/Product/getbyId/{id}
        [HttpGet("getbyId/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, p => p.Category, p => p.Images);
                if (product == null)
                {
                    return NotFound(new ResponseAPI(404, ($"Product with id {id} not found.")));
                }
                else
                {
                    var productDTO = _mapper.Map<ProductDTO>(product);
                    return Ok(productDTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
            }
        }

        //post
        //api/Product/addProduct
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct(AddProductDTO productDTO)
        {
            try
            { 
                await _unitOfWork.ProductRepository.AddAsync(productDTO);
                return Ok(new ResponseAPI(200,"Product is added sucessfully")) ;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " | Inner Exception: " + ex.InnerException.Message;

                    Console.WriteLine("Inner Exception: " + ex.InnerException.ToString());
                }  
                return BadRequest(new ResponseAPI(400, $"Error adding product: {errorMessage}")); 
            }
        }


        //put
        //api/Product/updateProduct/{id}
        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO updateProduct)
        {
            try
            {
                await _unitOfWork.ProductRepository.UpdateAsync(updateProduct);
                return Ok(new ResponseAPI(200, "Product is updated successfully"));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
            }
        }


        //delete
        //api/Product/deleteProduct/{id}
        [HttpDelete("deleteProduct/{Id}")]
        public async Task<IActionResult> delete(int Id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository
                    .GetByIdAsync(Id,  Product => Product.Images, Product => Product.Category);

                await _unitOfWork.ProductRepository.DeleteAsync(product);

                return Ok(new ResponseAPI (  200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI( 400, ex.Message ));
            }
        }
    }
}