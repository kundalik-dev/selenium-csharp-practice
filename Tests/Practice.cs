using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium_CSharp_Practice.Tests
{
    public class StaticTableTests
    {
        public IWebDriver driver;
        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();

            //options.AddArgument("--headless");

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://testautomationpractice.blogspot.com/");
        }

        [TestCase("Master In Selenium", "3000")]
        [TestCase("Master In JS", "1000")]
        [Test]
        public void BookPrice_StaticTable(string bookName, string expectedBookPrice)

        { 
            IWebElement row = driver.FindElement(By.XPath($"//tr[td[text()='{bookName}']]"));

            // From that row, get the 4th column (price)
            string actBookPrice = row.FindElement(By.XPath("./td[4]")).Text;

            Assert.That(actBookPrice, Is.EqualTo(expectedBookPrice));

        }

        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
            driver.Quit();
        }
    }
}
