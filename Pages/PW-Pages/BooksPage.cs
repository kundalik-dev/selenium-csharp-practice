using OpenQA.Selenium;
using Selenium_CSharp_Practice.Utils;
using Selenium_CSharp_Practice.Base;

namespace Selenium_CSharp_Practice.Pages.PW_Pages
{
    public class BooksPage : BasePage
    {
        private static readonly By PageHeading = By.XPath("//h1");

        public BooksPage(IWebDriver driver, WaitHelper waits) : base(driver, waits) { }

        public string GetPageTitle()
        {
            return GetText(PageHeading);
        }

    }
}
