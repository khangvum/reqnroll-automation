/**
 * Program:         InventoryPage.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-14
 * Description:     A class that represents the inventory page of Swag Labs.
 */

namespace ReqnrollAutomation.Pages
{
    /// <summary>
    /// A class that represents the inventory page of Swag Labs.
    /// </summary>
    public class InventoryPage : BasePage
    {
        #region Public Properties
        public override string PageUrl => "https://www.saucedemo.com/inventory.html";
        #endregion

        #region Page Locators
        // Footer
        private readonly By _footerContainerLocator = By.CssSelector("footer.footer");
        private readonly By _twitterLinkLocator = By.CssSelector("ul.social li.social_twitter a");
        private readonly By _facebookLinkLocator = By.CssSelector("ul.social li.social_facebook a");
        private readonly By _linkedInLinkLocator = By.CssSelector("ul.social li.social_linkedin a");
        private readonly By _copyrightTextLocator = By.CssSelector("footer.footer .footer_copy");
        #endregion

        #region Page Elements
        private IWebElement FooterContainer => WaitAndFindElement(_footerContainerLocator);
        private IWebElement TwitterLink => WaitAndFindElement(_twitterLinkLocator);
        private IWebElement FacebookLink => WaitAndFindElement(_facebookLinkLocator);
        private IWebElement LinkedInLink => WaitAndFindElement(_linkedInLinkLocator);
        private IWebElement CopyrightText => WaitAndFindElement(_copyrightTextLocator);
        #endregion

        #region Constructor
        public InventoryPage(IWebDriver driver) : base(driver)
        {
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
        /// Retrieves the text of the footer copyright element.
        /// </summary>
        /// <returns>The text of the copyright element.</returns>
        public string GetCopyrightText() => CopyrightText.Text;

        /// <summary>
        /// Checks if the social media link for the specified platform is visible on the page.
        /// </summary>
        /// <param name="platform">The social media platform (e.g., "Twitter", "Facebook", "LinkedIn").</param>
        /// <returns>True if the link is visible, false otherwise.</returns>
        public bool IsSocialMediaLinkVisible(string platform) => GetSocialMediaLink(platform).Displayed;

        /// <summary>
        /// Clicks the social media link for the specified platform.
        /// </summary>
        /// <param name="platform">The social media platform (e.g., "Twitter", "Facebook", "LinkedIn").</param>
        public void ClickSocialMediaLink(string platform) => GetSocialMediaLink(platform).Click();
        #endregion

        #region Private Helper Methods
        /// <summary>
        /// Gets the social media link element based on the specified platform.
        /// </summary>
        /// <param name="platform">The social media platform (e.g., "Twitter", "Facebook", "LinkedIn").</param>
        /// <returns>The social media link element.</returns>
        /// <exception cref="ArgumentException">Throws if the platform is not supported.</exception>
        private IWebElement GetSocialMediaLink(string platform)
        {
            return platform.ToLower() switch
            {
                "twitter" => TwitterLink,
                "facebook" => FacebookLink,
                "linkedin" => LinkedInLink,
                _ => throw new ArgumentException($"Unsupported social media platform: {platform}")
            };
        }
        #endregion
    }
}
