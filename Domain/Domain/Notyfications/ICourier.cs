using MediatR;

namespace Domain;

public interface ICourier
{
    public delegate Task DefaultDel<T>(T notification, CancellationToken cancellationToken);

    public void Subscribe<T>(DefaultDel<T> a);


    public void UnSubscribe<T>(DefaultDel<T> a);

    public Task Publish(INotification notification);

}
