/**
 * Program:         BasePage.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-04
 * Description:     A class that represents the base page object model for the web application under test, 
 *                  providing common functionality and properties for all page objects.
 */

using OpenQA.Selenium.Support.UI;
using ReqnrollAutomation.Config;

namespace ReqnrollAutomation.Pages
{
    /// <summary>
    /// A class that represents the base page object model for the web application under test, 
    /// providing common functionality and properties for all page objects.
    /// </summary>
    public abstract class BasePage
    {
        #region Private Attributes
        private string? _originalWindowHandle;
        #endregion

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

        #region Public Methods
        /// <summary>
        /// Navigates to the page's URL.
        /// </summary>
        /// <remarks>
        /// This method navigates to the URL specified by the <see cref="PageUrl"/> 
        /// property, which must be overridden in derived classes.
        /// </remarks>
        public void Navigate() => _driver.Navigate().GoToUrl(PageUrl);

        /// <summary>
        /// Switches to the newly opened tab in the browser, storing the original window handle for later use.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if the new window handle is not found.</exception>
        public void SwitchToNewTab()
        {
            _originalWindowHandle = _driver.CurrentWindowHandle;

            // Wait for the new tab to open
            _wait.Until(driver => driver.WindowHandles.Count > 1);

            // Switch to the new tab
            string? newWindowHandle = _driver.WindowHandles.FirstOrDefault(handle => handle != _originalWindowHandle);
            _driver.SwitchTo().Window(newWindowHandle ?? throw new InvalidOperationException("New window handle not found."));
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
        #endregion
    }
}
