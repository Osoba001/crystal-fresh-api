namespace Main.Application.Requests.OrderRequests
{
    public class FetchOrderByCustomerRequest : Request
    {
        public required string Id { get; set; }
    }
}
