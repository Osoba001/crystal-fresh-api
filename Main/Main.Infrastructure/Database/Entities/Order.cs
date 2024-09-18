using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public required string CustomerId { get; set; }
        public required double Discount { get; set; }
        public required double Tip { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.InProgress;
        public List<OrderedItem> OrderedItems { get; set; } = [];
        public required double TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    internal class OrderEntityConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(o => o.Customer).WithMany(o => o.Orders).HasForeignKey(o => o.CustomerId).OnDelete(DeleteBehavior.Cascade);
        }
    }

}
