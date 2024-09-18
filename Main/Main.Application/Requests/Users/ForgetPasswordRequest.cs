using System.ComponentModel.DataAnnotations;

namespace Main.Application.Requests.Users
{

    public class ForgetPasswordRequest : Request
    {

        [EmailAddress]
        public required string Email { get; set; }

    }


}
