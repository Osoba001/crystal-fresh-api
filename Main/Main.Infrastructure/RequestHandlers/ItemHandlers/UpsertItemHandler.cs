using Main.Application.Requests.ItemRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;

namespace Main.Infrastructure.RequestHandlers.ItemHandlers
{
    internal class UpsertItemHandler(CrystalFreshDbContext dbContext) : IRequestHandler<UpsertItemRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpsertItemRequest request, CancellationToken cancellationToken = default)
        {
            string message = "Created";
            var item = await _dbContext.Items.FindAsync(request.Name.Trim().ToLower());
            if (item == null)
            {
                item= new Item { Name= request.Name.Trim().ToLower(), CombinedPrice=request.CombinedPrice, IroningPrice=request.IroningPrice };
                _dbContext.Items.Add(item);
            }
            else
            {
                item.CombinedPrice = request.CombinedPrice;
                item.WashingPrice = request.WashingPrice;
                item.IroningPrice = request.IroningPrice;

                _dbContext.Items.Update(item);
                message = "Updated";
            }
            var resp = await _dbContext.CompleteAsync();
            if (!resp.IsSuccess) return resp;

            resp.PayLoad = new { ItemName = item.Name, message};
            return resp;
        }
    }
}
