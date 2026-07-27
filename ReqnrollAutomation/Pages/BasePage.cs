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
        /// Navigates to the specified URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        public void Navigate(string url) => _driver.Navigate().GoToUrl(url);

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

        /// <summary>
        /// Switches back to the original tab in the browser, using the stored original window handle.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if no original window handle is stored.</exception>
        public void SwitchToOriginalTab()
        {
            if (_originalWindowHandle == null)
            {
                throw new InvalidOperationException("No original window handle stored. Cannot switch back to the original tab.");
            }

            _driver.SwitchTo().Window(_originalWindowHandle);
            _originalWindowHandle = null;
        }

        /// <summary>
        /// Closes the current tab and switches back to the original tab in the browser.
        /// </summary>
        public void CloseCurrentTabAndSwitchBackToOriginalTab()
        {
            _driver.Close();
            SwitchToOriginalTab();
        }

        /// <summary>
        /// Waits for the current URL to stabilize, meaning it remains the same for a short duration, indicating that the page has finished loading, navigating, or redirecting.
        /// </summary>
        /// <returns>The stabilized URL.</returns>
        public string WaitForUrlToStabilize()
        {
            string previousUrl = "";
            return _wait.Until(driver =>
            {
                string currentUrl = driver.Url;
                // If the current URL is the same as the previous URL, it means the page has stabilized, hence return the current URL
                // Otherwise, update the previous URL and continue waiting
                if (!string.IsNullOrEmpty(currentUrl) && currentUrl.Equals(previousUrl, StringComparison.OrdinalIgnoreCase))
                {
                    // Sleep for a short duration to ensure the page has fully stabilized
                    Thread.Sleep(500);
                    if (currentUrl == previousUrl)
                    {
                        return currentUrl;
                    }
                }

                previousUrl = currentUrl;
                return null;
            });
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
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({ behavior: 'instant', block: 'center' });", element);
        }
        #endregion
    }
}
