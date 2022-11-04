using System;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class AccessTokenController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public AccessTokenController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public static class Routes
        {
            public const string Base = RouteConstants.ApiPrefix + "accesstoken";
            public const string Authenticate = Base + "/authenticate";
            public const string Renew = Base + "/renew";
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.Authenticate)]
        public async Task<AuthenticationResult> Authenticate(AuthenticationQuery request)
        {
            return await _mediator.Send(request);
        }
            
        
        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.Renew)]
        public async Task<RenewAccessTokenResult> Renew(RenewAccessTokenCommand request) => 
            await _mediator.Send(request);
    }
}