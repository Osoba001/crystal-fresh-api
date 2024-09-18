using Main.Application.Requests.ItemRequests;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.ItemHandlers
{
    internal class DeleteItemHandler(CrystalFreshDbContext dbContext) : IRequestHandler<DeleteItemRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(DeleteItemRequest request, CancellationToken cancellationToken = default)
        {
            var item= await _dbContext.Items.FindAsync(request.Name.Trim().ToLower());
            if (item == null) return NotFoundResult(); 

            _dbContext.Items.Remove(item);
            return await _dbContext.CompleteAsync(cancellationToken);
        }
    }
}
