using Main.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using Scrypt;

namespace Main.Infrastructure.Database
{
    internal class CrystalFreshDbContext : DbContext
    {
        public CrystalFreshDbContext(DbContextOptions<CrystalFreshDbContext> options) : base(options)
        {
            var dbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (!dbCreator.CanConnect())
                dbCreator.Create();

            if (!dbCreator.HasTables())
            {
                var users = new List<User>
                {
                    new() { Email = "sirkellyc@gmail.com", Role = Role.SuperAdmin.ToString(), PasswordHash = new ScryptEncoder().Encode("string"), Name="Kelly Osoba",PhoneNo="08051651956" }
                };
                dbCreator.CreateTables();


                Users.AddRange(users);
                SaveChangesAsync();
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderedItem> OrderedItems { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntityConfig().Configure(modelBuilder.Entity<User>());
            new PermissionEntityConfig().Configure(modelBuilder.Entity<Permission>());
            new OrderEntityConfig().Configure(modelBuilder.Entity<Order>());
            new OrderedItemEntityConfig().Configure(modelBuilder.Entity<OrderedItem>());
        }

        public async Task<ActionResponse> CompleteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
                return SuccessResult();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresException)
            {
                return postgresException.SqlState switch
                {
                    "23505" => BadRequestResult("The record already exists in the database."),
                    "23503" => BadRequestResult("Operation failed due to a foreign key constraint violation."),
                    _ => ServerExceptionError(postgresException),
                };
            }
            catch (PostgresException ex)
            {
                return ServerExceptionError(ex);
            }
            catch (Exception ex)
            {
                return ServerExceptionError(ex);
            }
        }

    }
}
