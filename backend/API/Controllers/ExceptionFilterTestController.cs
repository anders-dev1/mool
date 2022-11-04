using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Features;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public record ExceptionTestRequest(string Error, string Key);
    
    public record ValidationTestRequest(ValidationError[] ValidationErrors, string Message, string Key);
    public record ValidationError(string PropertyName, string Error);
    
    [ApiController]
    public class ExceptionFilterTestController : ControllerBase
    {
        public const string Key = "BC716417-A86E-4525-92A7-FEC93909A2E4";
        
        public static class Routes
        {
            public const string Base = RouteConstants.ApiPrefix + "exceptionfiltertest";
            public const string BadRequest = Base + "/badrequest";
            public const string Conflict = Base + "/conflict";
            public const string NotFound = Base + "/notfound";
            public const string Unauthorized = Base + "/unauthorized";
            public const string Validation = Base + "/validation";
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.BadRequest)]
        public ActionResult BadRequest(ExceptionTestRequest request)
        {
            if (Key != request.Key)
            {
                return new OkResult();
            }
            
            throw new BadRequestException(request.Error);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.Conflict)]
        public ActionResult Conflict(ExceptionTestRequest request)
        {
            if (Key != request.Key)
            {
                return new OkResult();
            }
            
            throw new ConflictException(request.Error);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.NotFound)]
        public ActionResult NotFound(ExceptionTestRequest request)
        {
            if (Key != request.Key)
            {
                return new OkResult();
            }
            
            throw new NotFoundException(request.Error);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.Unauthorized)]
        public ActionResult Unauthorized(ExceptionTestRequest request)
        {
            if (Key != request.Key)
            {
                return new OkResult();
            }
            
            throw new UnauthorizedException(request.Error);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.Validation)]
        public ActionResult Validation(ValidationTestRequest request)
        {
            if (Key != request.Key)
            {
                return new OkResult();
            }

            var errors = request.ValidationErrors.Select(e => new ValidationFailure(e.PropertyName, e.Error));
            throw new ValidationException("test3", errors);
        }
    }
}