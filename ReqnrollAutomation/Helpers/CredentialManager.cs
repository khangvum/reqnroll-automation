/**
 * Program:         CredentialManager.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-10
 * Description:     A class that handles parsing and retrieving the credentials for Swag Labs from credentials.json file.
 */

using Microsoft.Extensions.Configuration;
using ReqnrollAutomation.Models;
using System.Text;

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
        private static SwagLabsCredentials Credentials => _credentials.Value;

        // Constructor
        static CredentialManager()
        {
            _credentials = new(LoadCredentials);
        }

        // Public methods
        /// <summary>
        /// Retrieves the username associated with the specified account type from the credentials.
        /// </summary>
        /// <param name="accountKey">The account type for which to retrieve the username.</param>
        /// <returns>The username associated with the specified account type.</returns>
        /// <exception cref="KeyNotFoundException">Throws when the specified account type is not found in the credentials.</exception>
        public static string GetUsername(string accountKey) =>
            Credentials.Accounts.TryGetValue(accountKey, out string? username) && !string.IsNullOrEmpty(username)
                ? username
                : throw new KeyNotFoundException($"[ERROR] Account type '{accountKey}' not found in credentials.");

        /// <summary>
        /// Retrieves the shared password from the credentials.
        /// </summary>
        /// <returns>The shared password.</returns>
        public static string GetSharedPassword() => Credentials.SharedPassword;

        /// <summary>
        /// Normalizes the account type string to a standard key format (e.g., "standard user" becomes "StandardUser").
        /// </summary>
        /// <param name="accountType">The account type string to normalize.</param>
        /// <returns>The normalized account type string.</returns>
        public static string NormalizeAccountType(string accountType)
        {
            string[] words = accountType.Split([' ', '-'], StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new();

            foreach (var word in words)
            {
                result.Append(char.ToUpperInvariant(word[0]));
                result.Append(word[1..].ToLower());
            }

            return result.Append("User").ToString();
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
                configEngine.GetSection(nameof(SwagLabsCredentials)).Bind(credentials);

                return credentials;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"[ERROR] Failed to load credentials: {ex.Message}", ex);
            }
        }
    }
}
