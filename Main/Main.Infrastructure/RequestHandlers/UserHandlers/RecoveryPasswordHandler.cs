
using Main.Application.Requests.Users;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.ServiceHandlers
{
    internal class RecoveryPasswordHandler(CrystalFreshDbContext dbContext) : IRequestHandler<RecoveryPasswordRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(RecoveryPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email.ToLower().Trim());

            if (user is null)
                return NotFoundResult(UserNotFound);

            if (user.RecoveryPinCreationTime.AddMinutes(10) < DateTime.UtcNow)
                return BadRequestResult(SessionExpired);

            if (user.PasswordRecoveryPin != request.RecoveryPin)
            {
               var result=BadRequestResult(IncorrectPin);
                user.RecoveryPinCreationTime = DateTime.UtcNow.AddMinutes(-20);
                _dbContext.Users.Entry(user).Property(x => x.RecoveryPinCreationTime).IsModified = true;
                await _dbContext.CompleteAsync();
                return result;
            }
            user.AllowSetNewPassword = DateTime.UtcNow;
            _dbContext.Users.Entry(user).Property(x => x.AllowSetNewPassword).IsModified = true;
           return await _dbContext.CompleteAsync();
        }
    }
}
