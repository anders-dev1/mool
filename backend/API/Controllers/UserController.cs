using System;
using System.Threading.Tasks;
using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route(Routes.Base)]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public static class Routes
        {
            public const string Base = RouteConstants.ApiPrefix + "user";
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Create(UserCreationCommand userCreationCommand)
        {
            await _mediator.Send(userCreationCommand);
        }
    }
}