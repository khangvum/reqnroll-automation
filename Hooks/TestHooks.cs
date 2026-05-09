/**
 * Program:         TestHooks.cs
 * Author:          Manh Khang Vu
 * Date:            May 07, 2026
 * Description:     A class that provides hooks for setting up and tearing down test environments using Selenium WebDriver.
 */

using OpenQA.Selenium;
using Reqnroll;
using reqnroll_automation.Drivers;

namespace reqnroll_automation.Hooks
{
    /// <summary>
    /// A class that provides hooks for setting up and tearing down test environments using Selenium WebDriver.
    /// </summary>
    [Binding]
    internal class TestHooks
    {
        /// <summary>
        /// Runs before the entire test run to perform any global setup.
        /// </summary>
        #region Test Run Hooks
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Any global setup can be done here, such as initializing logging or configuration.
        }

        /// <summary>
        /// Runs after the entire test run to perform any global cleanup.
        /// </summary>
        [AfterTestRun]
        public static void AfterTestRun()
        {
            // Any global cleanup can be done here, such as closing resources or generating reports.
        }
        #endregion

        #region Scenario Hooks
        /// <summary>
        /// Runs before each test scenario to initialize the WebDriver and 
        /// store it in the scenario context to be used in step definitions.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        [BeforeScenario]
        public static void BeforeScenario(ScenarioContext scenarioContext)
        {
            IWebDriver driver = DriverFactory.GetDriver();
            scenarioContext.Set(driver);
        }

        /// <summary>
        /// Runs after each test scenario to clean up the WebDriver instance.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        [AfterScenario]
        public static void AfterScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TryGetValue<IWebDriver>(out var driver))
            {
                driver.Quit();
            }
        }
        #endregion
    }
}
