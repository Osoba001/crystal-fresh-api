using Main.Application.Requests.CustomerRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.CustomerRequests
{
    internal class SearchCustomerHandler(CrystalFreshDbContext dbContext) : IRequestHandler<SearchCustomerRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(SearchCustomerRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.Customers.Where(x=>x.Id.Contains(request.Id))
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
