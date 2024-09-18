using Auth.Application.AuthServices;
using Main.Application.Requests.Users;
using Main.Infrastructure.Database;

namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class RefreshTokenHandler(IAuthService authServices, CrystalFreshDbContext dbContext) : IRequestHandler<RefreshTokenRequest>
    {
        private readonly IAuthService _authServices = authServices;
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var id = _authServices.RetrieveTokenNameIdentifier(request.AccessToken);
            if (id == null)
                return BadRequestResult(InvalidToken);
            var user = await _dbContext.Users.FindAsync(id.Value);
            if (user is null)
                return BadRequestResult(InvalidToken);

            if (request.RefreshToken != user.RefreshToken)
                return BadRequestResult(InvalidToken);
            if (DateTime.UtcNow > user.RefreshTokenExpireTime)
                return BadRequestResult(InvalidToken);

            var token = _authServices.TokenManager(user.Id, user.Email, user.Role);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpireTime = token.RefreshTokenExpireTime;
            _dbContext.Users.Entry(user).Property(x => x.RefreshToken).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.RefreshTokenExpireTime).IsModified = true;
            await _dbContext.CompleteAsync();
            return new ActionResponse
            {
                PayLoad = token
            };
        }
    }
}
