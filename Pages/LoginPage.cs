using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium_CSharp_Practice.Base;

namespace Selenium_CSharp_Practice.Pages
{
    public class LoginPage : BasePage
    {
        private static readonly By Logo = By.ClassName("login_logo");
        private static readonly By UsernameInput = By.Id("user-name");
        private static readonly By PasswordInput = By.Id("password");
        private static readonly By LoginButton = By.Id("login-button");

        private static readonly By ErrorMessageHeading = By.XPath("//h3");

        public LoginPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
        }

        public bool IsLogoDisplayed() => IsDisplayed(Logo);
        public void EnterUsername(string username) => Type(UsernameInput, username);
        public void EnterPassword(string password) => Type(PasswordInput, password);
        public void ClickLogin() => Click(LoginButton);

        // Valid Login navigate to dashboard page
        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
        }

        public InventoryPage LoginAndNavigate(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();

            var _inventoryPage = new InventoryPage(driver, wait);
            _inventoryPage.GetPageTitle();
            return _inventoryPage;
        }

        public void InvalidLogin(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
        }

        public string ErrorMessageText()
        {
            return GetText(ErrorMessageHeading);
        }


    }
}
