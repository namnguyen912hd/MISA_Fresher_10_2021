using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MISA.Core.Enums.MISAEnum;

namespace MISA.Core.Entities
{
    public class BaseEntity
    {
        /// <summary>
        /// ngày tạo
        /// createdBy: namnguyen(14/01/2022)
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// người tạo bản ghi
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// ngày sửa mới nhất
        /// createdBy: namnguyen(14/01/2022)
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// người sửa
        /// createdBy: namnguyen(14/01/2022)
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// chế độ của object: Add/Edit/Delete
        /// createdBy: namnguyen(14/01/2022)
        /// </summary>
        public EntityState EntityState { get; set; } = EntityState.AddNew;
    }
}
