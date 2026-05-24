/**
 * Program:         TestHooks.cs
 * Author:          Manh Khang Vu
 * Date:            2026-05-08
 * Description:     A class that provides hooks for setting up and tearing down test environments using Selenium WebDriver.
 */

using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using OpenQA.Selenium;
using Reqnroll;
using ReqnrollAutomation.Drivers;
using ReqnrollAutomation.Extensions;
using ReqnrollAutomation.Helpers;
using System.Collections.Concurrent;
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
        // Context
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;

        // Logging & Reports
        private readonly string _timestamp;
        private string? _scenarioLogFilePath;
        private static string? _testRunDirectoryPath;
        private static string? _mainLogFilePath;
        private static readonly object _fileLock = new();

        // Extent Reports (thread-safe with ConcurrentDictionary)
        private static ExtentReports? _extentReports;
        private static readonly ConcurrentDictionary<string, ExtentTest> _featureNodes = new();
        private static readonly object _reportLock = new();

        // Per-scenario test node (stored in ScenarioContext for thread safety)
        private ExtentTest? _scenarioNode;
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
        /// <exception cref="Exception">Thrown when an error occurs during setup.</exception>
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

                // Clean up old report directories
                CleanupOldReports();

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
        /// <exception cref="Exception">Thrown when an error occurs during cleanup.</exception>
        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                // Log the end of the test run
                WriteMainLog("[LOG] Test run completed");

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
        /// <exception cref="Exception">Thrown when an error occurs during feature setup.</exception>
        #region Feature Hooks
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            try
            {
                string featureTitle = featureContext.FeatureInfo.Title;

                // Lock before checking and adding to the report to prevent race conditions where
                // multiple threads might try to create a node for the same feature at the same time
                lock (_reportLock)
                {
                    // Create a node for the current feature in the Extent Report (thread-safe)
                    // GetOrAdd() ensures only one feature node is created per feature
                    _featureNodes.GetOrAdd(featureTitle, key => _extentReports!.CreateTest(key));
                }

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
        /// <exception cref="Exception">Thrown when an error occurs during feature cleanup.</exception>
        [AfterFeature]
        public static void AfterFeature(FeatureContext featureContext)
        {
            // Try-catch block is unnecessary here at the moment, but can be future-proofed
            try
            {
                WriteMainLog($"[LOG] Feature completed:  {featureContext.FeatureInfo.Title}");
                WriteMainLog(new string('=', 100));
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
        /// <exception cref="Exception">Thrown when an error occurs during scenario setup.</exception>
        [BeforeScenario]
        public void BeforeScenario()
        {
            try
            {
                string featureTitle = _featureContext.FeatureInfo.Title;

                // Get the feature node (thread-safe)
                if (!_featureNodes.TryGetValue(featureTitle, out ExtentTest? feature))
                {
                    throw new InvalidOperationException($"Feature node not found for: {featureTitle}");
                }

                // Create a node for the current scenario under the feature node in the Extent Report (thread-safe)
                lock (_reportLock)
                {
                    _scenarioNode = feature.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
                }

                string message = $"[LOG] Scenario started: {_scenarioContext.ScenarioInfo.Title}";

                // Thread-safe logging
                lock (_reportLock)
                {
                    _scenarioNode.Log(Status.Info, LogMessageFormatter.FormatLogMessage(message));
                }

                WriteMainLog(message);  // Log to main log

                // Create a new WebDriver instance for this scenario (not shared between scenarios)
                IWebDriver driver = DriverFactory.CreateDriver();
                _scenarioContext["WebDriver"] = driver;

                // Log to the scenario log file
                _scenarioLogFilePath = GetLogPath();
                WriteLog(message);
                WriteLog($"[LOG] Feature: {_featureContext.FeatureInfo.Title}");
                WriteLog($"[LOG] Browser: {(driver as IHasCapabilities)?.Capabilities.GetCapability("browserName")}");
                WriteLog($"[LOG] Time: {DateTime.Now:HH:mm:ss.fff}");
            }
            catch (Exception ex) {
                WriteMainLog($"[ERROR] BeforeScenario Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Runs after each test scenario to clean up the WebDriver instance.
        /// </summary>
        /// <exception cref="Exception">Thrown when an error occurs during scenario cleanup.</exception>"
        [AfterScenario]
        public void AfterScenario()
        {
            IWebDriver? driver = null;
            try
            {
                // Retrieve the driver from ScenarioContext
                driver = _scenarioContext.GetDriver();

                string message;
                if (_scenarioContext.TestError != null)
                {
                    // Log the error message to both the Extent Report and the main log
                    message = $"[ERROR] Scenario failed: {_scenarioContext.TestError.Message}";

                    // Take a screenshot
                    string screenshotPath = CaptureScreenshot($"FAILED_{_scenarioContext.ScenarioInfo.Title}", driver);
                    string screenshotHtml = GetBase64ScreenshotHtml(screenshotPath);

                    // Thread-safe logging
                    lock (_reportLock)
                    {
                        _scenarioNode?.Fail($"{LogMessageFormatter.FormatErrorMessage("[ERROR] Scenario failed")} {LogMessageFormatter.FormatExceptionMessage(_scenarioContext.TestError)}{screenshotHtml}");  // Log to Extent Report
                    }
                }
                else
                {
                    // Log the error message to both the Extent Report and the main log
                    message = $"[PASS] Scenario passed";
                    
                    // Take a screenshot
                    string screenshotPath = CaptureScreenshot($"PASSED_{_scenarioContext.ScenarioInfo.Title}", driver);
                    string screenshotHtml = GetBase64ScreenshotHtml(screenshotPath);

                    // Thread-safe logging
                    lock (_reportLock)
                    {
                        _scenarioNode?.Pass($"{LogMessageFormatter.FormatPassMessage("[PASS] Scenario passed")}{screenshotHtml}"); // Log to Extent Report
                    }
                }

                // Log to main log
                WriteMainLog(message);
                WriteMainLog(new string('-', 100));

                // Log to the scenario log file
                WriteLog(message);
            }
            catch (Exception ex)
            {
                WriteMainLog($"[ERROR] AfterScenario Error: {ex.Message}");
            }
            finally
            {
                // Flush the Extent Report after each scenario to ensure logs are written to the file
                try
                {
                    lock (_reportLock)
                    {
                        ReportManager.FlushReport();
                    }
                }
                catch (Exception ex)
                {
                    WriteMainLog($"[ERROR] Failed to flush report: {ex.Message}");
                }

                // Clear cookies and cache to ensure test isolation
                try
                {
                    DriverFactory.ClearCookiesAndCache(driver);
                }
                catch (Exception ex)
                {
                    WriteMainLog($"[ERROR] Failed to clear cookies and cache: {ex.Message}");
                }

                // Guarantee cleanup of the WebDriver instance for this scenario
                try
                {
                    DriverFactory.QuitDriver(driver);
                }
                catch (Exception ex)
                {
                    WriteMainLog($"[ERROR] Failed to quit driver: {ex.Message}");
                }
            }
        }
        #endregion

        #region Step Hooks
        /// <summary>
        /// Runs before each test step begins.
        /// </summary>
        /// <exception cref="Exception">Thrown when an error occurs during step setup.</exception>
        [BeforeStep]
        public void BeforeStep()
        {
            try
            {
                StepInfo stepInfo = _scenarioContext.StepContext.StepInfo;
                string message = $"[LOG] Step started: {stepInfo.StepDefinitionType} {stepInfo.Text}";

                // Thread-safe logging
                lock (_reportLock)
                {
                    _scenarioNode?.Log(Status.Info, LogMessageFormatter.FormatLogMessage(message));   // Log to Extent Report
                }
                
                WriteLog(message); // Log to scenario log file
            }
            catch (Exception ex)
            {
                WriteLog($"[ERROR] BeforeStep Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Runs after each test step completes to take screenshots after each step.
        /// </summary>
        [AfterStep]
        public void AfterStep()
        {
            IWebDriver? driver = null;
            try
            {
                // Retrieve the driver from ScenarioContext
                driver = _scenarioContext.GetDriver();

                StepInfo stepInfo = _scenarioContext.StepContext.StepInfo;
                string message;
                if (_scenarioContext.TestError != null)
                {
                    // Log the error message to both the Extent Report and the main log
                    message = $"[ERROR] Step failed: {_scenarioContext.TestError.Message}";

                    // Take a screenshot if the step failed
                    string screenshotPath = CaptureScreenshot($"FAILED_{stepInfo.Text}", driver);
                    string screenshotHtml = GetBase64ScreenshotHtml(screenshotPath);

                    // Thread-safe logging
                    lock (_reportLock)
                    {
                        _scenarioNode?.Fail($"{LogMessageFormatter.FormatErrorMessage("[ERROR] Step failed")} {LogMessageFormatter.FormatExceptionMessage(_scenarioContext.TestError)}{screenshotHtml}");  // Log to Extent Report
                    }
                }
                else
                {
                    message = $"[PASS] Step passed";

                    // Take a screenshot
                    string screenshotPath = CaptureScreenshot($"PASSED_{NormalizeFileName($"{stepInfo.StepDefinitionType} {stepInfo.Text}")}", driver);
                    string screenshotHtml = GetBase64ScreenshotHtml(screenshotPath);

                    // Thread-safe logging
                    lock (_reportLock)
                    {
                        _scenarioNode?.Pass($"{LogMessageFormatter.FormatPassMessage("[PASS] Step passed")}{screenshotHtml}"); // Log to Extent Report
                    }
                }

                // Log to main log
                WriteMainLog(message);

                // Log to the scenario log file
                WriteLog(message);
                WriteLog(new string('-', 75));
            }
            catch (Exception ex)
            {
                WriteLog($"[ERROR] AfterStep Error: {ex.Message}");
            }
            finally
            {
                // Flush the Extent Report after each step to ensure logs are written to the file
                try
                {
                    lock (_reportLock)
                    {
                        ReportManager.FlushReport();
                    }
                }
                catch (Exception ex)
                {
                    WriteMainLog($"[ERROR] Failed to flush report: {ex.Message}");
                }
            }
        }
        #endregion

        #region Private Screenshot Methods
        /// <summary>
        /// Captures a screenshot of the current browser window and saves it to a file with a specified name.
        /// </summary>
        /// <param name="name">The base name used to generate the file name.</param>
        /// <returns>The full file path of the saved screenshot if successful; otherwise, an empty string.</returns>
        private string CaptureScreenshot(string name, IWebDriver? driver)
        {
            try
            {
                // Set up the file path for the screenshot
                string screenshotsDirectory = PathHelper.GetScreenshotsDirectoryPath();
                string fileName = $"{NormalizeFileName(name)}_{DateTime.Now:HHmmss}_{Environment.CurrentManagedThreadId}.png";
                string filePath = Path.Combine(screenshotsDirectory, fileName);

                // Take the screenshot and save it to the specified file path
                Screenshot? screenshot = (driver as ITakesScreenshot)?.GetScreenshot();
                screenshot?.SaveAsFile(filePath);
                WriteLog($"[LOG] Screenshot captured: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                WriteLog($"[ERROR] Failed to capture screenshot: {ex.Message}");
            }
                
            return "";
        }

        /// <summary>
        /// Generates an HTML <img> tag containing a Base64-encoded PNG image from the specified screenshot file path.
        /// </summary>
        /// <remarks>This method makes the Extent Report self-contained by embedding the screenshot directly in the report.</remarks>
        /// <param name="screenshotPath">The path to the screenshot image.</param>
        /// <returns>A string containing an HTML <img> tag with the screenshot image embedded 
        /// as a Base64-encoded PNG if successful; otherwise, an empty string.</returns>
        private string GetBase64ScreenshotHtml(string screenshotPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(screenshotPath) && File.Exists(screenshotPath))
                {
                    string base64String = Convert.ToBase64String(File.ReadAllBytes(screenshotPath));
                    return $"<br><img src='data:image/png;base64,{base64String}' style='width:75%'/>";
                }
            }
            catch (Exception ex)
            {
                WriteLog($"[ERROR] Failed to convert screenshot to Base64: {ex.Message}");
            }
                
            return "";
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
            if (string.IsNullOrEmpty(_scenarioLogFilePath))
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

        /// <summary>
        /// Cleans up old report directories, keeping only the 10 most recent ones.
        /// </summary>
        private static void CleanupOldReports()
        {
            try
            {
                // Keep the 10 most recent reports
                const int numberOfReportsToKeep = 10;
                string baseDirectory = Directory.GetParent(PathHelper.BaseDirectory)!.FullName; // "TestResults" folder
                // If the base directory doesn't exist, there's nothing to clean up
                if (!Directory.Exists(baseDirectory))
                    return;

                // Clean up deployment folders created by Reqnroll, which are located in the "TestResults" folder and have names starting with "Deploy_"
                List<DirectoryInfo> deploymentDirectories = Directory.GetDirectories(baseDirectory, "Deploy_*")
                                                            .Select(d => new DirectoryInfo(d))
                                                            .ToList();
                foreach (DirectoryInfo deploymentDirectory in deploymentDirectories)
                {
                    try
                    {
                        string deploymentName = deploymentDirectory.Name;
                        deploymentDirectory.Delete(true);
                        WriteMainLog($"[LOG] Deleted old deployment directory: {deploymentName}");
                    }
                    catch (Exception ex)
                    {
                        WriteMainLog($"[ERROR] Failed to delete old deployment directory: {ex.Message}");
                    }
                }

                // Delete old report directories, keeping only the most recent ones based on creation time
                // Folder name format: yyyy-MM-dd_HHmmss
                List<DirectoryInfo> reportDirectories = Directory.GetDirectories(baseDirectory)
                                                        .Select(d => new DirectoryInfo(d))
                                                        .OrderByDescending(d => d.CreationTime)
                                                        .ToList();
                foreach (DirectoryInfo dir in reportDirectories.Skip(numberOfReportsToKeep))
                {
                    try
                    {
                        string reportName = dir.Name;
                        dir.Delete(true);
                        WriteMainLog($"[LOG] Deleted old report directory: {reportName}");
                    }
                    catch (Exception ex)
                    {
                        WriteMainLog($"[ERROR] Failed to delete old report directory: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteMainLog($"[ERROR] Failed to clean up old reports: {ex.Message}");
            }
        }
        #endregion
    }
}

namespace ReqnrollAutomation.Extensions
{
    public static class ScenarioContextExtensions
    {
        /// <summary>
        /// Gets the WebDriver instance from the current scenario context.
        /// </summary>
        /// <remarks>Should be used by step defintions to retrieve the driver instance.</remarks>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <returns>The WebDriver instance.</returns>
        /// <exception cref="InvalidOperationException">Throws if the WebDriver instance is not found in the scenario context.</exception>
        public static IWebDriver GetDriver(this ScenarioContext scenarioContext)
        {
            if (scenarioContext.TryGetValue("WebDriver", out IWebDriver driver) && driver != null)
            {
                return driver;
            }

            throw new InvalidOperationException("WebDriver instance not found in ScenarioContext. Ensure BeforeScenario hook has been executed.");
        }
    }
}