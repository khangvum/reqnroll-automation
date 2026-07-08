/**
 * Program:         UserAuthenticationStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-10
 * Description:     A class that defines the step definitions for the user authentication feature.
 */

using AventStack.ExtentReports.Gherkin.Model;
using ReqnrollAutomation.Pages;

namespace ReqnrollAutomation.StepDefinitions.SwagLabs
{
    /// <summary>
    /// A class that defines the step definitions for the user authentication feature.
    /// </summary>
    [Binding]
    public class UserAuthenticationStepDefinitions : BaseStepDefinitions
    {
        #region  Registry
        private enum ValidationKey
        {
            SuccessfulLoginUrl,
            LockoutMessage,
            InvalidCredentialsMessage
        }

        private readonly Dictionary<ValidationKey, string> _registry;
        #endregion

        #region Constructor
        public UserAuthenticationStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
            _registry = new()
            {
                { ValidationKey.SuccessfulLoginUrl, InventoryPage.PageUrl },
                { ValidationKey.LockoutMessage, "Sorry, this user has been locked out" },
                { ValidationKey.InvalidCredentialsMessage, "Username and password do not match any user in this service" }
            };
        }
        #endregion

        #region Given Steps
        [Given("I am on the Swag Labs login page")]
        public void GivenIAmOnTheSwagLabsLoginPage()
        {
            LoginPage.Navigate();
        }
        #endregion

        #region When Steps
        [When("I enter standard user credentials")]
        public void WhenIEnterValidCredentials()
        {
            LoginPage.LoginAsRole("StandardUser");
        }

        [When("I enter locked-out user credentials")]
        public void WhenIEnterLockedOutUserCredentials()
        {
            LoginPage.LoginAsRole("LockedOutUser");
        }

        [When("I enter invalid credentials")]
        public void WhenIEnterInvalidCredentials()
        {
            LoginPage.LoginWithCredentials("invalid_user", "invalid_password");
        }
        #endregion

        #region Then Steps
        [Then("I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            Assert.Contains(_registry[ValidationKey.SuccessfulLoginUrl], Driver.Url, "User was not logged in successfully.");
        }   

        [Then("I should see a lockout message")]
        public void ThenIShouldSeeALockoutMessage()
        {
            string actualErrorMessage = LoginPage.GetErrorMessage();
            string expectedErrorMessage = _registry[ValidationKey.LockoutMessage];
            Assert.Contains(expectedErrorMessage, actualErrorMessage, "Lockout message was not displayed.");
        }

        [Then("I should see an error message")]
        public void ThenIShouldSeeAnErrorMessage()
        {
            string actualErrorMessage = LoginPage.GetErrorMessage();
            string expectedErrorMessage = _registry[ValidationKey.InvalidCredentialsMessage];
            Assert.Contains(expectedErrorMessage, actualErrorMessage, "Error message was not displayed for invalid credentials.");
        }
        #endregion
    }
}
