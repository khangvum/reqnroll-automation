/**
 * Program:         PathHelper.cs
 * Author:          Manh Khang Vu
 * Date:            2026-05-12
 * Description:     A class that includes helper methods for handling file paths and directories.
 */

namespace ReqnrollAutomation.Helpers
{
    /// <summary>
    /// A class that includes helper methods for handling file paths and directories.
    /// </summary>
    internal static class PathHelper
    {
        #region Private Attributes
        private static readonly string _timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
        private static readonly string _baseDirectory = Path.Combine(
            Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? Path.GetTempPath(),
            "TestResults",
            _timestamp);
        #endregion

        #region Public Properties
        public static string BaseDirectory => _baseDirectory;
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the full path to the directory used for storing reports, creating the directory if it does not already
        /// exist.
        /// </summary>
        /// <returns>The full path to the reports directory.</returns>
        public static string GetReportDirectoryPath()
        {
            string reportDir = Path.Combine(_baseDirectory, "Reports");
            Directory.CreateDirectory(reportDir);
            return reportDir;
        }

        /// <summary>
        /// Gets the full path to the directory used for storing screenshots, creating the directory if it does not already
        /// exist.
        /// </summary>
        /// <returns>The full path to the screenshots directory.</returns>
        public static string GetScreenshotsDirectoryPath()
        {
            string screenshotsDir = Path.Combine(_baseDirectory, "Screenshots");
            Directory.CreateDirectory(screenshotsDir);
            return screenshotsDir;
        }

        /// <summary>
        /// Gets the full path to the directory used for storing logs, creating the directory if it does not already
        /// exist.
        /// </summary>
        /// <returns>The full path to the logs directory.</returns>
        public static string GetLogDirectoryPath()
        {
            string logsDir = Path.Combine(_baseDirectory, "Logs");
            Directory.CreateDirectory(logsDir);
            return logsDir;
        }
        #endregion
    }
}
