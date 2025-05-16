using E_commerce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.infrastructure.Services
{
    public class ImageManagementService: IImageManagementService
    {
        //this all to an add image or delete image from the application
        // IFileProvider is used to get the file information
        private readonly IFileProvider fileProvider;
        public ImageManagementService(IFileProvider fileProvider) {
            this.fileProvider = fileProvider;
        }


        //to add image to the application in wwwroot/images
        public async Task<List<string>> AddPhotoAsync(IFormFileCollection files, string src)
        {
            List<string> SaveImageSrc = new List<string>();
            string ImageDirectory = Path.Combine("wwwroot", "Images", src);
            if (Directory.Exists(ImageDirectory) is not true)
            {
                Directory.CreateDirectory(ImageDirectory);
            }

            foreach (var item in files)
            {
                if (item.Length > 0)
                {
                    // Generate unique filename to prevent overwriting
                    var uniqueFileName = $"{Guid.NewGuid()}_{item.FileName}";
                    var ImageSrc = $"/Images/{src}/{uniqueFileName}";
                    var root = Path.Combine(ImageDirectory, uniqueFileName);

                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(uniqueFileName);
                }
            }
            return SaveImageSrc;
        }

        //to delete image from the application in wwwroot/images
        public async Task DeletePhotoAsync(string src)
        {
            var info = fileProvider.GetFileInfo(subpath: src);

            if (info.Exists && info.PhysicalPath != null)
            {
                File.Delete(path: info.PhysicalPath);
            }

            await Task.CompletedTask;
        }
    }
}
