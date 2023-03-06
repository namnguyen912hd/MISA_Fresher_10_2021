using MISA.Core.Entities;
using MISA.Core.Exceptions;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MISA.Core.Attributes.ValidatorAttribute;

namespace MISA.Core.Services
{
    /// <summary>
    /// service dùng chung
    /// </summary>
    /// createdBy: namnguyen(15/01/2022)
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {

        #region contructor

        IBaseRepository<TEntity> _baseRepository;
        protected ServiceResult _serviceResult;
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult() { MISACode = Enums.MISAEnum.MISACode.Success };

        }

        #endregion

        #region method

        public IEnumerable<TEntity> GetEntities()
        {
            return _baseRepository.GetEntities();
        }

        public TEntity GetEntityById(Guid entityId)
        {
            return _baseRepository.GetEntityById(entityId);
        }

        public virtual ServiceResult Add(TEntity entity)
        {
            entity.EntityState = Enums.MISAEnum.EntityState.AddNew;
            // validate dữ liệu
            var isValidate = Validate(entity);
            if (isValidate == true)
            {
                _serviceResult.Data = _baseRepository.Add(entity);
                _serviceResult.MISACode = Enums.MISAEnum.MISACode.IsValid;
            }
            return _serviceResult;
        }

        public virtual ServiceResult Update(TEntity entity)
        {
            entity.EntityState = Enums.MISAEnum.EntityState.Update;
            var isValidate = Validate(entity);
            if (isValidate == true)
            {
                _serviceResult.Data = _baseRepository.Update(entity);
                _serviceResult.MISACode = Enums.MISAEnum.MISACode.IsValid;
            }
            return _serviceResult;
        }

        public ServiceResult Delete(Guid entityId)
        {
            _serviceResult.Data = _baseRepository.Delete(entityId);
            return _serviceResult;
        }

        /// <summary>
        /// kiểm tra dữ liệu
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>true: Valid / false: Not Valid</returns>
        /// createdBy: namnguyen(15/01/2022)
        protected bool Validate(TEntity entity)
        {
            // khai báo 1 mảng chứa các câu thông báo lỗi
            var msgArrayError = new List<string>();
            var isValidate = true;
            // lấy các property
            var properties = entity.GetType().GetProperties();
            // đọc các property
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(entity);
                var displayName = string.Empty;
                //lấy tất cả tên của property
                var displayNameAttributes = property.GetCustomAttributes(typeof(DisplayName), true);
                // lấy tên hiển thị đầu tiên của property
                if (displayNameAttributes.Length > 0)
                {
                    displayName = (displayNameAttributes[0] as DisplayName).Name;
                }

                // kiểm tra xem attribute có cần validate không
                // check bắt buộc nhập
                if (property.IsDefined(typeof(Required), false))
                {
                    if (propertyValue == null || string.IsNullOrEmpty(propertyValue.ToString().Trim()))
                    {
                        isValidate = false;
                        msgArrayError.Add(string.Format(Properties.Resources.Msg_ErrorRequire, displayName));
                    }
                }
                // check trùng dữ liệu
                if (property.IsDefined(typeof(Unique), false))
                {
                    var propertyName = property.Name;
                    var entityDuplicate = _baseRepository.GetEntityByProperty(entity, property);
                    if (entityDuplicate != null)
                    {
                        isValidate = false;
                        msgArrayError.Add(string.Format(Properties.Resources.Msg_Duplicate, displayName,propertyValue));
                    }
                }
               
            }
            _serviceResult.Data = msgArrayError;
            if (msgArrayError.Count > 0)
            {
                throw new MISAValidateNotValidException(msgArrayError);
            }
            return isValidate;
        }

        /// <summary>
        /// hàm thực hiện kiểm tra nghiệp vụ/ dữ liệu tùy chỉnh
        /// </summary>
        /// <param name="entity">object</param>
        /// <returns>true:valid / false:not valid</returns>
        /// createdBy: namnguyen(15/01/2022)
        protected virtual bool ValidateCustom(TEntity entity)
        {
            return true;
        }

        #endregion


    }
}
