namespace OrdersAPI.Services.Events;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent evt);
}
