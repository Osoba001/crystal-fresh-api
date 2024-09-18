using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class OrderedItem
    {
        public required string ItemId { get; set; }
        public Item Item { get; set; }
        public required int OrderId { get; set; }
        public Order Order { get; set; }
        public required OrderType OrderType { get; set; }

    }
    internal class OrderedItemEntityConfig : IEntityTypeConfiguration<OrderedItem>
    {
        public void Configure(EntityTypeBuilder<OrderedItem> builder)
        {
            builder.HasKey(x => new { x.ItemId, x.OrderId });
            builder.HasOne(o => o.Item).WithMany(o => o.OrderedItems).HasForeignKey(o => o.ItemId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(o => o.Order).WithMany(o => o.OrderedItems).HasForeignKey(o => o.OrderId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
