using Newtonsoft.Json.Linq;
using RestSharp;
using Selenium_CSharp_Practice.Models;

namespace Selenium_CSharp_Practice.Tests.RestSharpTests;

public class BasicRequest
{
    [SetUp]
    public void BaseSetup()
    {

    }

    string baseUrl = "https://jsonplaceholder.typicode.com";
    string todoEndpoint = "/todos/1";
    string postsEndpoint = "/posts";

    [Test]
    public void GetTodos()
    {
        var client = new RestClient(baseUrl);
        var request = new RestRequest(todoEndpoint, Method.Get);

        var response = client.Execute(request);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        Console.WriteLine(response.Content);

    }

    [Test]
    public void CreatePost()
    {
        var client = new RestClient(baseUrl);
        var request = new RestRequest(postsEndpoint, Method.Post);
        // request.AddHeader("Accept", "application/json");
        request.AddHeaders(new Dictionary<string, string> { { "Content-Type", "application/json; charset=UTF-8" }, { "Accept", "application/json" } });

        request.AddJsonBody(new
        {
            title = "testing jk",
            body = "just for fun",
            userId = 11,
        });


        var response = client.Execute(request);
        Console.WriteLine(response.Content);

        // status code validation
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));

        // headers validation
        Assert.That(response.Headers.Any(h => h.Name == "Content-Type" && h.Value.ToString() == "application/json"));

    }

    [Test]
    public void GetAllPosts()
    {
        var client = new RestClient(baseUrl);
        var request = new RestRequest($"{postsEndpoint}/1", Method.Get);

        var response = client.Execute(request);
        Console.WriteLine(response.Content);

        // json body data validation
        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<PostsType>(response.Content);
        Assert.That((int)data!.userId, Is.EqualTo(1));

        // Body contains requried data validation
        Assert.That(response.Content!.Contains("sunt aut facere"));

        // schema validation
        JObject obj = JObject.Parse(response.Content);
        Assert.That(obj["userId"], Is.Not.Null);


        // Response time - custome helper methods needed
        //Assert.That(response..TotalMilliseconds, Is.LessThan(2000));


    }

    [Test]
    public void UpdatePost()
    {
        var client = new RestClient(baseUrl);
        var request = new RestRequest($"{postsEndpoint}/1", Method.Put);
        request.AddJsonBody(new
        {
            title = "testing",
            body = "bars",
            userId = 1,
        });

        var response = client.Execute(request);

        Console.WriteLine(response.Content);
    }

  
}
