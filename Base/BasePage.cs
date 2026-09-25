using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenium_CSharp_Practice.Base
{
    public abstract class BasePage
    {
        protected readonly IWebDriver driver = null!;
        protected readonly WebDriverWait wait = null!;

        protected BasePage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }

        public void NavigateTo(string url) => driver.Navigate().GoToUrl(url);

        // Element exists in the DOM
        protected IWebElement FindElement(By locator) =>
             wait.Until(drv => drv.FindElement(locator));

        // Get Text
        protected string GetText(By locator) => FindElement(locator).Text;

        // Element is displayed
        public bool IsDisplayed(By locator) =>
           wait.Until(drv =>
            {
                var element = drv.FindElement(locator);
                return element.Displayed;
            });

        // Sendkeys 
        protected void Type(By locator, string text)
        {
            var element = FindElement(locator);
            element.Clear();
            element.SendKeys(text);
        }

        // Click
        protected void Click(By locator)
        {
            var element = FindElement(locator);
            element.Click();
        }
    }
}
