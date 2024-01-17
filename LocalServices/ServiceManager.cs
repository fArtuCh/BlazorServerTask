namespace LocalServices;

public sealed class ServiceManager : IServiceManager
{
    public ServiceManager(IDragAndDropService dragAndDropService, IUserService userService )
    {
        DragDropLS = dragAndDropService;
        UserServiceLS = userService;
    }

   public  IDragAndDropService DragDropLS { get; init; }
   public IUserService UserServiceLS { get; init; }
}
