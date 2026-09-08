/**
 * Program:         KhangvumPortfolioBaseStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-09-09
 * Description:     A class that defines base step definitions for the automation framework for Khangvum Portfolio.
 */

using ReqnrollAutomation.Pages.KhangvumPortfolio;

namespace ReqnrollAutomation.StepDefinitions.KhangvumPortfolio
{
    public class KhangvumPortfolioBaseStepDefinitions : BaseStepDefinitions
    {
        #region Private Attributes
        // Pages
        private PortfolioPage? _portfolioPage;
        #endregion

        #region Public Properties
        /// <summary>
        /// Lazy initialization of the LoginPage instance.
        /// </summary>
        public PortfolioPage PortfolioPage
        {
            get
            {
                _portfolioPage ??= new(Driver);
                return _portfolioPage;
            }
        }
        #endregion

        #region Constructor
        public KhangvumPortfolioBaseStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion
    }
}
