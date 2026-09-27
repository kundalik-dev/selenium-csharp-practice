using Selenium_CSharp_Practice.Base;
using Selenium_CSharp_Practice.Pages.PW_Pages;

namespace Selenium_CSharp_Practice.Tests.PW_Books
{
    public class PWLoginTests : BaseTest
    {
        PWLoginPage _pwLoginPage;

        [SetUp]
        public void PwLoginSetup()
        {
            _pwLoginPage = new PWLoginPage(driver, waits);
        }

        [TestCase("kundalik.dev@gmail.com", "Admin@123", "Log in")]
        public void Should_LoginWith_ValidCredentials(string username, string password, string expectedPageTitle)
        {
            var _booksPage = _pwLoginPage.PwLoginAndNavigateTo(username, password);
            string actPageTitle = _booksPage.GetPageTitle();

            Assert.That(actPageTitle, Is.EqualTo(expectedPageTitle));
        }

        [TestCase("wrong-email@gmail.com", "Admin@123", "Invalid email or password.")]
        [TestCase("kundalik.dev@gmail.com", "Wrong-password", "Invalid email or password.")]
        public void Should_Show_ToastMessage_With_InvalidCredentials(string username, string password, string errorMsgText)
        {
            _pwLoginPage.Login(username, password);
            string actToasMessage = _pwLoginPage.GetToastMessageText();

            Assert.That(actToasMessage, Does.Contain(errorMsgText));

        }

        [TestCase("", "", "Enter a valid email address.", "Password is required.")] 
        public void Should_Show_ErrorMessage_With_InvalidCredentials(string username, string password, string emailErrorMsgText, string passwordErrorMsgText)
        {
            _pwLoginPage.Login(username, password);
            string actEmailErrorMsg = _pwLoginPage.GetEmailErrorMessageText();
            string actPasswordErrorMsg = _pwLoginPage.GetPasswordErrorMessageText();

            Assert.That(actEmailErrorMsg, Does.Contain(emailErrorMsgText));
            Assert.That(actPasswordErrorMsg, Does.Contain(passwordErrorMsgText));

        }
    }
}
