/**
 * Program:         ReportManager.cs
 * Author:          Manh Khang Vu
 * Date:            2026-05-14
 * Description:     A class that manages the creation and configuration of ExtentReports
 *                  for test reporting in the Reqnroll automation framework.
 */

using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

namespace ReqnrollAutomation.Helpers
{
    /// <summary>
    /// A class that manages the creation and configuration of ExtentReports
    /// for test reporting in the Reqnroll automation framework.
    /// </summary>
    internal static class ReportManager
    {
        #region Private Attributes
        private static ExtentReports? _extentReports;
        private static ExtentSparkReporter? _extentSparkReporter;
        private static string _reportPath = "";
        private static readonly object _flushLock = new();
        #endregion

        #region Public Properties
        public static string ReportPath => _reportPath;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes the ExtentReports and configures the report settings.
        /// </summary>
        public static void InitializeReport()
        {
            // Set up the report path
            string reportDir = PathHelper.GetReportDirectoryPath();
            _reportPath = Path.Combine(reportDir, "ExtentReport.html");
            Console.WriteLine($"Extent Report Path: {_reportPath}");

            // Set up the ExtentSparkReporter
            _extentSparkReporter = new(_reportPath);
            _extentSparkReporter.Config.DocumentTitle = "Reqnroll Automation Test Report";
            _extentSparkReporter.Config.ReportName = "Reqnroll Automation Tests";
            _extentSparkReporter.Config.Theme = Theme.Dark;

            // Set up the ExtentReports
            _extentReports = new();
            _extentReports.AttachReporter(_extentSparkReporter);

            _extentReports.AddSystemInfo("Environment", "QA");
            _extentReports.AddSystemInfo("Tester", Environment.UserName);
            _extentReports.AddSystemInfo("OS", Environment.OSVersion.ToString());

            Console.WriteLine("[LOG] Report initialized successfully.");
        }

        /// <summary>
        /// Gets the singleton instance of the ExtentReports object used for reporting.
        /// </summary>
        /// <returns>The singleton instance of the ExtentReports.</returns>
        public static ExtentReports GetExtentReports()
        {
            if (_extentReports == null)
            {
                InitializeReport();
            }
            return _extentReports!;
        }

        /// <summary>
        /// Flushes and saves the report.
        /// </summary>
        public static void FlushReport()
        {
            try
            {
                lock (_flushLock)
                {
                    _extentReports?.Flush();
                }
                Console.WriteLine("[LOG] Report saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Report failed to save: {ex.Message}");
            }
        }
        #endregion
    }
}
