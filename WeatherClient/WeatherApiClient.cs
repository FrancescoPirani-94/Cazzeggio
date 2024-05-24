using Core.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

namespace WeatherClient
{
    public class WeatherApiClient
    {
        private readonly string? _apiKey;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializeOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public WeatherApiClient()
        {
            var config = InitializeConfiguration();
            _apiKey = config["ApiKey"] ?? string.Empty;
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(config["HttpBaseAddress"] ?? string.Empty)
            };

            _httpClient.DefaultRequestHeaders.Add("key", _apiKey);
        }

        private static IConfiguration InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appconfig.json", optional: false, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true);

            return builder.Build();
        }

        public async Task<WeatherApiResponseModel> GetCurrentWeatherAsync(
            string city)
        {
            var response = await _httpClient.GetAsync($"current.json?q={city}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<WeatherApiResponseModel>(jsonResponse) ?? new();

            return deserializedResponse;
        }
    }
}
