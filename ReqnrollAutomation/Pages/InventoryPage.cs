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
        #region Private Attributes
        private const string PageUrl = "https://www.saucedemo.com/inventory.html";
        #endregion

        #region Page Locators
        // Footer
        private readonly By _footerContainerLocator = By.CssSelector("footer.footer");
        private readonly By _twitterLinkLocator = By.CssSelector("ul.social li.social_twitter a");
        private readonly By _facebookLinkLocator = By.CssSelector("ul.social li.social_facebook a");
        private readonly By _linkedInLinkLocator = By.CssSelector("ul.social li.social_linkedin a");
        #endregion

        #region Page Elements
        private IWebElement FooterContainer => WaitAndFindElement(_footerContainerLocator);
        private IWebElement TwitterLink => WaitAndFindElement(_twitterLinkLocator);
        private IWebElement FacebookLink => WaitAndFindElement(_facebookLinkLocator);
        private IWebElement LinkedInLink => WaitAndFindElement(_linkedInLinkLocator);
        #endregion

        #region Constructor
        public InventoryPage(IWebDriver driver) : base(driver)
        {
        }
        #endregion
    }
}
