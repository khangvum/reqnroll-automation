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
using System.Text;

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
            try
            {
                // Set up Extent Reports
                ReportManager.InitializeReport();
                _extentReports = ReportManager.GetExtentReports();
                //Console.WriteLine("[LOG] Extent Reports initialized successfully.");
                WriteMainLog("[LOG] Extent Reports initialized successfully.");

                // Set up the directories for reports, screenshots, and logs
                Directory.CreateDirectory(PathHelper.GetScreenshotsDirectoryPath());
                Directory.CreateDirectory(PathHelper.GetLogDirectoryPath());
                _testRunDirectoryPath = PathHelper.BaseDirectory;
                _mainLogFilePath = Path.Combine(PathHelper.GetLogDirectoryPath(), $"TestRun_{DateTime.Now:yyyyMMdd_HHmmss}.log");

                // Log the start of the test run
                WriteMainLog("[LOG] Test run started");
            }
            catch (Exception ex)
            {
                WriteMainLog($"[ERROR] BeforeTestRun Error: {ex.Message}");
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
                // Log the end of the test run
                WriteMainLog("[LOG] Test run completed");

                // Clean up the WebDriver instance
                DriverFactory.QuitDriver();

                // Flush the Extent Report
                ReportManager.FlushReport();

                // Automatically open the Extent Report in the browser if in headfull mode
                string reportPath = ReportManager.ReportPath;
                bool isHeadless = Environment.GetEnvironmentVariable("HEADLESS") == "1";
                if (!isHeadless && !string.IsNullOrEmpty(reportPath) && File.Exists(reportPath))
                {
                    WriteMainLog($"[LOG] Opening report: {reportPath}");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = reportPath,
                        UseShellExecute = true
                    });
                }

            }
            catch (Exception ex)
            {
                WriteMainLog($"[ERROR] AfterTestRun Error: {ex.Message}");
            }
        }
        #endregion

        /// <summary>
        /// Runs before each feature to create a node in the Extent Report for the current feature.
        /// </summary>
        /// <param name="featureContext">The feature context.</param>
        #region Feature Hooks
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            try
            {
                // Create a node for the current feature in the Extent Report
                _featureNode = _extentReports?.CreateTest(featureContext.FeatureInfo.Title);

                WriteMainLog($"[LOG] Feature started: {featureContext.FeatureInfo.Title}");
            }
            catch (Exception ex)
            {
                WriteMainLog($"[ERROR] BeforeFeature Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Runs after each feature to perform any necessary cleanup.
        /// </summary>
        /// <param name="featureContext">The feature context.</param>
        [AfterFeature]
        public static void AfterFeature(FeatureContext featureContext)
        {
            // Try-catch block is unnecessary here at the moment, but can be future-proofed
            try
            {
                WriteMainLog($"[LOG] Feature completed");
            }
            catch (Exception ex)
            {
                WriteMainLog($"[ERROR] AfterFeature Error: {ex.Message}");
            }
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
                // Create a node for the current scenario in the Extent Report under the current feature node
                _scenarioNode = _featureNode?.CreateNode(_scenarioContext.ScenarioInfo.Title);
                string message = $"[LOG] Scenario started: {_scenarioContext.ScenarioInfo.Title}";
                _scenarioNode?.Log(Status.Info, message);   // Log to Extent Report
                WriteMainLog(message);  // Log to main log

                // Initialize the WebDriver instance and store it in the scenario context
                _driver = DriverFactory.GetDriver();
                _scenarioContext["WebDriver"] = _driver;

                // Log to the scenario log file
                _scenarioLogFilePath = GetLogPath();
                WriteLog(message);
                WriteLog($"[LOG] Feature: {_featureContext.FeatureInfo.Title}");
                WriteLog($"[LOG] Browser: {(_driver as IHasCapabilities)?.Capabilities.GetCapability("browserName")}");
                WriteLog($"[LOG] Time: {DateTime.Now:HH:mm:ss.fff}");
            }
            catch (Exception ex) {
                WriteMainLog($"[ERROR] BeforeScenario Error: {ex.Message}");
            }
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
        /// <summary>
        /// Captures a screenshot of the current browser window and saves it to a file with a specified name.
        /// </summary>
        /// <param name="name">The base name used to generate the file name.</param>
        /// <returns>The full file path of the saved screenshot if successful; otherwise, an empty string.</returns>
        private string CaptureScreenshot(string name)
        {
            try
            {
                // Set up the file path for the screenshot
                string screenshotsDirectory = PathHelper.GetScreenshotsDirectoryPath();
                string fileName = $"{NormalizeFileName(name)}_{DateTime.Now:HHmmss}_{Environment.CurrentManagedThreadId}.png";
                string filePath = Path.Combine(screenshotsDirectory, fileName);

                // Take the screenshot and save it to the specified file path
                Screenshot? screenshot = (_driver as ITakesScreenshot)?.GetScreenshot();
                screenshot?.SaveAsFile(filePath);
                WriteLog($"[LOG] Screenshot captured: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                WriteLog($"[ERROR] Failed to capture screenshot: {ex.Message}");
                return "";
            }
        }
        #endregion

        #region Private Logging Methods
        /// <summary>
        /// Gets the log file path for the current scenario based on the scenario and feature titles.
        /// </summary>
        /// <returns>The log file path for the current scenario.</returns>
        private string GetLogPath()
        {
            // Initialize the log file path based on the scenario and feature titles
            string scenarioTitle = NormalizeFileName(_scenarioContext.ScenarioInfo.Title);
            string featureTitle = NormalizeFileName(_featureContext.FeatureInfo.Title);

            if (string.IsNullOrEmpty(scenarioTitle))
                scenarioTitle = "UnknownScenario";

            if (string.IsNullOrEmpty(featureTitle))
                featureTitle = "UnknownFeature";

            string fileName = $"{scenarioTitle}_{_timestamp}.log";
            string logDirectory = PathHelper.GetLogDirectoryPath();
            string featureLogDirectory = Path.Combine(logDirectory, featureTitle);
            return Path.Combine(featureLogDirectory, fileName);
        }

        /// <summary>
        /// Logs the specified message to the scenario log file with a timestamp.
        /// </summary>
        /// <param name="message">The message to log.</param>
        private void WriteLog(string message)
        {
            if (!_isDetailedLoggingEnabled || string.IsNullOrEmpty(_scenarioLogFilePath))
                return;

            lock (_fileLock)
            {
                try
                {
                    // Ensure the directory exists before writing to the log file
                    string? directory = Path.GetDirectoryName(_scenarioLogFilePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    // Log the message with a timestamp
                    string timestamp = $"[{DateTime.Now:HH:mm:ss.fff}] ";
                    string logMessage = timestamp + message;
                    File.AppendAllText(_scenarioLogFilePath, logMessage + Environment.NewLine, Encoding.UTF8);
                    Console.WriteLine(logMessage); // Also write to console for real-time visibility
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to write to log file: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Logs the specified message to the main log file with a timestamp.
        /// </summary>
        /// <remarks> This is used for logging messages that are relevant to the entire test run, such as 
        /// setup and teardown messages, or any critical errors that occur outside of individual scenarios.</remarks>
        /// <param name="message">The message to log.</param>
        private static void WriteMainLog(string message)
        {
            // Ensure the main log file path exists
            if (string.IsNullOrEmpty(_mainLogFilePath))
            {
                string fallbackLogPath = Path.Combine(Directory.GetCurrentDirectory(), "TestResults", "Logs");
                Directory.CreateDirectory(fallbackLogPath);
                _mainLogFilePath = Path.Combine(fallbackLogPath, $"TestRun_{DateTime.Now:yyyyMMdd_HHmmss}.log");
            }

            lock (_fileLock)
            {
                try
                {
                    // Ensure the directory exists before writing to the main log file
                    string? directory = Path.GetDirectoryName(_mainLogFilePath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    // Log the message with a timestamp
                    string timestamp = $"[{DateTime.Now:HH:mm:ss.fff}] ";
                    string logMessage = timestamp + message;
                    File.AppendAllText(_mainLogFilePath, logMessage + Environment.NewLine, Encoding.UTF8);
                    Console.WriteLine(logMessage); // Also write to console for real-time visibility
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to write to main log file: {ex.Message}");
                }
            }
        }
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
