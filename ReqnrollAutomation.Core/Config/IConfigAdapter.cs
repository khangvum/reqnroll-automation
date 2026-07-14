/**
 * Program:         IConfigAdapter.cs
 * Author:          Manh Khang Vu
 * Date:            2026-07-11
 * Description:     An interface for adapting configuration settings for the 
 *                  Reqnroll automation framework from external sources.
 */

namespace ReqnrollAutomation.Core.Config
{
    /// <summary>
    /// An interface for adapting configuration settings for the
    /// Reqnroll automation framework from external sources.
    /// </summary>
    public interface IConfigAdapter
    {
        /// <summary>
        /// Gets the configured test environment.
        /// </summary>
        string TestEnvironment { get; }

        /// <summary>
        /// Gets the configured browser type.
        /// </summary>
        BrowserType Browser { get; }
    }
}
