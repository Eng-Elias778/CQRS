using MediatR;
using OrdersAPI.Dtos;

namespace OrdersAPI.Features.Queries.GetById;

public record GetOrderByIdQuery(int Id) : IRequest<OrderDto>;
