/**
 * Program:         InventoryItemStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-08-12
 * Description:     A class that defines the step definitions for the inventory item feature on Swag Labs website.
 */

using ReqnrollAutomation.Core.Extensions;
using ReqnrollAutomation.Models.SwagLabs;

namespace ReqnrollAutomation.StepDefinitions.SwagLabs
{
    /// <summary>
    /// A class that defines the step definitions for the inventory item feature on Swag Labs website.
    /// </summary>
    [Binding]
    public class InventoryItemStepDefinitions : SwagLabsBaseStepDefinitions
    {
        #region Constructor
        public InventoryItemStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region When Steps
        [When(@"I add inventory items to the cart")]
        public void WhenIAddInventoryItemsToTheCart()
        {
            IReadOnlyList<InventoryItemDetails> addedItemsDetails = InventoryPage.AddRandomInventoryItems();
            _scenarioContext.SetValue(AddedInventoryItemsDetailsKey, addedItemsDetails);
        }

        [When(@"I navigate to the cart page")]
        public void WhenINavigateToTheCartPage()
        {
            InventoryPage.ClickCartLink();
        }
        #endregion

        #region Then Steps
        [Then(@"the cart badge on should reflect the number of items added")]
        public void ThenTheCartBadgeOnShouldReflectTheNumberOfItemsAdded()
        {
            // Retrieve the details of the added items from the scenario context
            IReadOnlyList<InventoryItemDetails> addedItemsDetails = _scenarioContext.GetValue<IReadOnlyList<InventoryItemDetails>>(AddedInventoryItemsDetailsKey);

            // Check if the cart badge reflects the correct number of items added
            int expectedItemCount = addedItemsDetails.Count;
            int actualItemCount = InventoryPage.GetCartBadgeItemCount();
            Assert.AreEqual(expectedItemCount, actualItemCount, $"Expected cart badge count to be {expectedItemCount}, but found {actualItemCount}.");
        }

        [Then(@"the cart should display the correct number of items added")]
        public void ThenTheCartShouldDisplayTheCorrectNumberOfItemsAdded()
        {
            // Retrieve the details of the added items from the scenario context
            IReadOnlyList<InventoryItemDetails> addedItemsDetails = _scenarioContext.GetValue<IReadOnlyList<InventoryItemDetails>>(AddedInventoryItemsDetailsKey);

            // Check if the number of items in the cart matches the number of items added
            int expectedItemCount = addedItemsDetails.Count;
            int actualItemCount = CartPage.GetCartItemsDetails().Count;
            Assert.AreEqual(expectedItemCount, actualItemCount, $"Expected {expectedItemCount} items in the cart, but found {actualItemCount}.");
        }

        [Then(@"the items' details and pricing in the cart should match the product page")]
        public void ThenTheItemsDetailsAndPricingInTheCartShouldMatchTheProductPage()
        {
            // Retrieve the details of the added items from the scenario context
            IReadOnlyList<InventoryItemDetails> addedItemsDetails = _scenarioContext.GetValue<IReadOnlyList<InventoryItemDetails>>(AddedInventoryItemsDetailsKey);
            IReadOnlyList<InventoryItemDetails> cartItemsDetails = CartPage.GetCartItemsDetails();

            // Check if the details of each item in the cart match the details of the items added
            for (int i = 0; i < addedItemsDetails.Count; i++)
            {
                InventoryItemDetails addedItem = addedItemsDetails[i];
                InventoryItemDetails cartItem = cartItemsDetails[i];
                Assert.AreEqual(addedItem.Name, cartItem.Name, $"Name for item at index {i} does not match.");
                Assert.AreEqual(addedItem.Description, cartItem.Description, $"Description for item '{addedItem.Name}' does not match.");
                Assert.AreEqual(addedItem.Price, cartItem.Price, $"Price for item '{addedItem.Name}' does not match.");
            }
        }
        #endregion
    }
}
