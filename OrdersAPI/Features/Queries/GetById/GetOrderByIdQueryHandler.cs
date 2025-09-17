using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersAPI.Data;
using OrdersAPI.Dtos;
using OrdersAPI.Models;
using OrdersAPI.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrdersAPI.Features.Queries.GetById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly ReadDbContext _context;

    public GetOrderByIdQueryHandler(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (order == null)
            return null;

        return new OrderDto
        {
            Id = order.Id,
            FirstName = order.FirstName,
            LastName = order.LastName,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            TotalCost = order.TotalCost
        };
    }
}
