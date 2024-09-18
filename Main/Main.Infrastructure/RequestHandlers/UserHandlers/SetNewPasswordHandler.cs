using Auth.Application.AuthServices;
using Main.Application.Requests.Users;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class SetNewPasswordHandler(IAuthService authServices, CrystalFreshDbContext dbContext) : IRequestHandler<SetNewPasswordRequest>
    {
        private readonly IAuthService _authServices = authServices;
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(SetNewPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email.ToLower().Trim());

            if (user is null)
                return NotFoundResult(UserNotFound);
            if (user.PasswordRecoveryPin != request.RecoveryPin)
                return BadRequestResult(IncorrectPin);

            if (user.AllowSetNewPassword < DateTime.UtcNow.AddMinutes(-10))
                return BadRequestResult(SessionExpired);

            user.PasswordHash = _authServices.HashPassword(request.Password);
            user.AllowSetNewPassword = DateTime.UtcNow.AddMinutes(-20);
            _dbContext.Users.Entry(user).Property(x => x.PasswordHash).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.AllowSetNewPassword).IsModified = true;
            return await _dbContext.CompleteAsync();
        }

    }
}
