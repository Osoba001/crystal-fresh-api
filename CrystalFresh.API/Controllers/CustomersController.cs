using Main.Application.Requests.CustomerRequests;
using Main.Infrastructure.Authentications.PermissionAuthorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.MediatKO;
using SwinvaMainV2.API.Controllers;

namespace CrystalFresh.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CustomerResponse>))]
        [HasPermission(Permission.Administrator)]
        public async Task<IActionResult> FetchCustomers(int page, int pageSize) =>
            await SendRequestAsync(new FetchCustomerRequest { Page = page, PageSize = pageSize });

        [HttpGet("{id}")]
        [HasPermission(Permission.Administrator)]
        [ProducesResponseType(200, Type = typeof(List<CustomerResponse>))]
        public async Task<IActionResult> SearchCustomers(string id)=> 
            await SendRequestAsync(new SearchCustomerRequest { Id = id });

    }
}
