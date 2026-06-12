/**
 * Program:         CredentialManager.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-10
 * Description:     A class that handles parsing and retrieving the credentials for Swag Labs from credentials.json file.
 */

using Microsoft.Extensions.Configuration;
using ReqnrollAutomation.Models;

namespace ReqnrollAutomation.Helpers
{
    /// <summary>
    /// A class that handles parsing and retrieving the credentials for Swag Labs from credentials.json file.
    /// </summary>
    internal static class CredentialManager
    {
        // Private attributes
        private static readonly Lazy<SwagLabsCredentials> _credentials;

        // Public properties
        public static SwagLabsCredentials Credentials => _credentials.Value;

        // Constructor
        static CredentialManager()
        {
            _credentials = new(LoadCredentials);
        }

        // Private helper methods
        private static SwagLabsCredentials LoadCredentials()
        {
            try
            {
                // Determine the path to the credentials.json file on the project root directory
                string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
                string jsonPath = Path.Combine(projectRoot, "credentials.json");

                if (!File.Exists(jsonPath))
                {
                    throw new FileNotFoundException($"[ERROR] Could not find credentials.json at project root: {jsonPath}");
                }

                // Build the configuration from the credentials.json file
                IConfigurationRoot configEngine = new ConfigurationBuilder()
                    .SetBasePath(projectRoot)
                    .AddJsonFile("credentials.json", optional: false, reloadOnChange: true)
                    .Build();

                SwagLabsCredentials credentials = new();
                configEngine.GetSection("SwagLabsAccounts").Bind(credentials);

                return credentials;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"[ERROR] Failed to load credentials: {ex.Message}", ex);
            }
        }
    }
}
