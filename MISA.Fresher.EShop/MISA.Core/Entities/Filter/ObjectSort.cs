using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.Core.Enums.OperatorEnum;

namespace MISA.Core.Entities.Filter
{
    public class ObjectSort
    {
        /// <summary>
        /// Tên cột
        /// </summary>
        /// Created(16/01/2022)
        public string Column { get; set; }

        /// <summary>
        /// Điều kiện sắp xếp
        /// </summary>
        /// Created(16/01/2022)
        public Sort SortOrder { get; set; } = 0;
    }
}
