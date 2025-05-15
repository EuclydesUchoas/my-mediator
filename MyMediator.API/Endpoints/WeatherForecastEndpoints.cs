using Microsoft.AspNetCore.Http.HttpResults;

namespace MyMediator.API.Endpoints;

public sealed class WeatherForecastEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/v1/weatherforecast")
            .WithTags("Weather Forecast");

        group.MapGet("/", Get)
            .WithName("Get Weather Forecast");

        group.MapGet("/getbysummary/{summary}", GetBySummary)
            .WithName("Get Weather Forecast By Summary");
    }

    private static readonly string[] _summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    public static async Task<Ok<WeatherForecast[]>> Get()
    {
        var forecast = await Task.FromResult(Enumerable.Range(1, 5)
            .Select(index => new WeatherForecast(
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(index)),
                Random.Shared.Next(-20, 55),
                _summaries[Random.Shared.Next(_summaries.Length)]
                ))
            .ToArray());

        return TypedResults.Ok(forecast);
    }

    public static async Task<Results<Ok<WeatherForecast>, NotFound>> GetBySummary(string summary)
    {
        if (!_summaries.Contains(summary))
        {
            return TypedResults.NotFound();
        }

        var forecast = await Task.FromResult(
            new WeatherForecast(
                DateOnly.FromDateTime(DateTime.UtcNow),
                Random.Shared.Next(-20, 55),
                summary
                ));

        return TypedResults.Ok(forecast);
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
