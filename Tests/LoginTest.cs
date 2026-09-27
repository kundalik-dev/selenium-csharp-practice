using Selenium_CSharp_Practice.Base;
using Selenium_CSharp_Practice.Pages;

namespace Selenium_CSharp_Practice.Tests
{
    [TestFixture]
    public class LoginTest : BaseTest
    {

        [Test]
        public void LogoDisplay()
        {
            LoginPage loginPage = new LoginPage(driver, waits);
            var isDis = loginPage.IsLogoDisplayed();
            Assert.That(isDis, Is.True);
        }

        [Category("Smoke")]
        [TestCase("standard_user", "secret_sauce", "Products")]
        public void Login_With_ValidCrendentials(string username, string password, string expectedPageTitle)
        {
            LoginPage loginPage = new LoginPage(driver, waits);
            InventoryPage inventoryPage = new InventoryPage(driver, waits);

            loginPage.Login(username, password);
            string actualPageTitle = inventoryPage.GetPageTitle();

            Assert.That(actualPageTitle, Is.EqualTo(expectedPageTitle));
        }

        [Category("Smoke")]
        [TestCase("standard_user", "secret_sauce", "Products")]
        public void Login_With_ValidCrendentials_Should_NavigateTo_InventoryPage(string username, string password, string expectedPageTitle)
        {
            LoginPage loginPage = new LoginPage(driver, waits);

            string actualPageTitle = loginPage.LoginAndNavigate(username, password).GetPageTitle();
            Assert.That(actualPageTitle, Is.EqualTo(expectedPageTitle));
        }

        [TestCase("wrong_username", "secret_sauce", "Epic sadface: Username and password do not match any user in this service")]
        [TestCase("locked_out_user", "secret_sauce", "Epic sadface: Sorry, this user has been locked out.")]
        [TestCase("", "secret_sauce", "Epic sadface: Username is required")]
        [TestCase("standard_user", "", "Epic sadface: Password is required")]
        public void Login_With_InalidCredential_Should_Show_ErrorMessage(string username, string password, string expectedErrorMsg)
        {
            LoginPage loginPage = new LoginPage(driver, waits);

            loginPage.Login(username, password);

            string actualErrorMsg = loginPage.ErrorMessageText();

            Assert.That(actualErrorMsg, Is.EqualTo(expectedErrorMsg));
        }
    }
}
