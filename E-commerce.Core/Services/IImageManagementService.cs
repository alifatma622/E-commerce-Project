using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Services
{
    public interface IImageManagementService
    {
        // Service to manage photos by IFormFile to be in the application  
        Task<List<string>> AddPhotoAsync(IFormFileCollection files, string src);
        Task DeletePhotoAsync(string src);
    }
}
