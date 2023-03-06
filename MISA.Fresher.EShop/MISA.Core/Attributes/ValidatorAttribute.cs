using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Attributes
{
    public class ValidatorAttribute
    {
        /// <summary>
        /// attribute cung cấp cho các property bắt buộc nhập - phục vụ cho validate
        /// createdBy: namnguyen(14/01/2022)
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class Required : Attribute
        {

        }

        /// <summary>
        /// attribute cung cấp cho property để tránh trùng dữ liệu
        /// createdBy: namnguyen(14/01/2022)
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class Unique : Attribute
        {

        }

        /// <summary>
        /// attribute cung cấp cho property để hiên thị tên
        /// createdBy: namnguyen(14/01/2022)
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class DisplayName : Attribute
        {
            public string Name { get; set; }
            public DisplayName(string name = null)
            {
                this.Name = name;
            }
        }

    }
}
