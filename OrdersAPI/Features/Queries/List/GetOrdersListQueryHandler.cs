using MediatR;
using Microsoft.EntityFrameworkCore;
using OrdersAPI.Data;
using OrdersAPI.Dtos;
using OrdersAPI.Services;

namespace OrdersAPI.Features.Queries.List;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersListDto>>
{
    private readonly ReadDbContext _context;

    public GetOrdersListQueryHandler(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrdersListDto>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AsNoTracking()
            .Select(x => new OrdersListDto
            {
                OrderId = x.Id,
                CustomerName = $"{x.FirstName} {x.LastName}",
                Status = x.Status,
                TotalCost = x.TotalCost
            }).ToListAsync(cancellationToken);
    }
}
