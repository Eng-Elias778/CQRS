namespace OrdersAPI.Models;

public class Order
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalCost { get; set; } 
}
