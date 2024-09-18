using Main.Application.Constants;
using Main.Application.Requests.Users;

namespace Auth.Application.AuthServices
{
    public interface IAuthService
    {
        string HashPassword(string password);
        Guid? RetrieveTokenNameIdentifier(string jwt);
        TokenModel TokenManager(Guid id,string email, string role);
        bool VerifyExpiredJwtToken(string jwtToken);
        bool VerifyPassword(string password, string hashedPassword);
    }
}