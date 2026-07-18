/**
 * Program:         CarfaxCanadaBaseStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-07-18
 * Description:     A class that defines base step definitions for the automation framework for CARFAX Canada.
 */

using ReqnrollAutomation.Pages.CarfaxCanada;

namespace ReqnrollAutomation.StepDefinitions.CarfaxCanada
{
    public class CarfaxCanadaBaseStepDefinitions : BaseStepDefinitions
    {
        #region Private Attributes
        // Pages
        private HomePage? _homePage;
        #endregion

        #region Public Properties
        /// <summary>
        /// Lazy initialization of the LoginPage instance.
        /// </summary>
        public HomePage HomePage
        {
            get
            {
                _homePage ??= new(Driver);
                return _homePage;
            }
        }
        #endregion

        #region Constructor
        public CarfaxCanadaBaseStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion
    }
}
