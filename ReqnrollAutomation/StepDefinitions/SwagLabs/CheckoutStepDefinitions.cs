

using ReqnrollAutomation.Pages.SwagLabs;

/**
 * Program:         CheckoutStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-08-20
 * Description:     A class that defines the step definitions for the checkout feature on Swag Labs website.
 */
namespace ReqnrollAutomation.StepDefinitions.SwagLabs
{
    /// <summary>
    ///  A class that defines the step definitions for the checkout feature on Swag Labs website.
    /// </summary>
    [Binding]
    public class CheckoutStepDefinitions : SwagLabsBaseStepDefinitions
    {
        #region Constructor
        public CheckoutStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region When Steps
        [When(@"I proceed to the checkout information page")]
        public void WhenIProceedToTheCheckoutInformationPage()
        {
            CartPage.ClickCheckoutButton();
        }

        [When(@"I provide valid checkout details")]
        public void WhenIProvideValidCheckoutDetails()
        {
            CheckoutPage.EnterRandomCheckoutInformation();
        }

        [When(@"I click the continue button")]
        public void WhenIClickTheContinueButton()
        {
            CheckoutPage.ClickContinue();
        }

        [When(@"I finish the checkout overview")]
        public void WhenIFinishTheCheckoutOverview()
        {
            CheckoutPage.ClickFinish();
        }
        #endregion

        #region Then Steps
        [Then(@"the checkout completion page should display a successful order confirmation message")]
        public void ThenTheCheckoutCompletionPageShouldDisplayASuccessfulOrderConfirmationMessage()
        {
            // Get the actual order confirmation header and message from the page
            string actualConfirmationHeader = CheckoutPage.GetOrderConfirmationHeader();
            string actualConfirmationMessage = CheckoutPage.GetOrderConfirmationMessage();

            // Get the expected order confirmation header and message from the registry
            string expectedConfirmationHeader = CheckoutPage.Registry[CheckoutPage.ValidationKey.OrderConfirmationHeader];
            string expectedConfirmationMessage = CheckoutPage.Registry[CheckoutPage.ValidationKey.OrderConfirmationMessage];

            // Verify that the actual confirmation header and message match the expected values
            Assert.AreEqual(expectedConfirmationHeader, actualConfirmationHeader, $"The order confirmation header '{actualConfirmationHeader}' does not match the expected value {expectedConfirmationHeader}.");
            Assert.AreEqual(expectedConfirmationMessage, actualConfirmationMessage, $"The order confirmation message '{actualConfirmationMessage}' does not match the expected value {expectedConfirmationMessage}.");
        }
        #endregion
    }
}
