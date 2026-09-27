using OpenQA.Selenium;
using Selenium_CSharp_Practice.Utils;
using Selenium_CSharp_Practice.Base;

namespace Selenium_CSharp_Practice.Pages.PW_Pages
{
    public class PWLoginPage : BasePage
    {
        private static readonly By UsernameInput = By.Id("email");
        private static readonly By PasswordInput = By.Id("password");
        private static readonly By LoginButton = By.XPath("//button[@type='submit']");

        private static readonly By EmailErrorMessage = By.XPath("//p[@data-testid='email-error']");
        private static readonly By PasswordErrorMessage = By.XPath("//p[@data-testid='password-error']");
        private static readonly By ToastErrorMessage = By.XPath("//span[@class='toast__message']");


        public PWLoginPage(IWebDriver driver, WaitHelper waits) : base(driver, waits) { }

        public void EnterUsername(string username) => Type(UsernameInput, username);
        public void EnterPassword(string password) => Type(PasswordInput, password);
        public void ClickOnLoginButton() => Click(LoginButton);

        public BooksPage PwLoginAndNavigateTo(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickOnLoginButton();

            BooksPage _booksPage = new BooksPage(driver, waits);
            return _booksPage;

        }

        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickOnLoginButton();
        }

        public string GetEmailErrorMessageText()
        {
            return GetText(EmailErrorMessage);
        }

        public string GetPasswordErrorMessageText()
        {
            return GetText(PasswordErrorMessage);
        }

        public string GetToastMessageText()
        {
            return GetText(ToastErrorMessage);
        }
    }
}
