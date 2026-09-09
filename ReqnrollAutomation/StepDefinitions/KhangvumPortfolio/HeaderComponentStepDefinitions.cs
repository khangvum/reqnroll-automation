/**
 * Program:         HeaderStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-09-09
 * Description:     A class that defines the step definitions for the header component verification feature on the Khangvum Portfolio website.
 */

using ReqnrollAutomation.Models.KhangvumPortfolio;
using ReqnrollAutomation.Pages.CarfaxCanadaWebsite;

namespace ReqnrollAutomation.StepDefinitions.KhangvumPortfolio
{
    /// <summary>
    /// A class that defines the step definitions for the header component verification feature on the Khangvum Portfolio website.
    /// </summary>
    [Binding]
    public class HeaderComponentStepDefinitions : KhangvumPortfolioBaseStepDefinitions
    {
        #region Constructor
        public HeaderComponentStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region Given Steps
        [Given(@"I am on the Khangvum Portfolio page")]
        public void GivenIAmOnTheKhangvumPortfolioPage()
        {
            PortfolioPage.Navigate();
        }
        #endregion

        #region Then Steps
        [Then(@"all header links should navigate to their expected sections")]
        public void ThenAllHeaderLinksShouldNavigateToTheirExpectedSections()
        {
            foreach (HeaderLink headerLink in PortfolioPage.HeaderLinks)
            {
                // Click on the header link
                PortfolioPage.ClickHeaderLink(headerLink.Section);

                // Verify the current URL matches the expected URL
                string actualUrl = PortfolioPage.WaitForUrlToStabilize();
                Assert.AreEqual(headerLink.ExpectedUrl, actualUrl, $"The URL for the '{headerLink.Section}' link does not match the expected URL.");

                // Check if the section is the viewport
                Assert.IsTrue(PortfolioPage.IsSectionInViewport(headerLink.Section), $"The '{headerLink.Section}' section is not in the viewport after clicking the link.");
            }
        }
        #endregion
    }
}
