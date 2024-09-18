using Main.Application.Requests.ItemRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.ItemHandlers
{
    internal class FetchItemHandler(CrystalFreshDbContext dbContext) : IRequestHandler<FetchItemRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchItemRequest request, CancellationToken cancellationToken = default)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.Items.Select(x => new ItemResponse
                {
                    CombinedPrice = x.CombinedPrice,
                    IroningPrice = x.IroningPrice,
                    Name = x.Name,
                    WashingPrice = x.WashingPrice,
                }).ToListAsync(),
            };
        }
    }
}
