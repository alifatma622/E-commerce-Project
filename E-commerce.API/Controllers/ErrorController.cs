using E_commerce.API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.API.Controllers
{
    [Route("Error/{statusCode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        //to handle the controller when an end point  not found it will run this  error 

        [HttpGet]
        public IActionResult HandleError(int statusCode)
        {
            return new ObjectResult(new ResponseAPI(statusCode));
        }
    }
}
