/**
 * Program:         UserAuthenticationStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-10
 * Description:     A class that defines the step definitions for the Swag Labs login feature.
 */

using AventStack.ExtentReports.Gherkin.Model;
using ReqnrollAutomation.Pages;

namespace ReqnrollAutomation.StepDefinitions
{
    /// <summary>
    /// A class that defines the step definitions for the Swag Labs login feature.
    /// </summary>
    [Binding]
    public class UserAuthenticationStepDefinitions : BaseStepDefinitions
    {
        #region Private Attributes
        private readonly LoginPage _loginPage;
        #endregion

        #region Constructor
        public UserAuthenticationStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
            _loginPage = new(Driver);
        }
        #endregion

        #region Step Definitions
        [Given("I am on the Swag Labs login page")]
        public void GivenIAmOnTheSwagLabsLoginPage()
        {
            _loginPage.Navigate();
        }

        [When("I enter standard user credentials")]
        public void WhenIEnterValidCredentials()
        {
            _loginPage.LoginAsRole("StandardUser");
        }

        [When("I enter locked-out user credentials")]
        public void WhenIEnterLockedOutUserCredentials()
        {
            _loginPage.LoginAsRole("LockedOutUser");
        }

        [When("I enter invalid credentials")]
        public void WhenIEnterInvalidCredentials()
        {
            IWebElement usernameInput = Driver.FindElement(By.Id("user-name"));
            IWebElement passwordInput = Driver.FindElement(By.Id("password"));
            IWebElement loginButton = Driver.FindElement(By.Id("login-button"));

            usernameInput.SendKeys("locked_out_user");
            passwordInput.SendKeys("secret_sauce");
            loginButton.Click();
        }


        [Then(@"I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            Assert.IsTrue(Driver.Url.Contains("https://www.saucedemo.com/inventory.html"), "User was not logged in successfully.");
        }

        [Then(@"I should see an error message")]
        public void ThenIShouldSeeAnErrorMessage()
        {
            IWebElement errorMessage = Driver.FindElement(By.CssSelector(".error-message-container"));
            Assert.IsTrue(errorMessage.Displayed, "Error message was not displayed.");
        }
        #endregion
    }
}
