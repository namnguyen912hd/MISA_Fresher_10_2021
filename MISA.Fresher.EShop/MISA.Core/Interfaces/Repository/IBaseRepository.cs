using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    /// <summary>
    /// interface dùng chung
    /// </summary>
    /// createdBy: namnguyen(14/01/2022)
    public interface IBaseRepository<TEntity>
    {

        #region method

        /// <summary>
        /// lấy tất cả đối tượng
        /// </summary>
        /// <returns>dánh sách đối tượng</returns>
        /// createdBy: namnguyen(14/01/2022)
        IEnumerable<TEntity> GetEntities();

        /// <summary>
        /// lấy thông tin đối tượng theo ID
        /// </summary>
        /// <param name="entityId">ID đối tượng</param>
        /// <returns>thông tin đối tượng</returns>
        /// createdBy: namnguyen(14/01/2022)
        TEntity GetEntityById(Guid entityId);

        /// <summary>
        /// thêm mới đối tượng
        /// </summary>
        /// <param name="entity">đối tượng</param>
        /// <returns>số bản ghi thêm được</returns>
        /// createdBy: namnguyen(14/01/2022)
        int Add(TEntity entity);

        /// <summary>
        /// thêm mới đối tượng với transaction
        /// </summary>
        /// <param name="entity">đối tượng</param>
        /// <returns>số bản ghi thêm được</returns>
        /// createdBy: namnguyen(14/01/2022)
        int Add(TEntity entity, IDbTransaction transaction);

        /// <summary>
        /// sửa thông tin đối tượng
        /// </summary>
        /// <param name="entity">đối tượng</param>
        /// <returns>số bản ghi sửa được</returns>
        /// createdBy: namnguyen(14/01/2022)
        int Update(TEntity entity);

        /// <summary>
        /// sửa thông tin đối tượng với transaction
        /// </summary>
        /// <param name="entity">đối tượng</param>
        /// <returns>số bản ghi sửa được</returns>
        /// createdBy: namnguyen(14/01/2022)
        int Update(TEntity entity, IDbTransaction transaction);


        /// <summary>
        /// xóa đối tượng
        /// </summary>
        /// <param name="entityId">Id đối tượng</param>
        /// <returns>số bản ghi xóa được</returns>
        /// createdBy: namnguyen(14/01/2022)
        int Delete(Guid entityId);

        /// <summary>
        /// xóa đối tượng với transaction
        /// </summary>
        /// <param name="entityId">Id đối tượng</param>
        /// <returns>số bản ghi xóa được</returns>
        /// createdBy: namnguyen(14/01/2022)
        int Delete(Guid entityId, IDbTransaction transaction);


        /// <summary>
        /// lấy đối tượng theo thuộc tính 
        /// </summary>
        /// <param name="entity">đối tượng </param>
        /// <param name="property">thuộc tính</param>
        /// <returns>các đối tượng tương ứng</returns>
        /// createdBy: namnguyen(14/01/2022)
        TEntity GetEntityByProperty(TEntity entity, PropertyInfo property);

        #endregion

    }
}
