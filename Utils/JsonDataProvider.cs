using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Selenium_CSharp_Practice.Utils
{
    public static class JsonDataProvider
    {
        public static T GetTestData<T>(string relativePath)
        {
            var fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Test data file not found: '{fullPath}'.");

            var json = File.ReadAllText(fullPath);

            return JsonSerializer.Deserialize<T>(json, Options)
            ?? throw new InvalidOperationException(
                $"Could not deserialize '{fullPath}'.");
        }

        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
