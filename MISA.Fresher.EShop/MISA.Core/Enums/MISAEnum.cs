using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Enums
{
    public class MISAEnum
    {
        /// <summary>
        /// Lấy text của enum qua tên enum đó
        /// </summary>
        /// <param name="misaEnum">Enum</param>
        /// <returns>text của enum</returns>
        /// createdBy: namnguyen(24/12/2021)
        public static string GetEnumTextByEnumName<T>(T misaEnum)
        {
            var enumPropertyName = misaEnum.ToString();
            var enumName = misaEnum.GetType().Name;
            var resourceText = Properties.Resources.ResourceManager.GetString($"Enum_{enumName}_{enumPropertyName}");
            return resourceText;
        }

        /// <summary>
        /// MISACode để xác định trạng thái validate
        /// createdBy: namnguyen(24/12/2021)
        /// </summary>
        public enum MISACode
        {
            /// <summary>
            /// dữ liệu hợp lệ
            /// createdBy: namnguyen(24/12/2021)
            /// </summary>
            IsValid = 100,
            /// <summary>
            /// dữ liệu không hợp lệ
            /// createdBy: namnguyen(24/12/2021)
            /// </summary>
            NotValid = 400,
            /// <summary>
            /// dữ liệu thành công
            /// createdBy: namnguyen(24/12/2021)
            /// </summary>
            Success = 200
        }

        /// <summary>
        /// Xác định trạng thái của object
        /// createdBy: namnguyen(24/12/2021)
        /// </summary>
        public enum EntityState
        {
            /// <summary>
            /// không thao tác
            /// </summary>
            NoAction = 0,
            /// <summary>
            /// chế độ thêm mới
            /// </summary>
            AddNew = 1,
            /// <summary>
            /// chế độ cập nhập
            /// </summary>
            Update = 2,
            /// <summary>
            /// chế độ xóa
            /// </summary>
            Delete = 3
        }
    }
}
