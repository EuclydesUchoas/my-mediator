using Microsoft.Extensions.DependencyInjection;
using MyMediator.Application.Mediator;
using System.Reflection;

namespace MyMediator.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator();

        return services;
    }

    private static IServiceCollection AddMediator(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetExecutingAssembly();
        
        services.AddScoped<ISender, Sender>();

        var handlerInterface = typeof(IRequestHandler<,>);

        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
                .Select(i => new { HandlerType = t, InterfaceType = i }));

        foreach (var handlerType in handlerTypes)
        {
            services.AddScoped(handlerType.InterfaceType, handlerType.HandlerType);
        }

        return services;
    }
}
