using Auth.Application.AuthServices;
using Main.Application.Requests.Users;
using Main.Infrastructure.Database;


namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class ChangePasswordCommandHandler(IAuthService services, CrystalFreshDbContext context) : IRequestHandler<ChangePasswordRequest>
    {
        private readonly IAuthService _authServices = services;
        private readonly CrystalFreshDbContext _dbContext = context;

        public async Task<ActionResponse> HandleAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FindAsync(request.UserIdentifier);

            if (user == null)
                return NotFoundResult(UserNotFound);

           


            if (!_authServices.VerifyPassword(request.OldPassword, user.PasswordHash))
            {
                return Unauthorized(InvalidPassword);
            }

            user!.PasswordHash = _authServices.HashPassword(request.NewPassword);

            _dbContext.Users.Entry(user).Property(x => x.PasswordHash).IsModified = true;
            return await _dbContext.CompleteAsync();
        }
    }
}