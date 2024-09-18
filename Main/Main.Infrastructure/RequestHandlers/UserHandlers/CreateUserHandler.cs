using Auth.Application.AuthServices;
using Main.Application.Requests.Users;
using Main.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class CreateUserHandler(IAuthService services, CrystalFreshDbContext context) : IRequestHandler<CreateUserRequest>
    {
        private readonly IAuthService _authServices = services;
        private readonly CrystalFreshDbContext _dbContext = context;

        public async Task<ActionResponse> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            string email = request.Email.ToLower().Trim();
            var user = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (user is not null)
                return BadRequestResult(EmailAlreadyExist);

            user = new()
            {
                Email = email,
                Role = request.Role.ToString(),
                PasswordHash = _authServices.HashPassword(request.Password),
                Name = request.Name,
                PhoneNo = request.PhoneNo,

            };
            _dbContext.Users.Add(user);
            var res = await _dbContext.CompleteAsync();
            if (!res.IsSuccess)
                return res;

            var tokenModel = _authServices.TokenManager(user.Id, user.Role, user.Email);
            user.RefreshToken = tokenModel.RefreshToken;
            user.RefreshTokenExpireTime = tokenModel.RefreshTokenExpireTime;
            _dbContext.Users.Entry(user).Property(x => x.RefreshToken).IsModified = true;
            _dbContext.Users.Entry(user).Property(x => x.RefreshTokenExpireTime).IsModified = true;
            await _dbContext.CompleteAsync();
            return new ActionResponse
            {
                PayLoad = tokenModel
            };

        }


    }
}
