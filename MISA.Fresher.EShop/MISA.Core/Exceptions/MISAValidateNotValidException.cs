using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Exceptions
{
    /// <summary>
    /// class ngoại lệ khi validate dữ liệu sai
    /// createdBy: namnguyen(23/12/2021)
    /// </summary>
    public class MISAValidateNotValidException : Exception
    {
        public MISAValidateNotValidException( object? value = null) =>
                                    ( Value) = (value);
        public object? Value { get; }
    }
}
