using MyMediator.Application.Dtos.WeatherForecast;
using MyMediator.Application.Mediator;

namespace MyMediator.Application.Features.WeatherForecast.Queries;

public sealed class GetWeatherForecastBySummaryQueryHandler : IRequestHandler<GetWeatherForecastBySummaryQuery, WeatherForecastDto?>
{
    public async Task<WeatherForecastDto?> Handle(GetWeatherForecastBySummaryQuery request, CancellationToken cancellationToken)
    {
        if (!WeatherForecastDto.Summaries.TryGetValue(request.Summary, out string? summary))
        {
            return null;
        }

        var forecast = await Task.FromResult(
            new WeatherForecastDto(
                DateOnly.FromDateTime(DateTime.UtcNow), 
                Random.Shared.Next(-20, 55),
                summary
                ));

        if (cancellationToken.IsCancellationRequested)
        {
            throw new TaskCanceledException("Task was canceled.");
        }

        return forecast;
    }
}
