using Selenium_CSharp_Practice.Base;
using Selenium_CSharp_Practice.Config;
using Selenium_CSharp_Practice.Pages;

namespace Selenium_CSharp_Practice.Tests
{
    [TestFixture]
    public class LoginTest : BaseTest
    {
        //[OneTimeSetUp] public void Init() { }

        [Test]
        public void LogoDisplay()
        {
            LoginPage loginPage = new LoginPage(driver, wait);

            var isDis = loginPage.IsLogoDisplayed();
            Console.WriteLine(isDis);
        }

        [TestCase("standard_user", "secret_sauce")]
        public void Login_With_ValidCrendentials(string username, string password)
        {
            LoginPage loginPage = new LoginPage(driver, wait);
            InventoryPage inventoryPage = new InventoryPage(driver, wait);

            string expectedPageTitle = "Products";

            loginPage.Login(username, password);
            string actualPageTitle = inventoryPage.PageTitle(); 

            Assert.That(actualPageTitle, Is.EqualTo(expectedPageTitle));
        }
    }
}
