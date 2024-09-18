using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Main.Infrastructure.Database.Entities
{
    internal class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required string Name { get; set; }
        public double WashingPrice { get; set; } = 0;
        public double IroningPrice { get; set; } = 0;
        public double CombinedPrice { get; set; } = 0;
        public List<OrderedItem> OrderedItems { get; set; } = [];
    }
}
