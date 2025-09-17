namespace OrdersAPI.Dtos;

public class OrdersListDto
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    public string Status  { get; set; }
    public decimal TotalCost { get; set; }
}
