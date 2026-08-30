/**
 * Program:         CheckoutStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-08-20
 * Description:     A class that defines the step definitions for the checkout feature on Swag Labs website.
 */

using ReqnrollAutomation.Core.Extensions;
using ReqnrollAutomation.Models.SwagLabs;
using ReqnrollAutomation.Pages.SwagLabs;

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

        [Then(@"the checkout overview should display the correct number of items added")]
        public void ThenTheCheckoutOverviewShouldDisplayTheCorrectNumberOfItemsAdded()
        {
            // Retrieve the details of the added items from the scenario context
            IReadOnlyList<InventoryItemDetails> addedItemsDetails = _scenarioContext.GetValue<IReadOnlyList<InventoryItemDetails>>(AddedInventoryItemsDetailsKey);

            // Check if the number of items in the cart matches the number of items added
            int expectedItemCount = addedItemsDetails.Count;
            int actualItemCount = CheckoutPage.GetCheckoutOverviewItemsDetails().Count;
            Assert.AreEqual(expectedItemCount, actualItemCount, $"Expected {expectedItemCount} items in the checkout overview, but found {actualItemCount}.");
        }

        [Then(@"the checkout overview should calculate the correct subtotal, tax, and total price")]
        public void ThenTheCheckoutOverviewShouldCalculateTheCorrectSubtotalTaxAndTotalPrice()
        {
            // Retrieve the details of the added items from the scenario context
            IReadOnlyList<InventoryItemDetails> addedItemsDetails = _scenarioContext.GetValue<IReadOnlyList<InventoryItemDetails>>(AddedInventoryItemsDetailsKey);

            // Calculate the expected subtotal and compare it with the actual subtotal displayed on the checkout overview page
            decimal expectedSubtotal = addedItemsDetails.Sum(item => item.Price);
            decimal actualSubtotal = CheckoutPage.GetSubtotal();
            Assert.AreEqual(expectedSubtotal, actualSubtotal, $"Expected subtotal: {expectedSubtotal}, but found: {actualSubtotal}.");

            // Calculate the expected tax and compare it with the actual tax displayed on the checkout overview page
            decimal expectedTax = Math.Round(expectedSubtotal * CheckoutPage.TaxRate, 2);
            decimal actualTax = CheckoutPage.GetTax();
            Assert.AreEqual(expectedTax, actualTax, $"Expected tax: {expectedTax}, but found: {actualTax}.");

            // Calculate the expected total and compare it with the actual total displayed on the checkout overview page
            decimal expectedTotal = Math.Round(expectedSubtotal + expectedTax, 2);
            decimal actualTotal = CheckoutPage.GetTotal();
            Assert.AreEqual(expectedTotal, actualTotal, $"Expected total: {expectedTotal}, but found: {actualTotal}.");
        }

        [Then(@"the items' details and pricing in the checkout overview should match the product page")]
        public void ThenTheItemsDetailsAndPricingInTheCheckoutOverviewShouldMatchTheProductPage()
        {
            // Retrieve the details of the added items from the scenario context
            IReadOnlyList<InventoryItemDetails> addedItemsDetails = _scenarioContext.GetValue<IReadOnlyList<InventoryItemDetails>>(AddedInventoryItemsDetailsKey);
            IReadOnlyList<InventoryItemDetails> checkoutItemsDetails = CheckoutPage.GetCheckoutOverviewItemsDetails();

            // Check if the details of each item in the cart match the details of the items added
            for (int i = 0; i < addedItemsDetails.Count; i++)
            {
                InventoryItemDetails addedItem = addedItemsDetails[i];
                InventoryItemDetails checkoutItem = checkoutItemsDetails[i];
                Assert.AreEqual(addedItem.Name, checkoutItem.Name, $"Name for item at index {i} does not match.");
                Assert.AreEqual(addedItem.Description, checkoutItem.Description, $"Description for item '{addedItem.Name}' does not match.");
                Assert.AreEqual(addedItem.Price, checkoutItem.Price, $"Price for item '{addedItem.Name}' does not match.");
            }
        }
        #endregion
    }
}
