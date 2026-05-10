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
        #region Private Attributes
        private static IWebDriver? _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        #endregion

        #region Constructor
        public TestHooks(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext ?? throw new ArgumentNullException(nameof(featureContext));
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }
        #endregion

        /// <summary>
        /// Runs before the entire test run to perform any global setup.
        /// </summary>
        #region Test Run Hooks
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            try
            {
                // Any global setup can be done here, such as initializing logging or configuration.                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during BeforeTestRun: {ex.Message}");
            }
        }

        /// <summary>
        /// Runs after the entire test run to perform any global cleanup.
        /// </summary>
        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                // Any global cleanup can be done here, such as closing resources or generating reports.
                DriverFactory.QuitDriver();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during AfterTestRun: {ex.Message}");
            }
        }
        #endregion

        #region Feature Hooks
        [BeforeFeature]
        public static void BeforeFeature()
        {
            // Any setup specific to a feature can be done here, such as initializing feature-specific data.
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            // Any cleanup specific to a feature can be done here, such as clearing feature-specific data.
        }
        #endregion

        #region Scenario Hooks
        /// <summary>
        /// Runs before each test scenario to initialize the WebDriver and 
        /// store it in the scenario context to be used in step definitions.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        [BeforeScenario]
        public void BeforeScenario()
        {
            try
            {
                IWebDriver driver = DriverFactory.GetDriver();
                _scenarioContext.Set(driver);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during BeforeScenario: {ex.Message}");
            }
        }

        /// <summary>
        /// Runs after each test scenario to clean up the WebDriver instance.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                if (_scenarioContext.TestError != null)
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during AfterScenario: {ex.Message}");
            }
        }
        #endregion
    }
}
