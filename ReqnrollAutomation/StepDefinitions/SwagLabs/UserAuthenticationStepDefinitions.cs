/**
 * Program:         UserAuthenticationStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-10
 * Description:     A class that defines the step definitions for the user authentication feature on Swag Labs website.
 */

using AventStack.ExtentReports.Gherkin.Model;
using ReqnrollAutomation.Helpers;
using ReqnrollAutomation.Pages.SwagLabs;

namespace ReqnrollAutomation.StepDefinitions.SwagLabs
{
    /// <summary>
    /// A class that defines the step definitions for the user authentication feature on Swag Labs website.
    /// </summary>
    [Binding]
    public class UserAuthenticationStepDefinitions : SwagLabsBaseStepDefinitions
    {
        #region Constructor
        public UserAuthenticationStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region Given Steps
        [Given(@"I am on the Swag Labs login page")]
        public void GivenIAmOnTheSwagLabsLoginPage()
        {
            LoginPage.Navigate();
        }
        #endregion

        #region When Steps
        [Given(@"I am logged in as {} user")]
        [When(@"I log in as {} user")]
        public void WhenILogInAsUser(string accountType)
        {
            string accountKey = CredentialManager.NormalizeAccountType(accountType);
            LoginPage.LoginAsRole(accountKey);
        }
        #endregion

        #region Then Steps
        [Then(@"I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            Assert.Contains(LoginPage.Registry[LoginPage.ValidationKey.SuccessfulLoginUrl], Driver.Url, "User was not logged in successfully.");
        }   

        [Then(@"I should see a\(n\) {} message")]
        public void ThenIShouldSeeAMessage(string errorType)
        {
            // Remove spaces and append "message", for example "invalid credentials" > "invalidcredentialsmessage"
            string lookupKey = $"{errorType.Replace(" ", "")}message";

            // Try to parse the lookup key to the ValidationKey enum
            if (!Enum.TryParse(lookupKey, true, out LoginPage.ValidationKey registryKey))
            {
                throw new ArgumentException($"Could not map '{errorType}' to an existing registry key.");
            }

            string actualErrorMessage = LoginPage.GetErrorMessage();
            string expectedErrorMessage = LoginPage.Registry[registryKey];
            Assert.Contains(expectedErrorMessage, actualErrorMessage, "Lockout message was not displayed.");
        }
        #endregion
    }
}
