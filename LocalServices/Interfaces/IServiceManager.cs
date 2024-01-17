namespace LocalServices;

public interface IServiceManager
{

    public IDragAndDropService DragDropLS { get; init; }

    public IUserService UserServiceLS { get; init; }

}
