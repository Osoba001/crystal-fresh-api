namespace Main.Application.Requests.OrderRequests
{
    public class FetchOrderRequest:Request
    {
        public required int  Page { get; set; }
        public required int  PageSize { get; set; }
    }
    public class OrderResponse
    {
        public required int Id { get; set; }
        public required string CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public required double Discount { get; set; }
        public required double Tip { get; set; }
        public required DateTime CreateDate { get; set; }   
        public IEnumerable<OrderItemResponse> Items { get; set; } = [];

    }

    public class OrderItemResponse
    {
        public required string ItemName { get; set; }
        public required string OrderType { get; set; }
    }
}
