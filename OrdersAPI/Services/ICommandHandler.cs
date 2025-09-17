namespace OrdersAPI.Services;

public interface ICommandHandler<TCommand, TResult>
{
    Task<TResult?> HandelAsync(TCommand command);
}
