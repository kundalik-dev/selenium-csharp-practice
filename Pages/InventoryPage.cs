using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium_CSharp_Practice.Base;

namespace Selenium_CSharp_Practice.Pages
{
    public class InventoryPage : BasePage
    {
        private static readonly By PageTitel = By.XPath("//span[text()='Products']");

        public InventoryPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
        }

        public string PageTitle()
        {
          return GetText(PageTitel);
        }


    }
}
