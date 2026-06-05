/**
 * Program:         BasePage.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-04
 * Description:     A class that represents the base page object model for the web application under test, 
 *                  providing common functionality and properties for all page objects.
 */

using OpenQA.Selenium.Support.UI;
using ReqnrollAutomation.Helpers;

namespace ReqnrollAutomation.Pages
{
    /// <summary>
    /// A class that represents the base page object model for the web application under test, 
    /// providing common functionality and properties for all page objects.
    /// </summary>
    internal class BasePage
    {
        #region Protected Attributes
        protected readonly IWebDriver _driver;
        protected readonly WebDriverWait _wait;
        #endregion

        #region Constructor
        public BasePage(IWebDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _wait = new(_driver, TimeSpan.FromSeconds(ConfigManager.DefaultTimeout));
        }
        #endregion
    }
}
