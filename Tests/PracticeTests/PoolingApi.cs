using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using RestSharp;
using Selenium_CSharp_Practice.Models;

namespace Selenium_CSharp_Practice.Tests.PracticeTests
{
    public class PoolingApi
    {
        private readonly string baseUrl = "http://localhost:3000";

        // [Test]
        // public void ValidateResponseContainsExpectedText()
        // {
        //     var client = new RestClient(baseUrl);
        //     var request = new RestRequest("/todos", Method.Get);

        //     var response = client.Execute(request);

        //     // Ensure response is not null
        //     Assert.IsNotNull(response, "Response object was null");
        //     Assert.IsNotNull(response.Content, "Response content was null");

        //     Console.WriteLine("Response Content: " + response.Content);

        //     // Case-insensitive check for expected text
        //     Assert.IsTrue(
        //         response.Content.Contains("Learn API testing", StringComparison.OrdinalIgnoreCase),
        //         "Response did not contain the expected text 'Learn API testing'."
        //     );
        // }

        [Test]
        public void GetTodos()
        {
            using var client = new RestClient(baseUrl);
            var request = new RestRequest("/todos", Method.Get);

            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var content = response.Content
                ?? throw new AssertionException("Response content was null");
            var todos = JsonConvert.DeserializeObject<List<TodoTypes>>(content)
                ?? throw new AssertionException("Deserialization failed, todos is null");

            Assert.That(todos, Is.Not.Empty, "No todos returned");
            Assert.That(
                todos.Any(t => t.title.Contains("selenium", StringComparison.OrdinalIgnoreCase)),
                Is.True,
                "No todo title contained 'selenium'"
            );
        }

        [Test]
        public void ValidateTodosJsonSchema()
        {
            using var client = new RestClient(baseUrl);
            var request = new RestRequest("/todos", Method.Get);

            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            var content = response.Content
                ?? throw new AssertionException("Response content was null");
            // Parse any JSON value so the schema also checks the root array type.
            var json = JToken.Parse(content);

            var schemaJson = """
            {
              "type": "array",
              "items": {
                "type": "object",
                "properties": {
                  "id": { "type": "integer" },
                  "title": { "type": "string" },
                  "completed": { "type": "boolean" }
                },
                "required": ["id", "title", "completed"]
              }
            }
            """;

            var schema = JSchema.Parse(schemaJson);
            bool valid = json.IsValid(schema, out IList<string> errors);

            Assert.That(valid, Is.True,
                $"Response did not match expected schema: {string.Join("; ", errors)}");
        }
    }
}
