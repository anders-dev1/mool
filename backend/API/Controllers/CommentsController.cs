using System;
using System.Threading.Tasks;
using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public static class Routes
        {
            private const string Base = RouteConstants.ApiPrefix + "thread";
            public const string Comments = Base + "/{threadId}/comments";
        }

        [HttpGet]
        [Authorize]
        [Route(Routes.Comments)]
        public async Task<CommentList> Comments([FromRoute] string threadId, [FromQuery] CommentOrder? order)
        {
            var commentListQuery = new CommentListQuery(threadId, order ?? CommentOrder.OldestFirst);
            return await _mediator.Send(commentListQuery);
        }    
        
        [HttpPost]
        [Authorize]
        [Route(Routes.Comments)]
        public async Task Create([FromRoute] string threadId, [FromBody] string content)
        {
            await _mediator.Send(new CommentCreationCommand(threadId, content));
        }
    }
}