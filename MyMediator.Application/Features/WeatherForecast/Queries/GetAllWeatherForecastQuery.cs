using MyMediator.Application.Dtos.WeatherForecast;
using MyMediator.Application.Mediator;

namespace MyMediator.Application.Features.WeatherForecast.Queries;

public sealed record GetAllWeatherForecastQuery : IRequest<IEnumerable<WeatherForecastDto>>;
