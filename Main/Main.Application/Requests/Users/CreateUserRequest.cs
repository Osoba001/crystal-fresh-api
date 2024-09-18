using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Main.Application.Requests.Users
{
    public class CreateUserRequest : Request
    {

        [EmailAddress]
        public required string Email { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
    }
}
