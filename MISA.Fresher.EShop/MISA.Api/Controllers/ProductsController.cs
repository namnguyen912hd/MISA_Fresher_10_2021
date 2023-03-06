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
    public class ProductsController : BaseEntitiesController<Product>
    {

        #region declare and contructor
        IProductService _baseService;
        public ProductsController(IProductService baseService) : base(baseService)
        {
            _baseService = baseService;
        }

        #endregion

        #region api
        /// <summary>
        /// api lấy mã hàng hóa mới
        /// </summary>
        /// <returns>text </returns>
        /// /// createdBy: namnguyen(22/01/2022)
        [HttpGet("NewProductCodeSKU")]
        public IActionResult GetNewProductCodeSKU(string inputText)
        {
            try
            {
                return Ok(_baseService.GetNewProductCodeSKU(inputText));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        /// <summary>
        /// api lấy mã hàng hóa mới
        /// </summary>
        /// <returns>text </returns>
        /// /// createdBy: namnguyen(22/01/2022)
        [HttpGet("NewProductBarCode")]
        public IActionResult GetNewProductBarCode()
        {
            try
            {
                return Ok(_baseService.GetNewProductBarCode());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// api xóa nhiều nhân viên
        /// </summary>
        /// <param name="arrProductId">mảng id nhân viên</param>
        /// <returns></returns>
        /// createdBy: namnguyen(22/01/2022)
        [HttpPost("DeleteProducts")]
        public IActionResult DeleteMutilple([FromBody] string[] arrProductId)
        {
            var res = _baseService.DeleteMultipleProducts(arrProductId);
            if (res.MISACode == Core.Enums.MISAEnum.MISACode.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }


        /// <summary>
        /// api filter dữ liệu hàng hóa
        /// </summary>
        /// <param name="pageIndex">số trang</param>
        /// <param name="pageSize">số record trên 1 trang</param>
        /// <param name="objectFilters">đối tượng filter</param>
        /// <param name="objectSort">đối tượng sắp xếp</param>
        /// <returns>dữ liệu hàng hóa</returns>
        /// createdBy: namnguyen(22/01/2022)
        [HttpGet("FilterProducts")]
        public IActionResult FilterProducts([FromQuery] int pageIndex, int pageSize, string objectFilters, string objectSort)
        {
            var res = _baseService.FilterProducts(pageIndex, pageSize, objectFilters, objectSort);
            return Ok(res);
        }

        #endregion

    }
}
