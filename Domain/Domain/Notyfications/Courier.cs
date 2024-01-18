using MediatR;

namespace Domain;

public class Courier : ICourier,
    INotificationHandler<NotificationGroupChange>,
    INotificationHandler<NotificationShiftKeyRealeased>,
    INotificationHandler<NotificationOrderOfUserChanged>
{
    private readonly Dictionary<Type, List<Delegate>> handlers = new();

    public void Subscribe<T>(ICourier.DefaultDel<T> handler)
    {
        Type type = typeof(T);
        if (!handlers.TryGetValue(type, out List<Delegate>? value))
        {
            value = new();
            handlers[type] = value;
        }

        value.Add(handler);
    }

    public void UnSubscribe<T>(ICourier.DefaultDel<T> handler)
    {
        Type type = typeof(T);
        if (handlers.TryGetValue(type, out var handlerList))
        {
            handlerList.Remove(handler);
        }
    }
    private Task HandleNotification(Type type, object notification, CancellationToken cancellationToken)
    {
        if (handlers.TryGetValue(type, out var handlerList))
        {
            foreach (var handler in handlerList)
            {
                handler.DynamicInvoke(notification, cancellationToken);
            }
        }

        return Task.CompletedTask;
    }

    public Task Handle(NotificationGroupChange notification, CancellationToken cancellationToken)
         => HandleNotification(typeof(NotificationGroupChange), notification, cancellationToken);

    public Task Handle(NotificationShiftKeyRealeased notification, CancellationToken cancellationToken)
     => HandleNotification(typeof(NotificationShiftKeyRealeased), notification, cancellationToken);
    public Task Handle(NotificationOrderOfUserChanged notification, CancellationToken cancellationToken)
 => HandleNotification(typeof(NotificationOrderOfUserChanged), notification, cancellationToken);


    public Task Publish(INotification notification)
    {
        Type type = notification.GetType();
        CancellationToken cancellationToken = default;

        return HandleNotification(type, notification, cancellationToken);
    }
}
