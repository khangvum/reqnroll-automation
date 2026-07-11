/**
 * Program:         ConfigAdapter.cs
 * Author:          Manh Khang Vu
 * Date:            2026-07-11
 * Description:     An interface for adapting configuration settings for the 
 *                  Reqnroll automation framework from external sources.
 */

using ReqnrollAutomation.Core.Config;

namespace ReqnrollAutomation.Config
{
    /// <summary>
    /// An interface for adapting configuration settings for the 
    /// Reqnroll automation framework from external sources.
    /// </summary>
    internal class ConfigAdapter : IConfigAdapter
    {
        /// <inheritdoc/>
        public string Environment => ConfigManager.Environment;

        /// <inheritdoc/>
        public BrowserType Browser => ConfigManager.Browser;
    }
}
