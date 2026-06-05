using AventStack.ExtentReports.Gherkin.Model;

namespace ReqnrollAutomation.StepDefinitions
{
    [Binding]
    public class SwagLabsLoginStepDefinitions
    {
        #region Private Attributes
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        #endregion

        #region Constructor
        public SwagLabsLoginStepDefinitions(ScenarioContext scenarioContext)
        {
            _driver = scenarioContext.Get<IWebDriver>("WebDriver");
            _scenarioContext = scenarioContext;
        }
        #endregion

        #region Step Definitions
        [Given(@"I am on the Swag Labs login page")]
        public void GivenIAmOnTheSwagLabsLoginPage()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [When(@"I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            IWebElement usernameInput = _driver.FindElement(By.Id("user-name"));
            IWebElement passwordInput = _driver.FindElement(By.Id("password"));
            IWebElement loginButton = _driver.FindElement(By.Id("login-button"));

            usernameInput.SendKeys("standard_use");
            passwordInput.SendKeys("secret_sauce");
            loginButton.Click();
        }

        [When(@"I enter invalid credentials")]
        public void WhenIEnterInvalidCredentials()
        {
            IWebElement usernameInput = _driver.FindElement(By.Id("user-name"));
            IWebElement passwordInput = _driver.FindElement(By.Id("password"));
            IWebElement loginButton = _driver.FindElement(By.Id("login-button"));

            usernameInput.SendKeys("locked_out_user");
            passwordInput.SendKeys("secret_sauce");
            loginButton.Click();
        }

        [Then(@"I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            Assert.IsTrue(_driver.Url.Contains("https://www.saucedemo.com/inventory.html"), "User was not logged in successfully.");
        }

        [Then(@"I should see an error message")]
        public void ThenIShouldSeeAnErrorMessage()
        {
            IWebElement errorMessage = _driver.FindElement(By.CssSelector(".error-message-container"));
            Assert.IsTrue(errorMessage.Displayed, "Error message was not displayed.");
        }
        #endregion
    }
}
