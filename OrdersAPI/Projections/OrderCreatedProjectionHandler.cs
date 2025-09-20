using MediatR;
using OrdersAPI.Data;
using OrdersAPI.Features.Commands.Create;
using OrdersAPI.Models;
using OrdersAPI.Services.Events;

namespace OrdersAPI.Projections;

public class OrderCreatedProjectionHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly ReadDbContext _context;

    public OrderCreatedProjectionHandler(ReadDbContext context)
    {
        _context = context;
    }

    public async Task Handle(OrderCreatedEvent evt, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = evt.OrderId,
            FirstName = evt.FirstName,
            LastName = evt.LastName,
            TotalCost = evt.TotalCost,
            Status = "Created",
            CreatedAt = DateTime.Now
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
