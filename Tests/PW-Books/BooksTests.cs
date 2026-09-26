using OpenQA.Selenium.Support.UI;
using Selenium_CSharp_Practice.Base;
using Selenium_CSharp_Practice.Pages;
using Selenium_CSharp_Practice.Pages.PW_Pages;

namespace Selenium_CSharp_Practice.Tests.PW_Books
{
    public class BooksTests : BaseTest
    {
        //private LoginPage _loginPage = null!;
        private BooksPage _booksPage = null!;

        [SetUp]
        public void LoginSetup()
        {
            _booksPage = new BooksPage(driver, wait);
        }

        [TestCase("Books")]
        public void Should_Dispaly_PageHeadingAs_Books(string expPageHeading)
        {
            string actPageHeading = _booksPage.GetPageTitle();
            Assert.That(actPageHeading, Is.EqualTo(expPageHeading));
        }
    }
}
