using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    /// <summary>
    /// đối tượng đơn vị tính
    /// </summary>
    public class CalculationUnit:BaseEntity
    {
        /// <summary>
        /// id đơn vị tính
        /// </summary>
        public Guid CalculationUnitId { get; set; }

        /// <summary>
        /// mã đơn vị tính
        /// </summary>
        public string CalculationUnitCode { get; set; }

        /// <summary>
        /// tên đơn vị tính
        /// </summary>
        public string CalculationUnitName { get; set; }

        /// <summary>
        /// mô tả đơn vị tính
        /// </summary>
        public string  Description { get; set; }
    }
}
