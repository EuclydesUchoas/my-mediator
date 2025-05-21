using MyMediator.Application.Dtos.WeatherForecast;
using MyMediator.Application.Mediator;

namespace MyMediator.Application.Features.WeatherForecast.Commands;

public sealed class CreateWeatherForecastSummaryCommandHandler : IRequestHandler<CreateWeatherForecastSummaryCommand>
{
    public async Task Handle(CreateWeatherForecastSummaryCommand request, CancellationToken cancellationToken = default)
    {
        if (!WeatherForecastDto.Summaries.Add(request.Summary))
        {
            throw new InvalidOperationException($"Summary '{request.Summary}' already exists.");
        }

        await Task.CompletedTask;
    }
}
