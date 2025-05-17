namespace MyMediator.Application.Mediator;

public interface IRequest<TResponse>
{
    // Marker interface for requests that return a response of type TResponse
    // This interface can be used to define the contract for request handlers
    // that process requests and return a response of the specified type.
}
