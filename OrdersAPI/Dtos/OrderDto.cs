namespace OrdersAPI.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalCost { get; set; }
}
