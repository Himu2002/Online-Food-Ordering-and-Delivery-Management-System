using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Domain.Entities;
using OnlineFoodOrdering.Infrastructure.Persistence;

namespace OnlineFoodOrdering.Infrastructure.Services;

/// <summary>
/// Creates the database and seeds interview-ready sample data.
/// </summary>
public class DatabaseSeeder
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseSeeder"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="passwordHasher">The password hasher.</param>
    public DatabaseSeeder(AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Ensures the database exists and seeds starter data if it is empty.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.EnsureCreatedAsync(cancellationToken);

        if (await _dbContext.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var staff = new User
        {
            UserId = 1,
            Username = "staff1",
            Role = "Staff"
        };
        staff.PasswordHash = _passwordHasher.HashPassword(staff, "pass123");

        var customer = new User
        {
            UserId = 5,
            Username = "customer5",
            Role = "Customer"
        };
        customer.PasswordHash = _passwordHasher.HashPassword(customer, "pass123");

        var deliveryAgent = new User
        {
            UserId = 2,
            Username = "delivery1",
            Role = "DeliveryAgent"
        };
        deliveryAgent.PasswordHash = _passwordHasher.HashPassword(deliveryAgent, "pass123");

        var pizza = new MenuItem
        {
            MenuItemId = 1,
            Name = "Pizza",
            Category = "Main Course",
            Price = 12.99m,
            IsAvailable = true
        };

        var coke = new MenuItem
        {
            MenuItemId = 2,
            Name = "Coke",
            Category = "Beverage",
            Price = 1.99m,
            IsAvailable = true
        };

        var seededOrder = new Order
        {
            CustomerId = 5,
            OrderDate = DateTime.UtcNow.AddDays(-1),
            Status = "Delivered",
            Items = new List<OrderItem>
            {
                new() { MenuItemId = pizza.MenuItemId, Quantity = 1 },
                new() { MenuItemId = coke.MenuItemId, Quantity = 2 }
            }
        };

        _dbContext.Users.AddRange(staff, customer, deliveryAgent);
        _dbContext.MenuItems.AddRange(pizza, coke);
        _dbContext.Orders.Add(seededOrder);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}