namespace Main.Application.Requests.Users
{
    public class DeleteUserRequest : Request
    {
        public required string Password { get; set; }
        public required string Reason { get; set; }
        public event DeleteUserEventHandler? DeleteUser;
        public virtual void OnHardDelete(Guid id)
        {
            DeleteUser?.Invoke(id);
        }
    }
    public delegate void DeleteUserEventHandler(Guid userId);
}
