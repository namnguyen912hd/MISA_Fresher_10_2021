using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    /// <summary>
    /// đối tượng nhóm hàng hóa
    /// </summary>
    public class ProductCategory : BaseEntity
    {
        /// <summary>
        /// Id nhóm hàng hóa
        /// </summary>
        public Guid ProductCategoryId { get; set; }

        /// <summary>
        /// mã nhóm hàng hóa
        /// </summary>
        public string ProductCategoryCode { get; set; }

        /// <summary>
        /// tên nhóm hàng hóa
        /// </summary>
        public string  ProductCategoryName { get; set; }

        /// <summary>
        /// mô tả nhóm hàng hóa
        /// </summary>
        public string  Description { get; set; }
    }
}
