/**
 * Program:         TestHooks.cs
 * Author:          Manh Khang Vu
 * Date:            2026-05-08
 * Description:     A class that provides hooks for setting up and tearing down test environments using Selenium WebDriver.
 */

using AventStack.ExtentReports;
using OpenQA.Selenium;
using Reqnroll;
using ReqnrollAutomation.Drivers;
using ReqnrollAutomation.Helpers;
using System.Diagnostics;

namespace ReqnrollAutomation.Hooks
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
        }

        /// <summary>
        /// Runs after the entire test run to perform any global cleanup.
        /// </summary>
        [AfterTestRun]
        public static void AfterTestRun()
        {
        }
        #endregion

        #region Feature Hooks
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            // Create a node for the current feature in the Extent Report
            _featureNode = _extentReports?.CreateTest(featureContext.FeatureInfo.Title);

            string message = $"[FEATURE] {featureContext.FeatureInfo.Title}";
            Console.WriteLine(message);
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
        }

        /// <summary>
        /// Runs after each test scenario to clean up the WebDriver instance.
        /// </summary>
        [AfterScenario]
        public void AfterScenario()
        {
        }
        #endregion

        #region Private Screenshot Methods
        #endregion

        #region Private Helper Methods
        /// <summary>
        /// Normalizes a file name by replacing invalid characters with underscores and truncating it to a reasonable length.
        /// </summary>
        /// <param name="fileName">The file name to normalize.</param>
        /// <returns>The normalized file name.</returns>
        private static string NormalizeFileName(string fileName)
        {
            // If the file name is null or empty, return a default name
            if (string.IsNullOrEmpty(fileName))
                return "Unknown";

            // Replace invalid characters with underscores
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidFileNameChars)
                fileName = fileName.Replace(invalidChar, '_');

            // Replace spaces anmd special characters
            fileName = fileName.Replace(" ", "_")
                               .Replace(":", "")
                               .Replace("-", "_")
                               .Replace("(", "")
                               .Replace(")", "");

            return fileName.Length > 50 ? fileName.Substring(0, 50) : fileName;
        }
        #endregion
    }
}
