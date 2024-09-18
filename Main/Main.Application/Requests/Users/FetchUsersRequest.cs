namespace Main.Application.Requests.Users
{
    public class FetchUsersRequest : Request
    {
        public required int PageSize { get; set; }
        public required int Page { get; set; }
    }
    public class UserResponse
    {
        public required Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
    }
}
