using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Entities;
using MISA.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Api.Controllers
{
    public class ProductImagesController : BaseEntitiesController<ProductImage>
    {
        IProductImageService _baseService;
        ProductImage proImage = new ProductImage();
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductImagesController(IProductImageService baseService, IWebHostEnvironment hostEnvironment) : base(baseService)
        {
            _baseService = baseService;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// api upload ảnh
        /// </summary>
        /// <param name="productImage">dối tượng ảnh hàng hóa</param>
        /// <returns></returns>
        /// createdBy: namnguyen(15/01/2022)
        [HttpPost("UploadImage")]
        public async Task<string> Upload([FromForm] FileUploadApi objFile)
        {
            if (objFile.Files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_hostEnvironment.WebRootPath + "\\Images\\"))
                    {
                        Directory.CreateDirectory(_hostEnvironment.WebRootPath + "\\Images\\");
                    }

                    using (FileStream fileStream = System.IO.File.Create(_hostEnvironment.WebRootPath + "\\Images\\" + objFile.Files.FileName))
                    {
                        
                        objFile.Files.CopyTo(fileStream);
                        fileStream.Flush();
                        // thêm ảnh hàng hóa xuống database
                        proImage.ProductImageId = Guid.NewGuid();
                        proImage.ProductImageUrl = objFile.Files.FileName;
                        _baseService.Add(proImage);

                        return proImage.ProductImageId.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            else
            {
                return "Upload Failed";
            }
        }


        /// <summary>
        /// api lấy đường dẫn ảnh theo id
        /// </summary>
        /// <param name="id">id ảnh</param>
        /// <returns>trả về đường dẫn ảnh</returns>
        /// createdBy: namnguyen(23/01/2022)
        [HttpGet("Image")]
        public IActionResult Image (Guid id)
        {
            // lấy ra đường dẫn file
            string filePath = GetImagePath(id);
            var fileStream = System.IO.File.OpenRead(filePath);
            return new FileStreamResult(fileStream, "image/jpeg");
        }


        /// <summary>
        /// lấy đường dẫn ảnh theo id 
        /// </summary>
        /// <param name="id">id ảnh hàng hóa</param>
        /// <returns>đường dẫn ảnh</returns>
        /// createdBy: namnguyen(15/01/2022)
        private string GetImagePath(Guid id)
        {
            proImage = _baseService.GetEntityById(id);
            return Path.Combine(this._hostEnvironment.WebRootPath, "Images", proImage.ProductImageUrl);
        }

        /// <summary>
        /// đối tượng file upload
        /// </summary>
        public class FileUploadApi
        {
            public IFormFile Files { get; set; }
        }

        
    }
}
