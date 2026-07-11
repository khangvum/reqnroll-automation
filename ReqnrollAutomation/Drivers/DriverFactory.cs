/**
* Program:         DriverFactory.cs
* Author:          Manh Khang Vu
* Date:            2026-05-07
* Description:     A class that provides a factory for creating and managing WebDriver instances for the test automation framework, streamlining parallel test execution and ensuring proper resource management.
*/

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using ReqnrollAutomation.Config;
using ReqnrollAutomation.Core.Config;

namespace ReqnrollAutomation.Drivers
{
    /// <summary>
    /// A class that provides a factory for creating and managing WebDriver instances for the test 
    /// automation framework, streamlining parallel test execution and ensuring proper resource management.
    /// </summary>
    internal class DriverFactory
    {
        #region Public Methods
        /// <summary>
        /// Creates a new instance of the Selenium WebDriver.
        /// </summary>
        /// <param name="browserType">The type of browser to create the driver for.</param>
        /// <returns>The IWebDriver instance.</returns>
        public static IWebDriver CreateDriver()
        {
            // Get the browser type from the configuration file
            BrowserType browserType = ConfigManager.Browser;

            // Create the appropriate options based on the specified browser type
            ChromiumOptions options = browserType switch
            {
                BrowserType.Chrome => new ChromeOptions(),
                BrowserType.Edge => new EdgeOptions(),
                _ => throw new ArgumentException(nameof(browserType), $"Unsupported browser type: {browserType}")
            };

            // Allow running in headless mode by setting env var HEADLESS=1
            bool isHeadless = IsHeadless();
            if (isHeadless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");
            }
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage");

            // Disable extensions and automation banners for cleaner test runs
            DisableExtensionsAndBanners(options);

            IWebDriver driver = browserType switch
            {
                BrowserType.Chrome => new ChromeDriver((ChromeOptions)options),
                BrowserType.Edge => new EdgeDriver((EdgeOptions)options),
                _ => throw new ArgumentException(nameof(browserType), $"Unsupported browser type: {browserType}")
            };

            try
            {
                // Only attempt to maximize if not running headless
                if (!isHeadless)
                {
                    driver.Manage().Window.Maximize();
                }

            }
            catch { }

            return driver;
        }

        /// <summary>
        /// Clears all cookies and cache from the current WebDriver instance to ensure a clean state for testing.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        public static void ClearCookiesAndCache(IWebDriver? driver)
        {
            if (driver != null)
            {
                try
                {
                    driver.Manage().Cookies.DeleteAllCookies();
                }
                catch { }
            }
        }

        /// <summary>
        /// Closes and disposes the current WebDriver instance, releasing all associated 
        /// resources and terminating any orphaned browser driver processes.
        /// </summary>
        public static void QuitDriver(IWebDriver? driver)
        {
            if (driver != null)
            {
                try
                {
                    // Try normal quit/close first
                    try { driver.Quit(); } catch { }
                    try { driver.Dispose(); } catch { }
                }
                catch { }
            }
        }
        #endregion

        #region Private Helper Methods
        /// <summary>
        /// Disables browser extensions, popups, infobars, and password management features in Chrome to ensure a clean testing environment.
        /// </summary>
        /// <param name="options">The Chromium options instance.</param>
        private static void DisableExtensionsAndBanners(ChromiumOptions options)
        {
            // - Disable the Password Generation and Manager UI
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);

            // - Add the explicit key to turn off Data Breach scanning
            options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

            // - Disable Safe Browsing password protection/leak detection features
            options.AddUserProfilePreference("safebrowsing.password_protection_warning_trigger", 0);
            options.AddArgument("--disable-features=PasswordLeakDetection");
            options.AddArgument("--disable-features=SafeBrowsingPasswordProtection");

            // - Disable popups and infobars
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--disable-infobars");
        }
        #endregion
    }
}