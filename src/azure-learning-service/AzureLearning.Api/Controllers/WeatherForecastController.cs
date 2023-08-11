using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace AzureLearning.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IRedisClientFactory _redisClientFactory;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IRedisClientFactory redisClientFactory)
    {
        _logger = logger;
        _redisClientFactory = redisClientFactory;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>?> Get()
    {
        var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        var redisClient = _redisClientFactory.GetDefaultRedisClient();
        var redisDatabase1 = redisClient.Db0;
        var redisDatabase2 = redisClient.Db1;
        if (await redisDatabase1.ExistsAsync("weather"))
        {
            var result = await redisDatabase1.GetAsync<IEnumerable<WeatherForecast>>("weather");
            await redisDatabase1.RemoveAsync("weather");
            return result;
        }
        
        await redisDatabase2.AddAsync("weather", weather);

        return weather;
    }
}