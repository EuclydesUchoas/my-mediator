namespace MyMediator.Application.Mediator;

public sealed class Sender(IServiceProvider serviceProvider) : ISender
{
    // Modelo com dynamic
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        var handler = serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"Handler for {requestType.Name} not found.");

        var result = await ((dynamic)handler).Handle((dynamic)request, cancellationToken);

        return result;
    }

    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);

        var handler = serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"Handler for {requestType.Name} not found.");

        await ((dynamic)handler).Handle((dynamic)request, cancellationToken);
    }

    // Modelo com reflection e caching
    /*private static readonly ConcurrentDictionary<Type, Type?> _requestTypeCache = [];
    private static readonly ConcurrentDictionary<Type, MethodInfo?> _handlerMethodCache = [];
    
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = _requestTypeCache.GetOrAdd(
            requestType,
            (key) => typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse)))
            ?? throw new InvalidOperationException($"Handler for {requestType.Name} not found.");

        var handler = serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"Handler for {requestType.Name} not found.");

        var handlerMethod = _handlerMethodCache.GetOrAdd(
            handlerType,
            (key) => key.GetMethod("Handle", BindingFlags.Public | BindingFlags.Instance, null, [requestType, typeof(CancellationToken)], null))
            ?? throw new InvalidOperationException($"Handle method not found in {handlerType.Name}.");

        var task = (Task<TResponse>)handlerMethod.Invoke(handler, [request, cancellationToken])!;

        var result = await task;

        return result;
    }

    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var handlerType = _requestTypeCache.GetOrAdd(
            requestType,
            (key) => typeof(IRequestHandler<>).MakeGenericType(requestType))
            ?? throw new InvalidOperationException($"Handler for {requestType.Name} not found.");

        var handler = serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"Handler for {requestType.Name} not found.");

        var handlerMethod = _handlerMethodCache.GetOrAdd(
            handlerType,
            (key) => key.GetMethod("Handle", BindingFlags.Public | BindingFlags.Instance, null, [requestType, typeof(CancellationToken)], null))
            ?? throw new InvalidOperationException($"Handle method not found in {handlerType.Name}.");

        var task = (Task)handlerMethod.Invoke(handler, [request, cancellationToken])!;

        await task;
    }*/
}
