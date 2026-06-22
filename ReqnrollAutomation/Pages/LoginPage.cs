/**
 * Program:         LoginPage.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-09
 * Description:     A class that represents the login page of Swag Labs.
 */

using ReqnrollAutomation.Helpers;

namespace ReqnrollAutomation.Pages
{
    /// <summary>
    /// A class that represents the login page of Swag Labs.
    /// </summary>
    public class LoginPage : BasePage
    {
        #region Public Properties
        public override string PageUrl => "https://www.saucedemo.com/";
        #endregion

        #region Page Locators
        private readonly By _usernameFieldLocator = By.Id("user-name");
        private readonly By _passwordFieldLocator = By.Id("password");
        private readonly By _loginButtonLocator = By.Id("login-button");
        private readonly By _errorMessageContainerLocator = By.CssSelector(".error-message-container.error h3[data-test='error']");
        #endregion

        #region Page Elements
        private IWebElement UsernameField => WaitAndFindElement(_usernameFieldLocator);
        private IWebElement PasswordField => WaitAndFindElement(_passwordFieldLocator);
        private IWebElement LoginButton => WaitAndFindElement(_loginButtonLocator);
        private IWebElement ErrorMessageContainer => WaitAndFindElement(_errorMessageContainerLocator);
        #endregion

        #region Constructor
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Navigates to the login page.
        /// </summary>
        public void Navigate() => _driver.Navigate().GoToUrl(PageUrl);

        /// <summary>
        /// Logs in using the credentials associated with the specified role.
        /// </summary>
        /// <param name="role">The role for which to use credentials.</param>
        public void LoginAsRole(string role)
        {
            string username = CredentialManager.Credentials.Accounts[role];
            string password = CredentialManager.Credentials.SharedPassword;

            LoginWithCredentials(username, password);
        }

        /// <summary>
        /// Logs in using the specified username and password.
        /// </summary>
        /// <param name="username">The username to use for login.</param>
        /// <param name="password">The password to use for login.</param>
        public void LoginWithCredentials(string username, string password)
        {
            UsernameField.SendKeys(username);
            PasswordField.SendKeys(password);
            LoginButton.Click();
        }

        /// <summary>
        /// Retrieves the error message displayed on the login page, if any.
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage() => ErrorMessageContainer.Text;
        #endregion
    }
}
