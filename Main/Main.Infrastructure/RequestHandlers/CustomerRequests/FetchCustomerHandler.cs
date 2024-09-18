using Main.Application.Requests.CustomerRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.CustomerRequests
{
    internal class FetchCustomerHandler(CrystalFreshDbContext dbContext) : IRequestHandler<FetchCustomerRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchCustomerRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                return BadRequestResult("Invalid Page or PageSize values.");

            int offset = (request.Page - 1) * request.PageSize;

            return new ActionResponse
            {
                PayLoad = await _dbContext.Customers
            .Skip(offset)
            .Take(request.PageSize)
            .Select(c => new CustomerResponse
            {
                CustomerId = c.Id,
                Name = c.Name,
                TotalOrders = c.Orders.Count
            ,
                TotalAmount = c.Orders.Sum(x => x.TotalAmount)
            }).ToListAsync()
            };
        }
    }
}
