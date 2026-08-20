/**
 * Program:         CartPage.cs
 * Author:          Manh Khang Vu
 * Date:            2026-08-18
 * Description:     A class that represents the cart page of Swag Labs.
 */

using ReqnrollAutomation.Models.SwagLabs;

namespace ReqnrollAutomation.Pages.SwagLabs
{
    /// <summary>
    /// A class that represents the cart page of Swag Labs.
    /// </summary>
    public class CartPage : BasePage
    {
        #region Public Properties
        // URL
        public override string PageUrl => "https://www.saucedemo.com/cart.html";
        #endregion

        #region Page Locators
        // Cart List locators
        private readonly By _cartItemLocator = By.CssSelector("div.cart_item");

        // Cart Item locators (relative to each cart item)
        private readonly By _itemNameLocator = By.CssSelector(".inventory_item_name");
        private readonly By _itemDescLocator = By.CssSelector(".inventory_item_desc");
        private readonly By _itemPriceLocator = By.CssSelector(".inventory_item_price");

        // Checkout Button locators
        private readonly By _checkoutButtonLocator = By.CssSelector("button#checkout");
        #endregion

        #region Page Elements
        // Cart List elements
        private IReadOnlyList<IWebElement> CartItems => _driver.FindElements(_cartItemLocator);

        // Checkout Button elements
        private IWebElement CheckoutButton => _driver.FindElement(_checkoutButtonLocator);
        #endregion

        #region Constructor
        public CartPage(IWebDriver driver) : base(driver)
        {
        }
        #endregion

        #region Page Methods
        /// <summary>
        /// Clicks the checkout button on the cart page to proceed to the checkout information page.
        /// </summary>
        public void ClickCheckoutButton() => CheckoutButton.Click();

        /// <summary>
        /// Gets the details of all items in the cart, including their name, description, and price.
        /// </summary>
        /// <returns>A list of InventoryItemDetails objects containing the name, description, and price of each item in the cart.</returns>
        public IReadOnlyList<InventoryItemDetails> GetCartItemsDetails()
        {
            List<InventoryItemDetails> cartItemsDetails = [];
            foreach (IWebElement cartItem in CartItems)
            {
                // Get the details of each item
                string name = cartItem.FindElement(_itemNameLocator).Text;
                string description = cartItem.FindElement(_itemDescLocator).Text;
                string priceText = cartItem.FindElement(_itemPriceLocator).Text.Replace("$", "");
                decimal price = decimal.Parse(priceText);

                // Store the details of the item
                cartItemsDetails.Add(new InventoryItemDetails
                {
                    Name = name,
                    Description = description,
                    Price = price,
                });
            }

            return cartItemsDetails;
        }
        #endregion
    }
}
