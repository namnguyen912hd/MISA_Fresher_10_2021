using MISA.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.Core.Attributes.ValidatorAttribute;

namespace MISA.Core.Entities
{
    /// <summary>
    /// đối tượng hàng hóa
    /// </summary>
    public class Product: BaseEntity
    {
        /// <summary>
        /// id hàng hóa
        /// </summary>
        [DisplayName("ID hàng hóa")]
        public Guid ProductId { get; set; }

        /// <summary>
        /// mã sku hàng hóa
        /// </summary>
        [Unique]
        [DisplayName("Mã hàng hóa")]
        public string ProductCodeSKU { get; set; }

        /// <summary>
        /// Tên hàng hóa
        /// </summary>
        [Required]
        [DisplayName("Tên hàng hóa")]
        public string ProductName { get; set; }

        /// <summary>
        /// Id nhóm hàng hóa
        /// </summary>
        public Guid? ProductCategoryId { get; set; }

        /// <summary>
        /// Giá mua
        /// </summary>
        public int? PurchasePrice { get; set; }

        /// <summary>
        /// giá bán
        /// </summary>
        public int? SellingPrice { get; set; }

        /// <summary>
        /// id đơn giá
        /// </summary>
        public Guid? CalculationUnitId { get; set; }

        /// <summary>
        /// thuộc tính hàng hóa
        /// </summary>
        public string ProductProperty { get; set; }


        /// <summary>
        /// Id cha
        /// </summary>
        public Guid? ProductParentId { get; set; }

        /// <summary>
        /// mã vạch hàng hóa
        /// </summary>
        [Unique]
        [DisplayName("Mã vạch")]
        public string ProductBarCode { get; set; }

        /// <summary>
        /// mô tả hàng hóa
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// id ảnh hàng hóa
        /// </summary>
        public Guid? ProductImageId { get; set; }

        /// <summary>
        /// tình trạng kinh doanh
        /// </summary>
        public int? BusinessStatus { get; set; }

        /// <summary>
        /// tình trạng hiển thị hàng hóa
        /// </summary>
        public int? ShowStatus { get; set; }

        /// <summary>
        /// tên đơn vị tính
        /// </summary>
        public string CalculationName { get; set; }

        /// <summary>
        /// tên nhóm hàng hóa
        /// </summary>
        public string ProductCategoryName { get; set; }

        /// <summary>
        /// danh sách hàng hóa con
        /// </summary>
        public List<Product> Products { get; set; }
    }
}
