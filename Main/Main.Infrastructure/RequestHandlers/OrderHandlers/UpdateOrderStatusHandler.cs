using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class UpdateOrderStatusHandler(CrystalFreshDbContext dbContext) : IRequestHandler<UpdateOrderStatusRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateOrderStatusRequest request, CancellationToken cancellationToken = default)
        {
            var order= await _dbContext.Orders.FindAsync(request.Id, cancellationToken);

            if (order == null) return NotFoundResult();

            order.OrderStatus=request.OrderStatus;

            _dbContext.Orders.Entry(order).Property(x=>x.OrderStatus).IsModified=true;
            return await _dbContext.CompleteAsync();
        }
    }
}
