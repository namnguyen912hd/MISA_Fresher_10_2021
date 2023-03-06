using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Enums
{
    public class OperatorEnum
    {
        /// <summary>
        /// Enum - Toán tử để so sánh điều kiện chuối
        /// </summary>
        /// createdBy: namnguyen(16/1/2022)
        public enum Operator
        {
            /// <summary>
            /// Chứa
            /// </summary>
            Contain = 0,
            /// <summary>
            /// Bằng
            /// </summary>
            EqualTo = 1,
            /// <summary>
            /// Bắt đầu bằng
            /// </summary>
            BeginWith = 2,
            /// <summary>
            /// Kết thúc bằng
            /// </summary>
            EndWith = 3,
            /// <summary>
            /// Không chứa
            /// </summary>
            NotContain = 4,
            /// <summary>
            /// Nhỏ hơn
            /// </summary>
            LessThan = 5,
            /// <summary>
            /// Nhỏ hơn hoặc bằng
            /// </summary>
            LessThanOrEqualTo = 6,
            /// <summary>
            /// Lớn hơn
            /// </summary>
            MoreThan = 7,
            /// <summary>
            /// Lớn hơn hoặc bằng
            /// </summary>
            MoreThanOrEqualTo = 8,
        }

        /// <summary>
        /// Sắp xếp the chiều
        /// </summary>
        /// createdBy: namnguyen(17/01/2022)
        public enum Sort
        {
            /// <summary>
            /// Tăng dần
            /// </summary>
            Asc = 0,
            /// <summary>
            /// Giảm dần
            /// </summary>
            Desc = 1,
        }
    }
}
