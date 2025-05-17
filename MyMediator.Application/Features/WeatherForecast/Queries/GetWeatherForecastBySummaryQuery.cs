using MyMediator.Application.Dtos.WeatherForecast;
using MyMediator.Application.Mediator;

namespace MyMediator.Application.Features.WeatherForecast.Queries;

public sealed record GetWeatherForecastBySummaryQuery(
    string Summary
    ) : IRequest<WeatherForecastDto?>;
