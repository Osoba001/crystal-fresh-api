using Main.Application.Requests.Users;
using Main.Infrastructure.Database;

namespace Users.Infrastucture.RequstHandlers.UserRequestHandlers
{
    internal class UpdateUserDetailsHandler(CrystalFreshDbContext dbContext) : IRequestHandler<UpdateUserDetailsRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(UpdateUserDetailsRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FindAsync(request.UserIdentifier, cancellationToken);
            if (user is null)
                return BadRequestResult("User not found");

            user.Name = request.Name;
            user.PhoneNo = request.Phone;

            _dbContext.Users.Entry(user).Property(x => x.Name).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.PhoneNo).IsModified = true;

           return await _dbContext.CompleteAsync();
        }
    }
}
