﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyMediator.Application.Dtos.WeatherForecast;
using MyMediator.Application.Features.WeatherForecast.Commands;
using MyMediator.Application.Features.WeatherForecast.Queries;
using MyMediator.Application.Mediator;

namespace MyMediator.API.Endpoints;

public sealed class WeatherForecastEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/v1/weatherforecast")
            .WithTags("Weather Forecast");

        group.MapPost("/createsummary", CreateSummary)
            .WithName("Create Weather Forecast Summary");

        group.MapGet("/", Get)
            .WithName("Get Weather Forecast");

        group.MapGet("/getbysummary/{summary}", GetBySummary)
            .WithName("Get Weather Forecast By Summary");
    }

    public static async Task<Ok<IEnumerable<WeatherForecastDto>>> Get([FromServices] ISender sender)
    {
        var forecasts = await sender.Send(new GetAllWeatherForecastQuery());

        return TypedResults.Ok(forecasts);
    }

    public static async Task<Results<Ok<WeatherForecastDto>, NotFound>> GetBySummary([FromServices] ISender sender, string summary)
    {
        var forecast = await sender.Send(new GetWeatherForecastBySummaryQuery(summary));

        if (forecast is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(forecast);
    }

    public static async Task<Results<Created<string>, BadRequest<string>>> CreateSummary([FromServices] ISender sender, string summary)
    {
        try
        {
            await sender.Send(new CreateWeatherForecastSummaryCommand(summary));

            return TypedResults.Created(nameof(CreateSummary), summary);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
