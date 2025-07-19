using Microsoft.EntityFrameworkCore;
using Store.G04.Core.Entities;
using Store.G04.Core.Entities.Order;
using System.Reflection;

namespace Store.G04.Repositpory.Data.Contexts;
public class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<BookingMachine> BookingMachine { get; set; }
    public DbSet<MachineEntity> Machine { get; set; }
    public DbSet<RawMaterial> RawMaterial { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    public DbSet<BookingMaterial> BookingMaterials { get; set; }
}
