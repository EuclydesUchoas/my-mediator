using MyMediator.Application.Dtos.WeatherForecast;
using MyMediator.Application.Mediator;
using System.Collections.Concurrent;

namespace MyMediator.Application.Features.WeatherForecast.Queries;

public sealed class GetAllWeatherForecastQueryHandler : IRequestHandler<GetAllWeatherForecastQuery, IEnumerable<WeatherForecastDto>>
{
    public async Task<IEnumerable<WeatherForecastDto>> Handle(GetAllWeatherForecastQuery request, CancellationToken cancellationToken)
    {
        var forecasts = await Task.FromResult(Enumerable.Range(1, 5)
            .Select(index => new WeatherForecastDto(
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(index)),
                Random.Shared.Next(-20, 55),
                WeatherForecastDto.Summaries[Random.Shared.Next(WeatherForecastDto.Summaries.Count)]
                ))
            .ToArray());

        if (cancellationToken.IsCancellationRequested)
        {
            throw new TaskCanceledException("Task was canceled.");
        }

        return forecasts;
    }
}
