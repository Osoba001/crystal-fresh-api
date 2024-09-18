using Auth.Application.AuthServices;
using Main.Application.Requests.Users;
using Main.Infrastructure.Database;


namespace Main.Infrastructure.RequestHandlers.UserHandlers
{
    internal class DeleteUserHandler(CrystalFreshDbContext context, IAuthService authServices) : IRequestHandler<DeleteUserRequest>
    {
        private readonly CrystalFreshDbContext _context = context;
        private readonly IAuthService _authServices = authServices;

        public async Task<ActionResponse> HandleAsync(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserIdentifier);
            if (user is null)
                return NotFoundResult(UserNotFound);

            if (!_authServices.VerifyPassword(request.Password, user.PasswordHash))
                return BadRequestResult("Incorrect password.");

            _context.Users.Remove(user);
            var result = await _context.CompleteAsync();
            if (result.IsSuccess)
            {
                result.Information = new { Action = $"Deleted {user.Name} User", Message = request.Reason, email=user.Email, user.Id };
                request.OnHardDelete(user.Id);
            }

            return result;
        }

    }
}
