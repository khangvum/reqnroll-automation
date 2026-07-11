/**
 * Program:         ConfigManager.cs
 * Author:          Manh Khang Vu
 * Date:            2026-05-26
 * Description:     A class that manages the configuration settings for the Reqnroll automation framework.
 */

using Microsoft.Extensions.Configuration;

namespace ReqnrollAutomation.Config
{
    /// <summary>
    /// A class that manages the configuration settings for the Reqnroll automation framework.
    /// </summary>
    internal static class ConfigManager
    {
        // Private attributes
        private static readonly IConfigurationRoot _configEngine;
        private const string ConfigFileName = "config.json";

        #region Public Properties
        /// <summary>
        /// Gets the configured environment, defaulting to "QA" if not specified.
        /// </summary>
        public static string Environment => GetValue(nameof(Environment), "QA");

        /// <summary>
        /// Gets the default timeout value in seconds, defaulting to 30 if not specified.
        /// </summary>
        public static int DefaultTimeout => GetValue(nameof(DefaultTimeout), 30);
        #endregion

        // Constructor
        static ConfigManager()
        {
            // Determine the path to the Config\config.json file on the project root directory
            string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            string configDirectory = Path.Combine(projectRoot, "Config");
            string jsonPath = Path.Combine(configDirectory, ConfigFileName);

            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException($"[ERROR] Could not find {ConfigFileName} at: {jsonPath}");
            }

            // Build the configuration from the config.json file
            _configEngine = new ConfigurationBuilder()
                .SetBasePath(configDirectory)
                .AddJsonFile(ConfigFileName, optional: false, reloadOnChange: true)
                .Build();
        }

        // Public methods
        /// <summary>
        /// Retrieves a configuration value of type T from the configuration settings using the specified key or path.
        /// </summary>
        /// <typeparam name="T">The type of the configuration value.</typeparam>
        /// <param name="keyOrPath">The key or path of the configuration value.</param>
        /// <param name="defaultValue">The default value to return if the configuration value is not found.</param>
        /// <returns>The configuration value or the default value if not found.</returns>
        public static T GetValue<T>(string keyOrPath, T defaultValue = default!)
        {
            try
            {
                IConfigurationSection section = _configEngine.GetSection(keyOrPath);

                if (!section.Exists())
                {
                    return defaultValue;
                }

                return section.Get<T>() ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
