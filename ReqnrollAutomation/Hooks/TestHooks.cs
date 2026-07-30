/**
 * Program:         TestHooks.cs
 * Author:          Manh Khang Vu
 * Date:            2026-05-08
 * Description:     A class that provides hooks for setting up and tearing down test environments using Selenium WebDriver.
 */

using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using ReqnrollAutomation.Config;
using ReqnrollAutomation.Core.Config;
using ReqnrollAutomation.Core.Extensions;
using ReqnrollAutomation.Core.Helpers;
using ReqnrollAutomation.Drivers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

        // Extent Reports (thread-safe with ConcurrentDictionary)
        private static ExtentReports? _extentReports;
        private static readonly ConcurrentDictionary<string, ExtentTest> _featureNodes = new();
        private static readonly Lock _reportLock = new();

        // Per-scenario test node
        private ExtentTest? _scenarioNode;
        #endregion

        #region Constructor
        public TestHooks(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _featureContext = featureContext ?? throw new ArgumentNullException(nameof(featureContext));
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
        }
        #endregion

        #region Test Run Hooks
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            try
            {
                ConfigProvider.Initialize(new ConfigAdapter());

                ReportManager.InitializeReport("Reqnroll Automation");
                _extentReports = ReportManager.GetExtentReports();
                Console.WriteLine("[LOG] Extent Reports initialized successfully.");

                Directory.CreateDirectory(PathHelper.GetScreenshotsDirectoryPath());
                CleanupOldReports();

                Console.WriteLine("[LOG] Test run started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] BeforeTestRun Error: {ex.Message}");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                Console.WriteLine("[LOG] Test run completed. Cleaning up all driver instances...");

                // Terminate all 4 thread-bound WebDriver instances
                DriverFactory.QuitAllDrivers();

                ReportManager.FlushReport();

                string reportPath = ReportManager.ReportPath;
                ExtentReportPatcher.Patch(reportPath);

                if (!ConfigManager.Headless && !string.IsNullOrEmpty(reportPath) && File.Exists(reportPath))
                {
                    Console.WriteLine($"[LOG] Opening report: {reportPath}");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = reportPath,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] AfterTestRun Error: {ex.Message}");
            }
        }
        #endregion

        #region Feature Hooks
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            try
            {
                string featureTitle = featureContext.FeatureInfo.Title;

                lock (_reportLock)
                {
                    _featureNodes.GetOrAdd(featureTitle, key => _extentReports!.CreateTest(key));
                }

                Console.WriteLine($"[LOG] Feature started: {featureContext.FeatureInfo.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] BeforeFeature Error: {ex.Message}");
            }
        }

        [AfterFeature]
        public static void AfterFeature(FeatureContext featureContext)
        {
            try
            {
                Console.WriteLine($"[LOG] Feature completed:  {featureContext.FeatureInfo.Title}");
                Console.WriteLine(new string('=', 100));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] AfterFeature Error: {ex.Message}");
            }
        }
        #endregion

        #region Scenario Hooks
        [BeforeScenario]
        public void BeforeScenario()
        {
            try
            {
                string featureTitle = _featureContext.FeatureInfo.Title;

                if (!_featureNodes.TryGetValue(featureTitle, out ExtentTest? feature))
                {
                    throw new InvalidOperationException($"Feature node not found for: {featureTitle}");
                }

                lock (_reportLock)
                {
                    _scenarioNode = feature.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
                }

                string message = $"[LOG] Scenario started: {_scenarioContext.ScenarioInfo.Title}";

                lock (_reportLock)
                {
                    _scenarioNode.Log(Status.Info, LogMessageFormatter.FormatLogMessage(message));
                }

                Console.WriteLine(message);

                // Reuses worker thread's driver or launches a new driver instance if thread is new
                IWebDriver driver = DriverFactory.GetOrCreateDriver();
                _scenarioContext.SetDriver(driver);

                // Ensure clean cookies and local/session storage state for the scenario
                DriverFactory.ResetSession();

                Console.WriteLine(message);
                Console.WriteLine($"[LOG] Feature: {_featureContext.FeatureInfo.Title}");
                Console.WriteLine($"[LOG] Browser: {(driver as IHasCapabilities)?.Capabilities.GetCapability("browserName")}");
                Console.WriteLine($"[LOG] Time: {DateTime.Now:HH:mm:ss.fff}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] BeforeScenario Error: {ex.Message}");
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                IWebDriver driver = _scenarioContext.GetDriver();
                string message;

                if (_scenarioContext.TestError != null)
                {
                    message = $"[ERROR] Scenario failed: {_scenarioContext.TestError.Message}";
                    string screenshotPath = CaptureScreenshot($"FAILED_{_scenarioContext.ScenarioInfo.Title}", driver);
                    string screenshotHtml = GetBase64ScreenshotHtml(screenshotPath);

                    lock (_reportLock)
                    {
                        _scenarioNode?.Fail($"{LogMessageFormatter.FormatErrorMessage("[ERROR] Scenario failed")} {LogMessageFormatter.FormatExceptionMessage(_scenarioContext.TestError)}{screenshotHtml}");
                    }
                }
                else
                {
                    message = $"[PASS] Scenario passed";
                    string screenshotPath = CaptureScreenshot($"PASSED_{_scenarioContext.ScenarioInfo.Title}", driver);
                    string screenshotHtml = GetBase64ScreenshotHtml(screenshotPath);

                    lock (_reportLock)
                    {
                        _scenarioNode?.Pass($"{LogMessageFormatter.FormatPassMessage("[PASS] Scenario passed")}{screenshotHtml}");
                    }
                }

                Console.WriteLine(message);
                Console.WriteLine(new string('-', 100));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] AfterScenario Error: {ex.Message}");
            }
            finally
            {
                try
                {
                    lock (_reportLock)
                    {
                        ReportManager.FlushReport();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to flush report: {ex.Message}");
                }

                // Reset state so the driver is clean for the next scenario on this worker thread
                DriverFactory.ResetSession();
            }
        }
        #endregion

        #region Step Hooks
        [BeforeStep]
        public void BeforeStep()
        {
            try
            {
                StepInfo stepInfo = _scenarioContext.StepContext.StepInfo;
                string message = $"[LOG] Step started: {stepInfo.StepDefinitionType} {stepInfo.Text}";

                lock (_reportLock)
                {
                    _scenarioNode?.Log(Status.Info, LogMessageFormatter.FormatLogMessage(message));
                }

                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] BeforeStep Error: {ex.Message}");
            }
        }

        [AfterStep]
        public void AfterStep()
        {
            try
            {
                IWebDriver driver = _scenarioContext.GetDriver();
                StepInfo stepInfo = _scenarioContext.StepContext.StepInfo;
                string message;

                if (_scenarioContext.TestError != null)
                {
                    message = $"[ERROR] Step failed: {_scenarioContext.TestError.Message}";
                    string screenshotPath = CaptureScreenshot($"FAILED_{stepInfo.Text}", driver);
                    string screenshotHtml = GetBase64ScreenshotHtml(screenshotPath);

                    lock (_reportLock)
                    {
                        _scenarioNode?.Fail($"{LogMessageFormatter.FormatErrorMessage("[ERROR] Step failed")} {LogMessageFormatter.FormatExceptionMessage(_scenarioContext.TestError)}{screenshotHtml}");
                    }
                }
                else
                {
                    message = $"[PASS] Step passed";
                    string screenshotPath = CaptureScreenshot($"PASSED_{NormalizeFileName($"{stepInfo.StepDefinitionType} {stepInfo.Text}")}", driver);
                    string screenshotHtml = GetBase64ScreenshotHtml(screenshotPath);

                    lock (_reportLock)
                    {
                        _scenarioNode?.Pass($"{LogMessageFormatter.FormatPassMessage("[PASS] Step passed")}{screenshotHtml}");
                    }
                }

                Console.WriteLine(message);
                Console.WriteLine(new string('-', 75));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] AfterStep Error: {ex.Message}");
            }
            finally
            {
                try
                {
                    lock (_reportLock)
                    {
                        ReportManager.FlushReport();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to flush report: {ex.Message}");
                }
            }
        }
        #endregion

        #region Private Screenshot Methods
        private string CaptureScreenshot(string name, IWebDriver? driver)
        {
            try
            {
                string screenshotsDirectory = PathHelper.GetScreenshotsDirectoryPath();
                string fileName = $"{NormalizeFileName(name)}_{DateTime.Now:HHmmss}_{Environment.CurrentManagedThreadId}.png";
                string filePath = Path.Combine(screenshotsDirectory, fileName);

                Screenshot? screenshot = (driver as ITakesScreenshot)?.GetScreenshot();
                screenshot?.SaveAsFile(filePath);
                Console.WriteLine($"[LOG] Screenshot captured: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to capture screenshot: {ex.Message}");
            }

            return "";
        }

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
                Console.WriteLine($"[ERROR] Failed to convert screenshot to Base64: {ex.Message}");
            }

            return "";
        }
        #endregion

        #region Private Helper Methods
        private static string NormalizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "Unknown";

            fileName = Regex.Replace(fileName, @"[ :()_""-]", "_");

            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidFileNameChars)
                fileName = fileName.Replace(invalidChar, '_');

            return fileName.Length > 50 ? fileName.Substring(0, 50) : fileName;
        }

        private static void CleanupOldReports()
        {
            try
            {
                const int numberOfReportsToKeep = 15;
                string baseDirectory = PathHelper.BaseDirectory!;

                if (!Directory.Exists(baseDirectory))
                    return;

                string testResultsDirectory = Directory.GetParent(baseDirectory)!.FullName;
                List<DirectoryInfo> deploymentDirectories = Directory.GetDirectories(testResultsDirectory, "Deploy_*")
                                                            .Select(d => new DirectoryInfo(d))
                                                            .ToList();
                foreach (DirectoryInfo deploymentDirectory in deploymentDirectories)
                {
                    try
                    {
                        string deploymentName = deploymentDirectory.Name;
                        deploymentDirectory.Delete(true);
                        Console.WriteLine($"[LOG] Deleted old deployment directory: {deploymentName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Failed to delete old deployment directory: {ex.Message}");
                    }
                }

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
                        Console.WriteLine($"[LOG] Deleted old report directory: {reportName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Failed to delete old report directory: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to clean up old reports: {ex.Message}");
            }
        }
        #endregion
    }
}