using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Selenium_CSharp_Practice.Config;

namespace Selenium_CSharp_Practice.Base
{
    [TestFixture]
    public abstract class BaseTest
    {
        protected IWebDriver driver { get; private set; } = null!;
        protected WebDriverWait wait { get; private set; } = null!;
        protected AppConfig appSettings { get; private set; } = null!;

        [SetUp]
        public void BaseSetup()
        {
            appSettings = ConfigReader.Settings;
            driver = OpenBrowser(appSettings);

            wait = new WebDriverWait(driver,
                TimeSpan.FromSeconds(appSettings.ExplicitWaitSeconds));

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl(appSettings.BaseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
            driver.Quit();
        }


        public static IWebDriver OpenBrowser(AppConfig config)
        {
            string browserName = config.Browser.ToLower().Trim();

            return browserName switch
            {
                "chrome" => OpenChromeBrowser(config),
                "firefox" => OpenFireFoxBrowser(config),
                "edge" => OpenEdgeBrowser(config),
                _ => throw new NotSupportedException($"Browser '{config.Browser}' is not supported.")
            };

        }


        public static IWebDriver OpenChromeBrowser(AppConfig config)
        {
            ChromeOptions options = new ChromeOptions();

            if (config.Headless)
            {
                options.AddArgument("--headless=new");
            }

            foreach (var argument in config.Options)
            {
                options.AddArgument(argument);
            }

            return new ChromeDriver(options);
        }
        public static IWebDriver OpenFireFoxBrowser(AppConfig config)
        {
            FirefoxOptions options = new(); // new way to declare new FireFox() object

            if (config.Headless) options.AddArgument("--headless=new");

            foreach (var argument in config.Options)
            {
                options.AddArgument(argument);
            }

            return new FirefoxDriver(options);
        }

        public static IWebDriver OpenEdgeBrowser(AppConfig config)
        {
            EdgeOptions options = new(); // new way to declare new FireFox() object

            if (config.Headless) options.AddArgument("--headless=new");

            foreach (var argument in config.Options)
            {
                options.AddArgument(argument);
            }

            return new EdgeDriver(options);
        }


    }
}
