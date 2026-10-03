using System.Text.Json;

namespace Selenium_CSharp_Practice.Config
{
   

    public static class ConfigReader
    {
        // private static readonly string configFileName = "config.json";
        private static readonly string configFileName = "demo-config.json";

        private static readonly Lazy<AppConfig> LazySettings = new(LoadConfig);
        public static AppConfig Settings => LazySettings.Value;


        private static AppConfig LoadConfig()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Config", configFileName);
            var json = File.ReadAllText(path);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<AppConfig>(json, options)
                   ?? throw new InvalidOperationException($"Unable to load configuration from '{path}'.");
        }
    }
}
