using Main.Application.Requests.OrderRequests;
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
    public class OrdersController(IMediator mediator, ILogger<CustomControllerBase> logger) : CustomControllerBase(mediator, logger)
    {
        [HttpPost]
        [HasPermission(Permission.Administrator)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)=> await SendRequestAsync(request);

        [HttpGet]
        [HasPermission(Permission.Administrator)]
        [ProducesResponseType(200, Type = typeof(List<OrderResponse>))]
        public async Task<IActionResult> FetchOrders(int page, int pageSize)=>
            await SendRequestAsync(new FetchOrderRequest { Page = page, PageSize = pageSize });

        [HttpGet("{customerId}")]
        [HasPermission(Permission.Administrator)]
        [ProducesResponseType(200, Type = typeof(List<OrderResponse>))]
        public async Task<IActionResult> FetchCusterOrders(string customerId)=> 
            await SendRequestAsync(new FetchOrderByCustomerRequest { Id = customerId });

        [HttpPut]
        [HasPermission(Permission.Administrator)]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request) => await SendRequestAsync(request);
    }
}
