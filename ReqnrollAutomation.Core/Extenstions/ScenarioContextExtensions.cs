/**
 * Program:         ScenarioContextExtensions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-12
 * Description:     A class that contains extension methods for the ScenarioContext class.
 */

using OpenQA.Selenium;
using Reqnroll;

namespace ReqnrollAutomation.Core.Extensions
{
    /// <summary>
    /// A class that contains extension methods for the ScenarioContext class.
    /// </summary>
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

        /// <summary>
        /// Sets the WebDriver instance in the current scenario context.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <param name="driver">The WebDriver instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if the WebDriver instance is null.</exception>
        public static void SetDriver(this ScenarioContext scenarioContext, IWebDriver driver)
        {
            scenarioContext["WebDriver"] = driver ?? throw new ArgumentNullException(nameof(driver), "WebDriver instance cannot be null.");
        }
    }
}