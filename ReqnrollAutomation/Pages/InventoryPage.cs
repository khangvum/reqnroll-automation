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
        #endregion
    }
}
