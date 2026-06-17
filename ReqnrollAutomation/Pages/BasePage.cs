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
    public abstract class BasePage
    {
        #region Protected Attributes
        protected readonly IWebDriver _driver;
        protected readonly WebDriverWait _wait;
        #endregion

        #region Public Properties
        public abstract string PageUrl { get; }
        #endregion

        #region Constructor
        public BasePage(IWebDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _wait = new(_driver, TimeSpan.FromSeconds(ConfigManager.DefaultTimeout));
        }
        #endregion

        #region Protected Helper Methods
        /// <summary>
        /// Scrolls the specified element into view using JavaScript, ensuring it is visible on the screen before interacting with it.
        /// </summary>
        /// <param name="element">The element to scroll into view.</param>
        protected void ScrollIntoView(IWebElement element)
        {
            ArgumentNullException.ThrowIfNull(element);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'center' });", element);
        }

        /// <summary>
        /// Waits for an element to be present in the DOM and visible on the page before locating it.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <returns>The located element.</returns>
        /// <exception cref="WebDriverTimeoutException">Throws if the element is not found or not visible within the timeout period.</exception>
        protected IWebElement WaitAndFindElement(By locator)
        {
            return _wait.Until(driver =>
            {
                var elements = driver.FindElements(locator);
                if (elements.Count > 0 && elements[0].Displayed)
                {
                    return elements[0];
                }
                return null;
            }) ?? throw new WebDriverTimeoutException($"Element with locator {locator} was not found or not visible within the timeout period.");
        }
        #endregion
    }
}
