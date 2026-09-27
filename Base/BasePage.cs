using OpenQA.Selenium;
using Selenium_CSharp_Practice.Utils;

namespace Selenium_CSharp_Practice.Base
{
    public abstract class BasePage
    {
        protected readonly IWebDriver driver = null!;
        protected readonly WaitHelper waits = null!;

        protected BasePage(IWebDriver driver, WaitHelper waits)
        {
            this.driver = driver;
            this.waits = waits;
        }

        // Navigate To
        public void NavigateTo(string url) => driver.Navigate().GoToUrl(url);

        // Element exists in the DOM - Wait For Element Present
        protected IWebElement FindElement(By locator) =>
            waits.UntilPresent(locator);

        protected IWebElement WaitForElementVisible(By locator, int? timeoutSeconds = null) =>
            waits.UntilVisible(locator, timeoutSeconds);

        // Find Elements
        protected IReadOnlyCollection<IWebElement> FindElements(By locator) =>
            waits.UntilAnyPresent(locator);

        // Waits until at least one matching element exists.
        protected IReadOnlyCollection<IWebElement> WaitForElements(By locator) =>
            waits.UntilAnyPresent(locator);

        // Get Text
        protected string GetText(By locator) => FindElement(locator).Text;

        // Element is displayed
        public bool IsDisplayed(By locator) =>
            waits.UntilDisplayed(locator);

        // Sendkeys
        protected void Type(By locator, string text)
        {
            var element = waits.UntilClickable(locator);
            element.Clear();
            element.SendKeys(text);
        }

        // Click
        protected void Click(By locator) =>
            waits.UntilClickable(locator).Click();
    }
}
