using WeatherClient;

namespace Weather
{
    public class Program
    {
        internal static async Task Main(string[] args)
        {
            var _httpCLient = new WeatherApiClient();

            var response = await _httpCLient.GetCurrentWeatherAsync("Pesaro");
        }
    }
}
