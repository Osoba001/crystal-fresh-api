namespace Main.Application.Requests.ItemRequests
{
    public class FetchItemRequest : Request
    {

    }
    public class ItemResponse
    {
        public required string Name { get; set; }
        public required double WashingPrice { get; set; }
        public required double IroningPrice { get; set; }
        public required double CombinedPrice { get; set; }
    }
}
