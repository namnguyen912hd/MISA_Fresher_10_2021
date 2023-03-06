using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MISA.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Exceptions
{
    public class MISAResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is MISAValidateNotValidException exception)
            {
                var result = new
                            {
                                devMsg = exception.Value,
                                userMsg = exception.Value,
                                data = DBNull.Value,
                                moreInfo = ""
                            };
                context.Result = new ObjectResult(result)
                {
                    StatusCode = ((int)MISAEnum.MISACode.NotValid),
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
