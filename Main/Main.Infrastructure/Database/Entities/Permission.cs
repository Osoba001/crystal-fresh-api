using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Main.Infrastructure.Database.Entities
{
    internal class Permission
    {
        public required Guid UserId { get; set; }

        public User User { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
    internal class PermissionEntityConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => new { p.UserId, p.Role });
            builder.HasOne(p => p.User).WithMany(u => u.Permissions).HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
