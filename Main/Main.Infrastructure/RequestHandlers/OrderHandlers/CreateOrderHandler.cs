using Main.Application.Requests.OrderRequests;
using Main.Infrastructure.Database;
using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.RequestHandlers.OrderHandlers
{
    internal class CreateOrderHandler(CrystalFreshDbContext dbContext) : IRequestHandler<CreateOrderRequest>
    {
        private readonly CrystalFreshDbContext _dbContext = dbContext;

        public async Task<ActionResponse> HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
        {
            var itemNames = request.OrderItems.Select(x => x.Name.ToLower().Trim());
            var items = await _dbContext.Items.Where(x => itemNames.Contains(x.Name))
                .ToDictionaryAsync(x => x.Name, x => x);

            if(items.Count< request.OrderItems.Count()) 
            {
                var wrongItems = request.OrderItems.ExceptBy(itemNames, x => x.Name.ToLower());
                var wrongItemName = "";
                foreach(var wrongItem in wrongItems)
                {
                    wrongItemName += wrongItem.Name+", ";
                }
                string msg = $"The following items do not exist:\n {wrongItemName}.";
                return NotFoundResult(msg);
            }
            var customer= await _dbContext.Customers.FindAsync(request.Customer.Id);
            if (customer == null)
            {
                customer= new Customer { Id = request.Customer.Id, Name=request.Customer.Name };
                _dbContext.Customers.Add(customer);
            }
            
            double sum = 0;
            foreach(var itemdto in request.OrderItems)
            {
                if(items.TryGetValue(itemdto.Name, out var item))
                {
                    sum += GetItemPrice(item, itemdto.OrderType);
                }
            }

            var order = new Order { CustomerId = customer.Id, Discount = request.Discount, Tip = request.Tip, TotalAmount = sum };
            _dbContext.Orders.Add(order);

            foreach(var item in request.OrderItems)
            {
                _dbContext.OrderedItems.Add(new OrderedItem { ItemId = item.Name, OrderType = item.OrderType, OrderId = order.Id });
            }
            var resp= await _dbContext.CompleteAsync();
            if (!resp.IsSuccess) return resp;

            resp.PayLoad =new {orderId = order.Id};
            return resp;
        }
        private double GetItemPrice(Item item, OrderType order)
        {
            switch (order)
            {
                case OrderType.Combined: return item.CombinedPrice;

                case OrderType.Washing: return item.WashingPrice;
                default: return item.IroningPrice;
            }
        }
    }
}
