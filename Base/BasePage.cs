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

        // Navigate To
        public void NavigateTo(string url) => driver.Navigate().GoToUrl(url);

        // Element exists in the DOM - Wait For Element Presnet
        protected IWebElement FindElement(By locator) =>
             wait.Until(drv => drv.FindElement(locator));

        // protected IWebElement WaitForElementPresent(By locator) =>
        //     wait.Until(driver => driver.FindElement(locator));

        // Find Elements
        protected IReadOnlyCollection<IWebElement> FindElements(By locator) =>
           wait.Until(driver =>
           {
               var elements = driver.FindElements(locator);
               return elements.Count > 0 ? elements : null;
           })!;

        // Waits until at least one matching element exists.
        protected IReadOnlyCollection<IWebElement> WaitForElements(By locator) =>
            wait.Until(driver =>
            {
                var elements = driver.FindElements(locator);
                return elements.Count > 0 ? elements : null;
            })!;

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
            var element = wait.Until(driver =>
            {
                var input = driver.FindElement(locator);
                return input.Displayed && input.Enabled ? input : null;
            });

            element.Clear();
            element.SendKeys(text);
        }

        // Click
        protected void Click(By locator)
        {
            var element = wait.Until(driver =>
            {
                var button = driver.FindElement(locator);
                return button.Displayed && button.Enabled ? button : null;
            });
            element.Click();
        }
    }
}
