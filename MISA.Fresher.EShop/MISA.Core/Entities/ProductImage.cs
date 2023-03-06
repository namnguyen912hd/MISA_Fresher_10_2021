using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    /// <summary>
    /// đối tượng ảnh hàng hóa
    /// </summary>
    public class ProductImage:BaseEntity
    {
        /// <summary>
        /// id ảnh hàng hóa
        /// </summary>
        public Guid ProductImageId { get; set; }

        /// <summary>
        /// file ảnh
        /// </summary>
        public IFormFile Files { get; set; }

        /// <summary>
        /// đường dẫn ảnh hàng hóa
        /// </summary>
        public string ProductImageUrl { get; set; }
    }
}
