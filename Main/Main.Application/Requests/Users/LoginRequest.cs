using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Main.Application.Requests.Users
{
    public class LoginRequest : Request
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }

    }
    public class TokenModel
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required string Role { get; set; }
        [JsonIgnore]
        public DateTime RefreshTokenExpireTime { get; set; }
        public Guid Id { get; set; }
    }
}
