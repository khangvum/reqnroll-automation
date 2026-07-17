/**
 * Program:         ConfigProvider.cs
 * Author:          Manh Khang Vu
 * Date:            2026-07-11
 * Description:     A class that provides configuration settings for the Reqnroll 
 *                  automation framework from the external IConfigAdapter.
 */

namespace ReqnrollAutomation.Core.Config
{
    /// <summary>
    /// Specifies the type of browser to be used for testing.
    /// </summary>
    public enum BrowserType
    {
        Chrome,
        Edge
    }

    /// <summary>
    /// A class that provides configuration settings for the Reqnroll 
    /// automation framework from the external IConfigAdapter.
    /// </summary>
    public static class ConfigProvider
    {
        // Private attributes
        private static IConfigAdapter? configAdapter;

        #region Public Properties
        /// <summary>
        /// Gets the configured test environment.
        /// </summary>
        public static string TestEnvironment
        {
            get
            {
                if (configAdapter == null)
                {
                    throw new InvalidOperationException("ConfigProvider is not initialized. Call Initialize() with a valid IConfigAdapter.");
                }
                return configAdapter.TestEnvironment;
            }
        }

        /// <summary>
        /// Gets the configured browser type.
        /// </summary>
        public static BrowserType Browser
        {
            get
            {
                if (configAdapter == null)
                {
                    throw new InvalidOperationException("ConfigProvider is not initialized. Call Initialize() with a valid IConfigAdapter.");
                }
                return configAdapter.Browser;
            }
        }

        /// <summary>
        /// Gets the configured default timeout in seconds for waiting operations.
        /// </summary>
        public static int DefaultTimeout
        {
            get
            {
                if (configAdapter == null)
                {
                    throw new InvalidOperationException("ConfigProvider is not initialized. Call Initialize() with a valid IConfigAdapter.");
                }
                return configAdapter.DefaultTimeout;
            }
        }
        #endregion

        // Public methods
        /// <summary>
        /// Initializes the ConfigProvider with the specified IConfigAdapter.
        /// </summary>
        /// <param name="adapter">The IConfigAdapter to use for configuration settings.</param>
        /// <exception cref="ArgumentNullException">Throws if the adapter is null.</exception>
        public static void Initialize(IConfigAdapter adapter)
        {
            configAdapter ??= adapter ?? throw new ArgumentNullException(nameof(adapter), "Config adapter cannot be null.");
        }
    }
}
