using MISA.Core.Entities;
using MISA.Core.Entities.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        /// <summary>
        /// lưu hàng hóa xuống DB
        /// </summary>
        /// <param name="product">đối tượng hàng hóa</param>
        /// <returns>số bản ghi lưu thành công</returns>
        /// createdBy: namnguyen(14/01/2022)
        public int SaveProduct(Product product);

        /// <summary>
        /// lấy mã hàng hóa SKU mới
        /// </summary>
        /// <param name="inputText">tên hàng hóa</param>
        /// <returns>mã hàng hóa dựa theo tên hàng hóa</returns>
        /// createdBy: namnguyen(14/01/2022)
        public string GetNewProductCodeSKU(string inputText);


        /// <summary>
        /// lấy mã vạch mới
        /// </summary>
        /// <returns>mã vạch mới</returns>
        /// createdBy: namnguyen(14/01/2022)
        public string GetNewProductBarCode();


        /// <summary>
        /// xóa nhiều hàng hóa
        /// </summary>
        /// <param name="arrProductId">danh sách Id hàng hóa xóa</param>
        /// <returns>số bản ghi xóa được</returns>
        /// createdBy: namnguyen(23/12/2021)
        int DeleteMultipleProducts(string[] arrProductId);


        /// <summary>
        /// Phân trang tìm kiếm và sắp xếp
        /// </summary>
        /// <param name="pageIndex">Số trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi hiển thị trên 1 trang</param>
        /// <param name="objectFilters">Danh sách điệu kiện tìm kiếm</param>
        /// <returns>Danh sách bản ghi cần tìm kiếm</returns>
        /// CreatedBy: namnguyen(16/01/2022)
        object FilterProducts(int pageIndex, int pageSize, List<ObjectFilter> objectFilters, ObjectSort objectSort);
    }
}
