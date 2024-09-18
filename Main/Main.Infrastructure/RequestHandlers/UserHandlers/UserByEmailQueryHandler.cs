using Main.Application.Requests.Users;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Auth.Infrastructure.ServiceHandlers
{
    internal class UserByEmailQueryHandler(CrystalFreshDbContext dbContext) :  IRequestHandler<UserByEmailRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UserByEmailRequest request, CancellationToken cancellationToken)
        {
            var resp = await _dbContext.Users.Where(x => x.Email == request.Email.ToLower()).FirstOrDefaultAsync();
            if (resp != null)
            {
                return new ActionResponse
                {
                    PayLoad = new UserResponse
                    {
                        Email = resp.Email,
                        Name = resp.Name,
                        PhoneNo = resp.PhoneNo,
                        Id = resp.Id,
                        Role = resp.Role,

                    }
                };
            }
            return NotFoundResult(UserNotFound);
        }
    }
}
