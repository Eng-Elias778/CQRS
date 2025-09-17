using FluentValidation;
using MediatR;
using OrdersAPI.Data;
using OrdersAPI.Dtos;
using OrdersAPI.Models;
using OrdersAPI.Services;
using OrdersAPI.Services.Events;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrdersAPI.Features.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly WriteDbContext _context;
    private readonly IValidator<CreateOrderCommand> _validator;
    private readonly IMediator _mediator;
    public CreateOrderCommandHandler(WriteDbContext context, IValidator<CreateOrderCommand> validator, IEventPublisher eventPublisher, IMediator mediator)
    {
        _context = context;
        _validator = validator;
        _mediator = mediator;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var validateResult = await _validator.ValidateAsync(command);

        if (!validateResult.IsValid)
        {
            throw new ValidationException(validateResult.Errors);
        }

        var order = new Order
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Status = command.Status,
            CreatedAt = DateTime.UtcNow,
            TotalCost = command.TotalCost,
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync(cancellationToken);

        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            FirstName = order.FirstName,
            LastName = order.LastName,
            TotalCost = order.TotalCost
        };

        await _mediator.Publish(orderCreatedEvent);

        return new OrderDto
        {
            Id = order.Id,
            FirstName = order.FirstName,
            LastName = order.LastName,
            Status = order.Status,
            CreatedAt = DateTime.UtcNow,
            TotalCost = order.TotalCost,

        };
    }
}
