using LabCMS.FixtureDomain.Server.EventHandles;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using Microsoft.AspNetCore.Mvc;

namespace LabCMS.FixtureDomain.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly FixtureDomainRepository _repository;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
        FixtureDomainRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        FixtureEventHandlersFactory factory = new();
        var handler = factory.Create<FixtureAcceptanceCheckEventHandler>(_repository);
        await handler.HandleAsync(new FixtureAcceptanceCheckEvent(123));

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
