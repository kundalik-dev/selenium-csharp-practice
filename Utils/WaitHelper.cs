using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Selenium_CSharp_Practice.Utils;

public sealed class WaitHelper
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public WaitHelper(IWebDriver driver, TimeSpan timeout)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, timeout);
    }

    public IWebElement UntilPresent(By locator) =>
        wait.Until(drv => drv.FindElement(locator));

    public IReadOnlyCollection<IWebElement> UntilAnyPresent(By locator) =>
        wait.Until(drv =>
        {
            var elements = drv.FindElements(locator);
            return elements.Count > 0 ? elements : null;
        })!;

    public IWebElement UntilVisible(By locator, int? timeoutSeconds = null)
    {
        var activeWait = CreateWait(timeoutSeconds);

        return activeWait.Until(drv =>
        {
            try
            {
                var element = drv.FindElement(locator);
                return element.Displayed ? element : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        })!;
    }

    public IWebElement UntilClickable(By locator) =>
        wait.Until(drv =>
        {
            try
            {
                var element = drv.FindElement(locator);
                return element.Displayed && element.Enabled ? element : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        })!;

    public bool UntilDisplayed(By locator) =>
        wait.Until(drv =>
        {
            try
            {
                return drv.FindElement(locator).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });

    public bool UntilInvisible(By locator, int? timeoutSeconds = null)
    {
        var activeWait = CreateWait(timeoutSeconds);

        return activeWait.Until(drv =>
        {
            try
            {
                return !drv.FindElement(locator).Displayed;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
            catch (StaleElementReferenceException)
            {
                return true;
            }
        });
    }

    public TResult Until<TResult>(Func<IWebDriver, TResult?> condition)
        where TResult : class =>
        wait.Until(condition)!;

    public bool Until(Func<IWebDriver, bool> condition) =>
        wait.Until(condition);

    private WebDriverWait CreateWait(int? timeoutSeconds) =>
        timeoutSeconds.HasValue
            ? new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
            : wait;
}
