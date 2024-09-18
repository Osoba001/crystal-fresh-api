namespace Main.Application.Requests.Users
{
    public class ChangePasswordRequest : Request
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }

    }
}