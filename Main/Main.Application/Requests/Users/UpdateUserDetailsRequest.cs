namespace Main.Application.Requests.Users
{
    public class UpdateUserDetailsRequest : Request
    {
        public required string Name { get; set; }
        public required string Phone { get; set; }

    }
}
