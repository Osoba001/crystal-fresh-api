namespace Main.Application.Requests.Users
{
    public class UserByEmailRequest : Request
    {
        public required string Email { get; set; }
    }
}
