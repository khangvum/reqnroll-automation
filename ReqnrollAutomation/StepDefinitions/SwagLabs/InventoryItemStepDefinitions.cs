

using AventStack.ExtentReports.Gherkin.Model;

/**
 * Program:         InventoryItemStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-08-12
 * Description:     A class that defines the step definitions for the inventory item feature on Swag Labs website.
 */

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
            IReadOnlyList<(string Name, string Description, decimal Price)> addedItemsDetails = InventoryPage.AddRandomInventoryItems();
            _scenarioContext[AddedInventoryItemsDetailsKey] = addedItemsDetails;
        }

        [When(@"I navigate to the cart page")]
        public void WhenINavigateToTheCartPage()
        {
            InventoryPage.ClickCartLink();
        }
        #endregion

        #region Then Steps
        [Then(@"the cart should display the correct number of items added")]
        public void ThenTheCartShouldDisplayTheCorrectNumberOfItemsAdded()
        {
            // Retrieve the details of the added items from the scenario context
            IReadOnlyList<(string Name, string Description, decimal Price)> addedItemsDetails = _scenarioContext[AddedInventoryItemsDetailsKey] as IReadOnlyList<(string Name, string Description, decimal Price)> 
                ?? throw new InvalidOperationException("Added inventory items details not found in the scenario context.");

            // Check if the number of items in the cart matches the number of items added
            int expectedItemCount = addedItemsDetails.Count;
            int actualItemCount = CartPage.CartItems.Count;
            Assert.AreEqual(expectedItemCount, actualItemCount, $"Expected {expectedItemCount} items in the cart, but found {actualItemCount}.");
        }
        #endregion
    }
}
