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

        [Test]
        public void Should_Display_Chrome_CPU_Ussage()
        {
            //string browserName = "Chrome";
            //string headerName = "CPU (%)";
            string browserName = "Firefox";
            string headerName = "Memory (MB)";

            var headers = driver.FindElements(By.XPath("//table[@id='taskTable']//th"));
            var rows = driver.FindElements(By.XPath("//table[@id='taskTable']//tbody//tr"));
            int headerIndex = headers
                            .Select((h, i) => new { h.Text, Index = i })
                            .First(x => x.Text == headerName).Index;

            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells[0].Text == browserName)
                {
                    string cellValue = cells[headerIndex].Text;
                    Console.WriteLine($"{browserName} {headerName} Usage: {cellValue}");
                    break;
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
            driver.Quit();
        }
    }
}
