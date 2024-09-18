using Main.Application.Requests.Users;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class FetchUsersHandler(CrystalFreshDbContext dbContext) : IRequestHandler<FetchUsersRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(FetchUsersRequest request, CancellationToken cancellationToken)
        {
            return new ActionResponse
            {
                PayLoad = await _dbContext.Users.Select(u => new UserResponse
                {
                    Email = u.Email,
                    Role = u.Role,
                    Id = u.Id,
                    Name = u.Name,
                    PhoneNo = u.PhoneNo,
                }).AsNoTracking()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync()
            };
        }

    }
}
