namespace OrdersAPI.Services;

public interface IQueryHandler<TQuery, TResult>
{
    Task<TResult?> HandelAsync(TQuery query);
}
