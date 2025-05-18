using AutoMapper;
using E_commerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        //api/Bug/Not-Found
        [HttpGet("Not-Found")]
        public async Task<IActionResult> getNotFound()
        {
            var catregory = await _unitOfWork.CategoryRepository.GetByIdAsync(50000);
            if (catregory == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(catregory);
            }
        }

        //api/Bug/Server-Error
        [HttpGet("Server-Error")]
  
        public IActionResult GetServerError()
        {
            var categoryTask = _unitOfWork.CategoryRepository.GetByIdAsync(20);
            var category = categoryTask.Result; 

            category.Name = "";
            return Ok(category);
        }


        //api/bug/Bad-Request/20
        [HttpGet("Bad-Request/{id}")]
        public async Task<IActionResult> getBadRequest(int id)
        {
             return  BadRequest();
        }
        //api/bug/Bad-Request
        [HttpGet("Bad-Request")]
        public async Task<IActionResult> getBadRequest()
        {
            return BadRequest();
        }





    }
}
