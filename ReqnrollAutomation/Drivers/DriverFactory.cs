/**
 * Program:         DriverFactory.cs
 * Author:          Manh Khang Vu
 * Date:            2026-05-07
 * Description:     A class that provides a factory for creating and managing WebDriver instances for the test automation framework, streamlining parallel test execution and ensuring proper resource management.
 */

using OpenQA.Selenium.Chrome;
using System.Diagnostics;

namespace ReqnrollAutomation.Drivers
{
    /// <summary>
    /// A class that provides a factory for creating and managing WebDriver instances for the test 
    /// automation framework, streamlining parallel test execution and ensuring proper resource management.
    /// </summary>
    internal class DriverFactory
    {
        /// <summary>
        /// Creates a new instance of the Selenium WebDriver.
        /// </summary>
        /// <returns>The IWebDriver instance.</returns>
        public static IWebDriver CreateDriver()
        {
            ChromeOptions options = new();
            // Allow running in headless mode by setting env var HEADLESS=1
            bool isHeadless = Environment.GetEnvironmentVariable("HEADLESS") == "1" ||
                              Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true" ||
                              Environment.GetEnvironmentVariable("CI") == "true";
            if (isHeadless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");
            }
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-dev-shm-usage");

            IWebDriver driver = new ChromeDriver(options);

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
            }
        }
    }
}
