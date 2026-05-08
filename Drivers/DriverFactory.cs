/**
 * Program:         DriverFactory.cs
 * Author:          Manh Khang Vu
 * Date:            May 07, 2026
 * Description:     A class that provides a singleton instance of the Selenium WebDriver used for automated testing.
 */

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;

namespace ReqnrollProject2.Drivers
{
    /// <summary>
    /// A class that provides a singleton instance of the Selenium WebDriver used for automated testing.
    /// </summary>
    public class DriverFactory
    {
        // Private attributes
        private static IWebDriver? _driver;

        // Public methods
        /// <summary>
        /// Gets a singleton instance of the Selenium WebDriver.
        /// </summary>
        /// <returns>The singleton IWebDriver instance.</returns>
        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                ChromeOptions options = new();
                // Allow running in headless mode by setting env var HEADLESS=1
                if (Environment.GetEnvironmentVariable("HEADLESS") == "1")
                {
                    options.AddArgument("--headless=new");
                }
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--disable-dev-shm-usage");

                // Do not provide a local chromedriver path - let Selenium Manager resolve the matching driver.
                _driver = new ChromeDriver(options);
                try
                {
                    _driver.Manage().Window.Maximize();
                }
                catch
                {
                    // Some environments (headless/remote) may not support window operations.
                }
            }

            return _driver!;
        }

        /// <summary>
        /// Clears all cookies and cache from the current WebDriver instance to ensure a clean state for testing.
        /// </summary>
        public static void ClearCookiesAndCache()
        {
            if (_driver != null)
            {
                try
                {
                    _driver.Manage().Cookies.DeleteAllCookies();
                }
                catch {}
            }
        }

        /// <summary>
        /// Closes and disposes the current WebDriver instance, releasing all associated resources and terminating any
        /// orphaned browser driver processes.
        /// </summary>
        public static void QuitDriver()
        {
            if (_driver != null)
            {
                try
                {
                    // Try normal quit/close first
                    try { _driver.Quit(); } catch { }
                    try { _driver.Dispose(); } catch { }
                }
                catch
                {
                    // Ignore
                }

                // Final fallback: Kill any orphan chromedriver processes to avoid zombie browsers
                try
                {
                    Process[] processes = Process.GetProcessesByName("chromedriver");
                    foreach (Process p in processes)
                    {
                        try { p.Kill(); } catch { }
                    }
                }
                catch { }
                _driver = null;
            }
        }
    }
}
