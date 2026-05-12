/**
 * Program:         TestHooks.cs
 * Author:          Manh Khang Vu
 * Date:            2026-05-08
 * Description:     A class that provides hooks for setting up and tearing down test environments using Selenium WebDriver.
 */

using AventStack.ExtentReports;
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
        // Driver & context
        private static IWebDriver? _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;

        // Logging & Reports
        private readonly string _timestamp;
        private string? _scenarioLogFilePath;
        private static string? _testRunDirectoryPath;
        private static string? _mainLogFilePath;
        private static readonly object _fileLock = new();

        // Extent Reports
        private static ExtentReports? _extentReports;
        private static ExtentTest? _featureNode;
        private ExtentTest? _scenarioNode;

        // Configuration Flags
        private static readonly bool _takeScreenshotOnFailure = true;
        private static readonly bool _takeScreenshotOnPass = false;
        private static readonly bool _takeScreenshotOnStep = false;
        private static readonly bool _isDetailedLoggingEnabled = true;
        #endregion

        #region Constructor
        public TestHooks(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext ?? throw new ArgumentNullException(nameof(featureContext));
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));

            _timestamp = DateTime.Now.ToString("HHmmss");
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
                //ReportManager.InitReport();
                //_extent = ReportManager.GetExtent();

                //_testRunDirectoryPath = PathHelper.GetBaseDir();
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
        public static void BeforeFeature(FeatureContext featureContext)
        {
            // Any setup specific to a feature can be done here, such as initializing feature-specific data.
            _featureNode = _extentReports?.CreateTest(featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        public static void AfterFeature(FeatureContext featureContext)
        {
            // Any cleanup specific to a feature can be done here, such as clearing feature-specific data.
        }
        #endregion

        #region Scenario Hooks
        /// <summary>
        /// Runs before each test scenario to initialize the WebDriver and 
        /// store it in the scenario context to be used in step definitions.
        /// </summary>
        [BeforeScenario]
        public void BeforeScenario()
        {
            try
            {
                _driver = DriverFactory.GetDriver();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during BeforeScenario: {ex.Message}");
            }
        }

        /// <summary>
        /// Runs after each test scenario to clean up the WebDriver instance.
        /// </summary>
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
