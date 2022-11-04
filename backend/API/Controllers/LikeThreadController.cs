using System.Threading.Tasks;
using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class LikeThreadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikeThreadController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public static class Routes
        {
            private const string Base = RouteConstants.ApiPrefix + "thread";
            public const string Like = Base + "/{Id}/like";
            public const string Unlike = Base + "/{Id}/unlike";
        }

        [HttpPost]
        [Authorize]
        [Route(Routes.Like)]
        public async Task Like([FromRoute]LikeThreadCommand likeThreadCommand) => await _mediator.Send(likeThreadCommand);
        
        [HttpPost]
        [Authorize]
        [Route(Routes.Unlike)]
        public async Task Unlike([FromRoute]UnlikeThreadCommand unlikeThreadCommand) => await _mediator.Send(unlikeThreadCommand);
    }
}