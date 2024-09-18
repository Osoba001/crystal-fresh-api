
namespace Main.Application.Requests.Users
{
    public class RefreshTokenRequest : Request
    {
        public required string RefreshToken { get; set; }
        public required string AccessToken { get; set; }
    }
}
