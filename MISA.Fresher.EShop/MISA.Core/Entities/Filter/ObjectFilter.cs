using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.Core.Enums.OperatorEnum;

namespace MISA.Core.Entities.Filter
{
    /// <summary>
    /// đối tượng filter
    /// </summary>
    public class ObjectFilter
    {
        /// <summary>
        /// Cột cần lọc
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// Toán tử
        /// </summary>
        public Operator Operator { get; set; }
        /// <summary>
        /// Giá trị lọc
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Kiểu dữ liệu lọc (Int, String, Datetime,...)
        /// </summary>
        public string ValueType { get; set; }
        /// <summary>
        /// Kiểu quan hệ điều kiện với các cột khác
        /// </summary>
        public string AdditionalOperator { get; set; } = "AND";
    }
}
