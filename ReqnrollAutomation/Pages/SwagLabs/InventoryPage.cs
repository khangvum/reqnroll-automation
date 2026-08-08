/**
 * Program:         InventoryPage.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-14
 * Description:     A class that represents the inventory page of Swag Labs.
 */

using ReqnrollAutomation.Core.Extensions;

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
        /// <summary>
        /// Scrolls the page to the footer section.
        /// </summary>
        public void ScrollToFooter() => ScrollIntoView(FooterContainer);

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
        /// Clicks the social media link for the specified platform.
        /// </summary>
        /// <param name="platform">The social media platform (e.g., "Twitter", "Facebook", "LinkedIn").</param>
        public void ClickSocialMediaLink(string platform) => GetSocialMediaLink(platform).Click();

        /// <summary>
        /// Retrieves the text of the footer copyright element.
        /// </summary>
        /// <returns>The text of the copyright element.</returns>
        public string GetCopyrightText() => CopyrightText.Text;
        #endregion
    }
}
