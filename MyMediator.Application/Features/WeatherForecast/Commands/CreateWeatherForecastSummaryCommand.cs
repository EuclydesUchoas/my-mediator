using MyMediator.Application.Mediator;

namespace MyMediator.Application.Features.WeatherForecast.Commands;

public sealed record CreateWeatherForecastSummaryCommand(
    string Summary
    ) : IRequest;
