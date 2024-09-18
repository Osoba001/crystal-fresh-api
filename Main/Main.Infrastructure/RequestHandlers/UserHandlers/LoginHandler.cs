using Auth.Application.AuthServices;
using Main.Application.Requests.Users;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.ServiceHandlers
{
    internal class LoginHandler(IAuthService authServices, CrystalFreshDbContext dbContext) : IRequestHandler<LoginRequest>
    {
        private readonly IAuthService _authServices = authServices;
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await GetUserByEmailAsync(request.Email, cancellationToken);

            if (user is null)
                return Unauthorized("Invalid email or password.");

           

            if (!_authServices.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            return await GenerateTokenAndHandleRefreshToken(user, cancellationToken);
        }

        private async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower().Trim(), cancellationToken);
        }


        private async Task<ActionResponse> GenerateTokenAndHandleRefreshToken(User user, CancellationToken cancellationToken)
        {
            var tokenModel = _authServices.TokenManager(user.Id, user.Email, user.Role);
            user.RefreshToken = tokenModel.RefreshToken;
            user.RefreshTokenExpireTime = tokenModel.RefreshTokenExpireTime;
            _dbContext.Users.Entry(user).Property(x => x.RefreshToken).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.RefreshTokenExpireTime).IsModified = true;

            var res = await _dbContext.CompleteAsync(cancellationToken);
            if (!res.IsSuccess) return res;

            return new ActionResponse
            {
                PayLoad = tokenModel
            };
        }
    }
}
