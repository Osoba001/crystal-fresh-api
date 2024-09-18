using Main.Application.Requests.CustomerRequests;
using Main.Application.Requests.ItemRequests;
using Main.Infrastructure.Authentications.PermissionAuthorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;
using SwinvaMainV2.API.Controllers;

namespace CrystalFresh.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HttpPost]
        [HasPermission(Permission.Administrator)]
        public async Task<IActionResult> UpserSertItem([FromBody]UpsertItemRequest request)=> await SendRequestAsync(request);

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ItemResponse>))]
        public async Task<IActionResult> FetchItems()=> await SendRequestAsync(new FetchItemRequest());

        [HttpDelete]
        [HasPermission(Permission.Administrator)]
        public async Task<IActionResult> DeleteItem(string name)=> await SendRequestAsync(new DeleteItemRequest { Name= name });
    }
}
