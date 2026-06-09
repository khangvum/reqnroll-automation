/**
 * Program:         LoginPage.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-09
 * Description:     A class that represents the login page of Swag Labs.
 */

namespace ReqnrollAutomation.Pages
{
    internal class LoginPage : BasePage
    {
        #region Page Locators
        private readonly By _usernameFieldLocator = By.Id("user-name");
        private readonly By _passwordFieldLocator = By.Id("password");
        private readonly By _loginButtonLocator = By.Id("login-button");
        private readonly By _errorMessageLocator = By.CssSelector(".error-message-container.error h3[data-test='error']");
        #endregion

        #region Page Elements
        private IWebElement UsernameField => _driver.FindElement(_usernameFieldLocator);
        private IWebElement PasswordField => _driver.FindElement(_passwordFieldLocator);
        private IWebElement LoginButton => _driver.FindElement(_loginButtonLocator);
        private IWebElement ErrorMessage => _driver.FindElement(_errorMessageLocator);
        #endregion

        #region Constructor
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }
        #endregion

        #region Public Methods
        public void LoginAsRole(string role)
        {

        }
        #endregion
    }
}
