namespace Main.Application.Requests.ItemRequests
{
    public class DeleteItemRequest : Request
    {
        public required string Name { get; set; }
    }
}
