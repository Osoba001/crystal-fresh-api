namespace Main.Application.Requests.OrderRequests
{
    public class CreateOrderRequest:Request
    {

        public required CustomerDto Customer { get; set; }
        public required double Discount { get; set; }
        public required double Tip { get; set; }
        public required IEnumerable<OrderItemDto> OrderItems { get; set; } = [];
    }

    public class CustomerDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
    }
    public class OrderItemDto
    {
        public required string Name { get; set; }
        public  required OrderType OrderType { get; set; }

    }
    
}
