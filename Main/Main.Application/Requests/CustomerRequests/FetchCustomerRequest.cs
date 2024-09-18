namespace Main.Application.Requests.CustomerRequests
{
    public class FetchCustomerRequest : Request
    {
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
}
