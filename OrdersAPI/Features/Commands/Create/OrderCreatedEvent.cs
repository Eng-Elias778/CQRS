using MediatR;

namespace OrdersAPI.Features.Commands.Create;

public class OrderCreatedEvent : INotification
{
    public int OrderId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal TotalCost { get; set; }
}
