using System.Net;
using System.Net.Http;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class UnauthorizedExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not UnauthorizedException unauthorizedException)
                return;

            context.Result = new UnauthorizedObjectResult(unauthorizedException.Message);
        }
    }
}