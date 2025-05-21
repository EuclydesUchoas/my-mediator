namespace MyMediator.Application.Dtos.WeatherForecast;

public sealed record WeatherForecastDto(
    DateOnly Date, 
    int TemperatureC, 
    string? Summary
    )
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public static readonly HashSet<string> Summaries = new(StringComparer.OrdinalIgnoreCase)
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching",
    };
}
