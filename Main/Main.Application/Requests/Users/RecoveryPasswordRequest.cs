using System.ComponentModel.DataAnnotations;

namespace Main.Application.Requests.Users
{

    public class RecoveryPasswordRequest : Request
    {

        [EmailAddress]
        public required string Email { get; set; }

        public required int RecoveryPin { get; set; }

    }
}
