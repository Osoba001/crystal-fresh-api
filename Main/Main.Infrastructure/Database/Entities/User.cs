using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Main.Application.Requests.Users;

namespace Main.Infrastructure.Database.Entities
{
    [Table("users")]
    internal class User
    {
        public Guid Id { get; set; }
        public required string Role { get; set; }
        public required string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime AllowSetNewPassword { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public int PasswordRecoveryPin { get; set; }
        public DateTime RecoveryPinCreationTime { get; set; }
        public required string Name { get; set; }
        public required string PhoneNo { get; set; }
        public List<Permission> Permissions { get; set; } = [];
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    }

    internal class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(225);
            builder.Property(x => x.Role).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.PasswordHash).IsRequired();
        }
    }

}
