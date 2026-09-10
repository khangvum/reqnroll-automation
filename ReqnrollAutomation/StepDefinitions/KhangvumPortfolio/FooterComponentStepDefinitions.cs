

using ReqnrollAutomation.Models.KhangvumPortfolio;
using ReqnrollAutomation.Pages.CarfaxCanadaWebsite;

/**
 * Program:         FooterComponentStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-09-09
 * Description:     A class that defines the step definitions for the footer component verification feature on the Khangvum Portfolio website.
 */

namespace ReqnrollAutomation.StepDefinitions.KhangvumPortfolio
{
    /// <summary>
    /// A class that defines the step definitions for the footer component verification feature on the Khangvum Portfolio website.
    /// </summary>
    [Binding]
    public class FooterComponentStepDefinitions : KhangvumPortfolioBaseStepDefinitions
    {
        #region Constructor
        public FooterComponentStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region Given Steps
        [Given(@"the Khangvum Portfolio website footer contains links to social media pages")]
        public void GivenTheKhangvumPortfolioWebsiteFooterContainsLinksToSocialMediaPages()
        {
            PortfolioPage.ClickHeaderLink("Contact");
        }
        #endregion


        #region Then Steps
        [Then(@"all Khangvum Portfolio website social media links should navigate to their expected destinations")]
        public void ThenAllKhangvumPortfolioWebsiteSocialMediaLinksShouldNavigateToTheirExpectedDestinations()
        {
            foreach (SocialMediaLink socialMediaLink in PortfolioPage.SocialMediaLinks)
            {
                // Skip Gmail link verification as it opens the default email client instead of a web page
                if (socialMediaLink.ExpectedUrl.StartsWith("mailto:"))
                {
                    continue;
                }

                // Click the social media link & switch to the new tab
                PortfolioPage.ClickFooterSocialMediaButton(socialMediaLink.Platform);
                PortfolioPage.SwitchToNewTab();

                // Verify if the new tab navigates to the expected URL
                string actualUrl = PortfolioPage.WaitForUrlToStabilize();
                Assert.AreEqual(socialMediaLink.ExpectedUrl, actualUrl, $"The Khangvum Portfolio {socialMediaLink.Platform} link did not navigate to the expected URL.");

                // Close the new tab and switch back to the original tab
                PortfolioPage.CloseCurrentTabAndSwitchBackToOriginalTab();
            }
        }
        #endregion
    }
}
