using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Attributes
{
    /// <summary>
    /// attribute cung cấp cho các property bắt buộc nhập - phục vụ cho validate
    /// createdBy: namnguyen(24/12/2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DBNotMap : Attribute
    {
        
    }
}
