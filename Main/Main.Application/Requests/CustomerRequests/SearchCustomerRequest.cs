namespace Main.Application.Requests.CustomerRequests
{
    public class SearchCustomerRequest:Request
    {
        public required string Id { get; set; }
    }
    public class CustomerResponse
    {
        public required string CustomerId { get; set;}
        public required string Name { get; set;}
        public required int TotalOrders { get; set; }
        public required double TotalAmount { get; set; }
    }
}
