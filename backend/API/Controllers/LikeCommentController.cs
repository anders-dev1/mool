using System.Threading.Tasks;
using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class LikeCommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LikeCommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public static class Routes
        {
            private const string Base = RouteConstants.ApiPrefix + "comment";
            public const string Like = Base + "/{Id}/like";
            public const string Unlike = Base + "/{Id}/unlike";
        }
        
        [HttpPost]
        [Authorize]
        [Route(Routes.Like)]
        public async Task Like([FromRoute]LikeCommentCommand likeCommentCommand) => await _mediator.Send(likeCommentCommand);
        
        [HttpPost]
        [Authorize]
        [Route(Routes.Unlike)]
        public async Task Unlike([FromRoute]UnlikeCommentCommand unlikeThreadCommand) => await _mediator.Send(unlikeThreadCommand);
    }
}