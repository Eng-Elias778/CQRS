using MediatR;
using OrdersAPI.Dtos;

namespace OrdersAPI.Features.Queries.List;

public record GetOrdersListQuery() : IRequest<List<OrdersListDto>>;
