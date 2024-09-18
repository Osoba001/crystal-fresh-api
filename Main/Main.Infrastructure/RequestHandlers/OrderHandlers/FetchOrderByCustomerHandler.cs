using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class FetchOrderByCustomerHandler(CrystalFreshDbContext dbContext) : IRequestHandler<FetchOrderByCustomerRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchOrderByCustomerRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.Orders.Where(x => x.CustomerId == request.Id)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new OrderResponse
                {
                    Id=x.Id,
                    CustomerId = x.CustomerId,
                    CustomerName = x.Customer.Name,
                    Discount = x.Discount,
                    CreateDate = x.CreatedDate,
                    Tip = x.Tip,
                    Items = x.OrderedItems.Select(o =>
                    new OrderItemResponse { ItemName = o.ItemId, OrderType = o.OrderType.ToString() })
                }).ToListAsync()
            };
        }
    }
}
