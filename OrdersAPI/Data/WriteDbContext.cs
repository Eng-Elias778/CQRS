using Microsoft.EntityFrameworkCore;
using OrdersAPI.Models;

namespace OrdersAPI.Data;

public class WriteDbContext : DbContext
{
    public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options)
    {

    }

    public DbSet<Order> Orders { get; set; } = null!;
}
