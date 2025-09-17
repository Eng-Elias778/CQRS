namespace OrdersAPI.Services.Events;

public interface IEventHandler<TEvent>
{
    Task HandleAsync(TEvent evt);
}
