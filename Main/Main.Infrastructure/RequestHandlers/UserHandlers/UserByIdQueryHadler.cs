
using Main.Application.Requests.Users;
using Main.Infrastructure.Database;


namespace Auth.Infrastructure.ServiceHandlers
{
    internal class UserByIdQueryHadler(CrystalFreshDbContext authServices) : IRequestHandler<UserByIdRequest>
    {
        private readonly CrystalFreshDbContext _authServices = authServices;

        public async Task<ActionResponse> HandleAsync(UserByIdRequest request, CancellationToken cancellationToken)
        {
            var resp = await _authServices.Users.FindAsync(request.UserIdentifier);
            if (resp == null)
                return NotFoundResult(UserNotFound);

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

    }
}
