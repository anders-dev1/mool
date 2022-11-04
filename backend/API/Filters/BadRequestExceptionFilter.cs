using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class BadRequestExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not BadRequestException badRequestException)
                return;
            
            context.Result = new BadRequestObjectResult(badRequestException.Message);
        }
    }
}