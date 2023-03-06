using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Api.Controllers
{
    /// <summary>
    /// api BaseEntities - gồm các api dùng chung của các đối tượng
    /// createdBy: namnguyen(15/01/2022)
    /// </summary>
    ///
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseEntitiesController<TEntity> : ControllerBase
    {
        IBaseService<TEntity> _baseService;

        public BaseEntitiesController(IBaseService<TEntity> baseService)
        {
            _baseService = baseService;
        }

        /// <summary>
        /// api lấy tất cả đối tượng
        /// </summary>
        /// <returns>danh sách đối tượng</returns>
        /// createdBy: namnguyen(15/01/2022)
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _baseService.GetEntities();
            return Ok(entities);
        }

        /// <summary>
        /// api lấy đối tượng theo Id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <returns>1 đối tượng tương ứng với Id</returns>
        /// createdBy: namnguyen(15/01/2022)
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                var entity = _baseService.GetEntityById(Guid.Parse(id));
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// api thêm 1 một đối tượng
        /// </summary>
        /// <param name="entity">thông tin đối tượng thêm</param>
        /// createdBy: namnguyen(15/01/2022)
        [HttpPost]
        public IActionResult Post(TEntity entity)
        {

            var serviceResult = _baseService.Add(entity);
            return Ok(serviceResult);

        }

        /// <summary>
        /// sửa đối tượng được lấy về từ DB theo Id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <param name="entity">thông tin đối tượng sửa</param>
        /// createdBy: namnguyen(15/01/2022)
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] TEntity entity)
        {
            // lấy id đối tượng
            var keyProperty = entity.GetType().GetProperty($"{typeof(TEntity).Name}Id");
            // chuyển kiểu dữ liệu của Id đối tượng để tương ứng DB
            if (keyProperty.PropertyType == typeof(Guid)) // nếu là kiểu Guid
            {
                keyProperty.SetValue(entity, Guid.Parse(id));
            }
            else if (keyProperty.PropertyType == typeof(int)) // nếu là kiểu Int
            {
                keyProperty.SetValue(entity, int.Parse(id));
            }
            else // khác
            {
                keyProperty.SetValue(entity, id);
            }
            // thực hiện update
            var serviceResult = _baseService.Update(entity);
            if (serviceResult.MISACode == Core.Enums.MISAEnum.MISACode.NotValid)
            {
                return BadRequest(serviceResult);
            }
            else
            {
                return Ok(serviceResult);
            }

        }

        /// <summary>
        /// xóa một đối tượng theo Id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// createdBy: namnguyen(15/01/2022)
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var res = _baseService.Delete(id);
            return Ok(res);
        }
    }
}
