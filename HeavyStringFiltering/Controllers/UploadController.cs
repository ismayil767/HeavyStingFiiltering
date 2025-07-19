using HeavyStringFiltering.Application.Commands.UploadChunk;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HeavyStringFiltering.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UploadController(IMediator mediator) => _mediator = mediator;

        [HttpPost("upload")]
        public async Task<IActionResult> UploadChunk([FromBody] UploadChunkCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


    }
}
