using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Main.Infrastructure.Database.Entities
{
    internal class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<Order> Orders { get; set; } = [];
    }
    


    
}
