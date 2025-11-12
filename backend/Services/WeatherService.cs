using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WeatherWebApp.Models;

namespace WeatherWebApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeather:ApiKey"];
        }

        public async Task<WeatherResponse> GetWeatherAsync(string city)
        {
            var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={_apiKey}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var weatherData = JsonDocument.Parse(jsonString).RootElement;

            var weatherResponse = new WeatherResponse
            {
                City = weatherData.GetProperty("name").GetString(),
                Temperature = weatherData.GetProperty("main").GetProperty("temp").GetDouble(),
                Condition = weatherData.GetProperty("weather")[0].GetProperty("main").GetString(),
                Description = weatherData.GetProperty("weather")[0].GetProperty("description").GetString(),
                Icon = $"http://openweathermap.org/img/wn/{weatherData.GetProperty("weather")[0].GetProperty("icon").GetString()}@2x.png"
            };

            return weatherResponse;
        }
    }
}