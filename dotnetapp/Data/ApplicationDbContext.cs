using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Api.Models;

namespace OnlineFoodOrdering.Api.Data;

/// <summary>
/// EF Core database context for the application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the application users.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Gets or sets the menu items.
    /// </summary>
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    /// <summary>
    /// Gets or sets the orders.
    /// </summary>
    public DbSet<Order> Orders => Set<Order>();

    /// <summary>
    /// Gets or sets the order items.
    /// </summary>
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.UserId);
            entity.Property(user => user.Username).IsRequired().HasMaxLength(100);
            entity.Property(user => user.PasswordHash).IsRequired().HasMaxLength(200);
            entity.Property(user => user.Role).IsRequired().HasMaxLength(50);
            entity.HasIndex(user => user.Username).IsUnique();
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(menuItem => menuItem.MenuItemId);
            entity.Property(menuItem => menuItem.Name).IsRequired().HasMaxLength(150);
            entity.Property(menuItem => menuItem.Category).IsRequired().HasMaxLength(100);
            entity.Property(menuItem => menuItem.Price).HasPrecision(18, 2);
            entity.Property(menuItem => menuItem.IsAvailable).IsRequired();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(order => order.OrderId);
            entity.Property(order => order.OrderDate).IsRequired();
            entity.Property(order => order.Status).IsRequired().HasMaxLength(50);
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(order => order.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(orderItem => orderItem.OrderItemId);
            entity.Property(orderItem => orderItem.Quantity).IsRequired();
            entity.HasOne<Order>()
                .WithMany(order => order.Items)
                .HasForeignKey(orderItem => orderItem.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<MenuItem>()
                .WithMany()
                .HasForeignKey(orderItem => orderItem.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, Username = "staff1", PasswordHash = "pass123", Role = "Staff" },
            new User { UserId = 2, Username = "customer1", PasswordHash = "pass123", Role = "Customer" },
            new User { UserId = 3, Username = "delivery1", PasswordHash = "pass123", Role = "DeliveryAgent" });

        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { MenuItemId = 1, Name = "Margherita Pizza", Category = "Pizza", Price = 9.99m, IsAvailable = true },
            new MenuItem { MenuItemId = 2, Name = "Veg Burger", Category = "Burger", Price = 7.49m, IsAvailable = true },
            new MenuItem { MenuItemId = 3, Name = "Coke", Category = "Beverage", Price = 1.99m, IsAvailable = true },
            new MenuItem { MenuItemId = 4, Name = "French Fries", Category = "Sides", Price = 3.49m, IsAvailable = true },
            new MenuItem { MenuItemId = 5, Name = "Chicken Biryani", Category = "Rice", Price = 11.99m, IsAvailable = true });
    }
}
