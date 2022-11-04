using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var validationException = context.Exception as ValidationException;

            if (validationException == null)
                return;
            
            context.Result = new BadRequestObjectResult(validationException.Errors);
        }
    }
}