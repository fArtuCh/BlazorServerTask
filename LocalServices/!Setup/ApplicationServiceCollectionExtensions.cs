using Microsoft.Extensions.DependencyInjection;

namespace LocalServices;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddACHLocalServices(this IServiceCollection services)
    {

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDragAndDropService, DragAndDropService>();


        services.AddScoped<IServiceManager, ServiceManager>();

        
        return services;
    }




}
