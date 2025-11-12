using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WeatherWebApp.Services;

namespace WeatherWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City is required");
            }

            try
            {
                var weatherData = await _weatherService.GetWeatherAsync(city);
                if (weatherData == null)
                {
                    return NotFound("City not found");
                }
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while fetching weather data.");
            }
        }
    }
}