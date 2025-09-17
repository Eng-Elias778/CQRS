using Microsoft.EntityFrameworkCore;
using OrdersAPI.Models;

namespace OrdersAPI.Data;

public class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
    {
        
    }

    public DbSet<Order> Orders { get; set; } = null!;
}
