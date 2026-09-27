using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Selenium_CSharp_Practice.Tests.PracticeTests;

public class Practice01
{
    private IWebDriver driver;

    [SetUp]
    public void OpenBrowser()
    {
        driver = new ChromeDriver();

        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        driver.Navigate().GoToUrl("https://testautomationpractice.blogspot.com/");
    }

    [TearDown]
    public void CloseBrowser()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void PrintAllLinks()
    {
        IReadOnlyCollection<IWebElement> links = driver.FindElements(By.TagName("a")).ToList();

        Console.WriteLine(links.Count);

        foreach (IWebElement link in links)
        {

            // string href = link.GetAttribute("href");
            // if (string.IsNullOrEmpty(href)) continue;
            // Console.WriteLine(href);

            // HttpClient client = new HttpClient();
            // var response = client.GetAsync(href).Result;

            // if (!response.IsSuccessStatusCode) // status >= 400
            // {
            //     Console.WriteLine($"{href} - Status: {response.StatusCode}");
            // }
            Console.WriteLine(link.Text);
        }
    }

    [Test]
    public void AlertHandling()
    {

        // driver.FindElement(By.CssSelector("// div button#alertBtn")).Click();
        IWebElement als = driver.FindElement(By.Id("alertBtn"));

        Console.WriteLine(als.Text);
        als.Click();
        IAlert alert = driver.SwitchTo().Alert();
        string alertTxt = alert.Text;
        alert.Accept();

        Assert.That(alertTxt, Is.EqualTo("I am an alert box!"));
    }

    [TestCase("hello jk")]
    public void AlertPrompt(string promptText)
    {
        IWebElement promptButton = driver.FindElement(By.Id("promptBtn"));
        promptButton.Click();

        IAlert alert = driver.SwitchTo().Alert();
        alert.SendKeys(promptText);
        alert.Accept();

        IWebElement result = driver.FindElement(By.Id("demo"));

        Assert.That(result.Text, Does.Contain(promptText));
    }

    [Test]
    public void ChildWindowHandles()
    {
        IWebElement newTab = driver.FindElement(By.XPath("//button[text()='New Tab']"));
        newTab.Click();

        string mainPageId = driver.CurrentWindowHandle;
        Console.WriteLine($"Main page id is {mainPageId}");

        ReadOnlyCollection<string> allHandles = driver.WindowHandles;

        foreach (string handle in allHandles)
        {
            Console.WriteLine($"handle is {handle}");
        }

        List<string> handleIdList = allHandles.ToList();

        Console.WriteLine($"handle 1 is - {handleIdList[0]}");
        Console.WriteLine($"handle 2 is - {handleIdList[1]}");

        string mainHandleHeading = driver.Title;

        string handle2Heading = driver.SwitchTo().Window(handleIdList[1]).Title;
        string afterSwitch = driver.Title;

        Console.WriteLine($"main heading is - {mainHandleHeading}");
        Console.WriteLine($"tab2 heading is - {handle2Heading}");
        Console.WriteLine($"afterSwitch heading is - {afterSwitch}");

    }

    [Test]
    public void AddCookies()
    {
        // cook = { "username" = "kundalik","password" = "123"};

        var cookies = new List<Cookie>
        {
            new Cookie("username","jk"),
            new Cookie("password","1234"),
        };

        foreach (var cookie in cookies)
        {
            driver.Manage().Cookies.AddCookie(cookie);
        }


        ReadOnlyCollection<Cookie> allCookies = driver.Manage().Cookies.AllCookies;

        foreach (Cookie coo in allCookies)
        {
            Console.WriteLine(coo);
        }
 
    }

}
