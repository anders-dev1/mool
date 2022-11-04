using System.Threading.Tasks;
using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route(Routes.Base)]
    public class ThreadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ThreadController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        private static class Routes
        {
            public const string Base = RouteConstants.ApiPrefix + "thread";
        }

        [HttpPost]
        [Authorize]
        public async Task Create(ThreadCreationCommand threadCreationCommand) =>
            await _mediator.Send(threadCreationCommand);

        [HttpGet]
        [Authorize]
        public async Task<ThreadList> List() =>
            await _mediator.Send(new ThreadListQuery());
    }
}