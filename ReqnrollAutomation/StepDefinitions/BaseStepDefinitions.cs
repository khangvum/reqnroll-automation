/**
 * Program:         BaseStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-11
 * Description:     A class that defines base step definitions for the automation framework.
 */

using ReqnrollAutomation.Core.Extensions;

namespace ReqnrollAutomation.StepDefinitions
{
    public class BaseStepDefinitions
    {
        #region Private Attributes
        // Driver & Contexts
        private IWebDriver? _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        #endregion

        #region Public Properties
        /// <summary>
        /// Lazy initialization of the WebDriver instance from the ScenarioContext.
        /// </summary>
        public IWebDriver Driver
        {
            get
            {
                _driver ??= _scenarioContext.GetDriver();
                return _driver;
            }
        }
        #endregion

        #region Constructor
        public BaseStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            _featureContext = featureContext ?? throw new ArgumentNullException(nameof(featureContext));
        }
        #endregion
    }
}
