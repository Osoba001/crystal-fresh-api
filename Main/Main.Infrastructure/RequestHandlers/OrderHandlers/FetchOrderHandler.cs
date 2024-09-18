using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class FetchOrderHandler(CrystalFreshDbContext dbContext) : IRequestHandler<FetchOrderRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchOrderRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequestResult("Invalid Page or PageSize values.");

            int offset = (request.Page - 1) * request.PageSize;
            return new ActionResponse
            {
                PayLoad = await _dbContext.Orders
                .OrderByDescending(x=>x.CreatedDate)
                .Skip(offset)
                .Take(request.PageSize)
                .Select(x => new OrderResponse
                {
                    Id= x.Id,
                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer.Name,
                    Discount = x.Discount,
                    Tip = x.Tip,
                    CreateDate=x.CreatedDate,
                    Items = x.OrderedItems.Select(o =>
                    new OrderItemResponse { ItemName = o.ItemId, OrderType = o.OrderType.ToString() })
                }).ToListAsync()
            };
        }
    }
}
