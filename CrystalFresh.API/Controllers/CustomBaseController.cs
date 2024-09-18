using Microsoft.AspNetCore.Mvc;
using Share;
using Share.MediatKO;
using System.Net;
using System.Security.Claims;

namespace SwinvaMainV2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomControllerBase(IMediator mediator, ILogger<CustomControllerBase> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        internal readonly ILogger<CustomControllerBase> _logger = logger;

        public async Task<IActionResult> SendRequestAsync<TRequest>(TRequest request, Guid? id = null) where TRequest : Request
        {
            request.UserIdentifier = id ?? UserId;
            var result = request.Validate();
            if (!result.IsSuccess) return BadRequest(result.Message);
            result = await _mediator.SendAsync(request);
            if (result.Information != null)
            {
                //_logger.LogWarning(result.Information);
            }
            if (!result.IsSuccess)
            {
                if (result.ResponseType == ResponseType.NotFound)
                    return NotFound(result.Message);
                if (result.ServerException != null)
                {
                    _logger.LogError("Something went wrong. Unhandle exception: \n\n{message} ", result.ServerException);
                    return Problem(result.Message, statusCode: (int)HttpStatusCode.InternalServerError);
                }
                if (result.ResponseType == ResponseType.Unauthorized)
                    return Forbid(result.Message);
                return BadRequest(result.Message);
            }
            if (result.PayLoad is not null)
                return Ok(result.PayLoad);
            return NoContent();
        }

        private Guid UserId
        {
            get
            {
                if (User is null) return default;
                var result = User.FindFirst(ClaimTypes.NameIdentifier);
                if (result is not null)
                    return Guid.Parse(result.Value);
                return default;
            }
        }
    }
}