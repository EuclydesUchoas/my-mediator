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

        var handlerInterfaceVoid = typeof(IRequestHandler<>);
        var handlerInterfaceReturn = typeof(IRequestHandler<,>);

        Type[] handlerInterfaces = [handlerInterfaceVoid, handlerInterfaceReturn];

        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition()))
                .Select(i => new { HandlerType = t, InterfaceType = i }));

        foreach (var handlerType in handlerTypes)
        {
            services.AddScoped(handlerType.InterfaceType, handlerType.HandlerType);
        }

        return services;
    }
}
