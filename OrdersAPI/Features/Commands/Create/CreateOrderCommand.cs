using MediatR;
using OrdersAPI.Dtos;

namespace OrdersAPI.Features.Commands.Create;

public class CreateOrderCommand : IRequest<OrderDto>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Status { get; set; }
    public decimal TotalCost { get; set; }

}
