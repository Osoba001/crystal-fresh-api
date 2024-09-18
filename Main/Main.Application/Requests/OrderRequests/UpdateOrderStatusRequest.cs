namespace Main.Application.Requests.OrderRequests
{
    public class UpdateOrderStatusRequest:Request
    {
        public required int Id { get; set; }
        public required OrderStatus OrderStatus { get; set; }
    }
}
