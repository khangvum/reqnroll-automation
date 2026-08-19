/**
 * Program:         InventoryPage.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-14
 * Description:     A class that represents the inventory page of Swag Labs.
 */

using ReqnrollAutomation.Core.Extensions;
using ReqnrollAutomation.Models.SwagLabs;

namespace ReqnrollAutomation.Pages.SwagLabs
{
    /// <summary>
    /// A class that represents the inventory page of Swag Labs.
    /// </summary>
    public class InventoryPage : BasePage
    {
        #region Public Properties
        // URL
        public override string PageUrl => "https://www.saucedemo.com/inventory.html";

        // Registry
        public enum ValidationKey
        {
            CopyrightPattern
        }

        public readonly Dictionary<ValidationKey, string> Registry;

        // Constants
        // - Footer social media links
        public readonly IReadOnlyList<(string Platform, string ExpectedUrl)> SocialMediaLinks =
        [
            ("Twitter", "https://x.com/saucelabs"),
            ("Facebook", "https://www.facebook.com/saucelabs"),
            ("LinkedIn", "https://www.linkedin.com/company/sauce-labs/")
        ];
        #endregion

        #region Page Locators
        // Footer locators
        private readonly By _footerContainerLocator = By.CssSelector("footer.footer");
        private readonly By _socialMediaSectionLocator = By.CssSelector("ul.social");
        private readonly By _copyrightTextLocator = By.CssSelector("footer.footer .footer_copy");

        /// <summary>
        /// Gets the locator for a social media link based on the platform name.
        /// </summary>
        /// <param name="platform">The social media platform (e.g., "Twitter", "Facebook", "LinkedIn").</param>
        /// <returns>The locator for the social media link.</returns>
        private By GetSocialMediaLinkLocator(string platform) => By.CssSelector($"ul.social li.social_{platform.ToLower()} a");

        // Inventory List locators
        private readonly By _inventoryListLocator = By.CssSelector("div.inventory_list[data-test='inventory-list']");
        private readonly By _inventoryItemLocator = By.CssSelector("div.inventory_item[data-test='inventory-item']");

        // Inventory Item locators (relative to each inventory item)
        private readonly By _itemNameLocator = By.CssSelector(".inventory_item_name");
        private readonly By _itemDescLocator = By.CssSelector(".inventory_item_desc");
        private readonly By _itemPriceLocator = By.CssSelector(".inventory_item_price");
        private readonly By _itemAddToCartButtonLocator = By.CssSelector("button.btn_inventory");

        // Cart Link locator
        private readonly By _cartLinkLocator = By.CssSelector("a.shopping_cart_link");
        private readonly By _cartBadgeLocator = By.CssSelector("span.shopping_cart_badge");
        #endregion

        #region Page Elements
        // Footer elements
        private IWebElement FooterContainer => _driver.WaitAndFindElement(_footerContainerLocator);
        private IWebElement SocialMediaSection => _driver.WaitAndFindElement(_socialMediaSectionLocator);
        private IWebElement CopyrightText => _driver.WaitAndFindElement(_copyrightTextLocator);

        /// <summary>
        /// Gets the social media link element for the specified platform.
        /// </summary>
        /// <param name="platform">The social media platform (e.g., "Twitter", "Facebook", "LinkedIn").</param>
        /// <returns>The social media link element.</returns>
        private IWebElement GetSocialMediaLink(string platform) => _driver.WaitAndFindElement(GetSocialMediaLinkLocator(platform));

        // Inventory List elements
        private IReadOnlyList<IWebElement> InventoryItems => _driver.FindElements(_inventoryItemLocator);

        // Cart Link element
        public IWebElement CartLink => _driver.WaitAndFindElement(_cartLinkLocator);
        public IWebElement CartBadge => _driver.WaitAndFindElement(_cartBadgeLocator);
        #endregion

        #region Constructor
        public InventoryPage(IWebDriver driver) : base(driver)
        {
            Registry = new()
            {
                { ValidationKey.CopyrightPattern, @"© \d{4} Sauce Labs\. All Rights Reserved\." }
            };
        }
        #endregion

        #region Public Methods
        public void ClickCartLink() => CartLink.Click();

        /// <summary>
        /// Clicks the social media link for the specified platform.
        /// </summary>
        /// <param name="platform">The social media platform (e.g., "Twitter", "Facebook", "LinkedIn").</param>
        public void ClickSocialMediaLink(string platform) => GetSocialMediaLink(platform).Click();

        /// <summary>
        /// Retrieves the text of the footer copyright element.
        /// </summary>
        /// <returns>The text of the copyright element.</returns>
        public string GetCopyrightText() => CopyrightText.Text;

        /// <summary>
        /// Retrieves the number of items displayed in the cart badge.
        /// </summary>
        /// <returns>The number of items in the cart.</returns>
        public int GetCartBadgeItemCount()
        {
            try
            {
                return int.Parse(CartBadge.Text);
            }
            catch (NoSuchElementException)
            {
                throw new InvalidOperationException("Cart badge is not present on the page.");
            }
        }

        /// <summary>
        /// Checks if the footer copyright text is visible on the page.
        /// </summary>
        /// <returns>True if the copyright text is visible, false otherwise.</returns>
        public bool IsCopyrightTextVisible() => CopyrightText.Displayed;

        /// <summary>
        /// Checks if the social media section in the footer is visible on the page.
        /// </summary>
        /// <returns>True if the section is visible, false otherwise.</returns>
        public bool IsSocialMediaSectionVisible() => SocialMediaSection.Displayed;

        /// <summary>
        /// Scrolls the page to the footer section.
        /// </summary>
        public void ScrollToFooter() => ScrollIntoView(FooterContainer);

        /// <summary>
        /// Adds a random number of inventory items to the cart and returns their details.
        /// </summary>
        /// <returns>A list of InventoryItemDetails objects containing the name, description, and price of each added item.</returns>
        /// <exception cref="InvalidOperationException">Throws if no inventory items are found on the page.</exception>
        public IReadOnlyList<InventoryItemDetails> AddRandomInventoryItems()
        {
            // Check if there are any inventory items available
            if (InventoryItems.Count == 0)
                throw new InvalidOperationException("No inventory items found on the page.");

            // Determine a random number of items to add
            int itemsToAdd = Random.Shared.Next(1, InventoryItems.Count + 1);

            // Randomly select unique inventory items to add
            List<IWebElement> selectedItems = [.. InventoryItems.OrderBy(_ => Random.Shared.Next()).Take(itemsToAdd)];

            List<InventoryItemDetails> addedItemsDetails = [];
            foreach (IWebElement item in selectedItems)
            {
                // Get the details of each selected item
                string name = item.FindElement(_itemNameLocator).Text;
                string description = item.FindElement(_itemDescLocator).Text;
                string priceText = item.FindElement(_itemPriceLocator).Text.Replace("$", "");
                decimal price = decimal.Parse(priceText);

                // Click the "Add to cart" button for the item
                item.FindElement(_itemAddToCartButtonLocator).Click();

                // Store the details of the added item
                addedItemsDetails.Add(new InventoryItemDetails
                {
                    Name = name,
                    Description = description,
                    Price = price,
                });
            }

            return addedItemsDetails;
        }
        #endregion
    }
}
