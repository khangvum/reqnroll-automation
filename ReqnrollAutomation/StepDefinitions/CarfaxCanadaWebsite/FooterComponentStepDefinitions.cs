/**
 * Program:         FooterComponentStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-08-15
 * Description:     A class that defines the step definitions for the footer component on CARFAX Canada website.
 */

using ReqnrollAutomation.Models.CarfaxCanadaWebsite;

namespace ReqnrollAutomation.StepDefinitions.CarfaxCanadaWebsite
{
    /// <summary>
    /// A class that defines the step definitions for the footer component on CARFAX Canada website.
    /// </summary>
    [Binding]
    public class FooterComponentStepDefinitions : CarfaxCanadaBaseStepDefinitions
    {
        #region Registry
        private enum ValidationKey
        {
            DisclaimerText
        }

        private readonly Dictionary<ValidationKey, string> _registry;
        #endregion

        #region Constructor
        public FooterComponentStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
            _registry = new()
            {
                { ValidationKey.DisclaimerText, "CARFAX Canada Vehicle History Reports are based only on information supplied to CARFAX Canada and available as of the date and time a Vehicle History Report is generated. Other information about the vehicle, including problems, may not have been reported to CARFAX Canada. Use the Vehicle History Report as one important tool, along with a vehicle inspection and test drive, to make a better decision about your next used car." }
            };
        }
        #endregion

        #region Given Steps
        [Given(@"the CARFAX Canada website footer contains links to social media pages")]
        public void GivenTheCarfaxCanadaWebsiteFooterContainsLinksToSocialMediaPages()
        {
            // Scroll to the footer section to ensure it is visible on the screen
            HomePage.ScrollToFooter();

            // Verify if the footer contains the social media section
            bool isVisible = HomePage.IsSocialMediaSectionVisible();
            if (!isVisible)
            {
                throw new InvalidOperationException("Pre-condition Failed: The footer does not contain a social media section.");
            }
        }

        [Given(@"the CARFAX Canada website footer contains utility links")]
        public void GivenTheCarfaxCanadaWebsiteFooterContainsUtilityLinks()
        {
            // Scroll to the footer section to ensure it is visible on the screen
            HomePage.ScrollToFooter();

            // Verify if the footer contains the utility links section
            bool isVisible = HomePage.IsUtilityLinksSectionVisible();
            if (!isVisible)
            {
                throw new InvalidOperationException("Pre-condition Failed: The footer does not contain a utility links section.");
            }
        }
        #endregion

        #region Then Steps
        [Then(@"the CARFAX Canada website disclaimer text should be visible")]
        public void ThenTheCarfaxCanadaWebsiteDisclaimerTextShouldBeVisible()
        {
            // Scroll to the footer section to ensure it is visible on the screen
            HomePage.ScrollToFooter();

            // Verify if the footer disclaimer text is visible
            Assert.IsTrue(HomePage.IsDisclaimerTextVisible(), "The footer disclaimer text is not visible.");
        }

        [Then(@"the CARFAX Canada website disclaimer text should display correctly")]
        public void ThenTheCarfaxCanadaWebsiteDisclaimerTextShouldDisplayCorrectly()
        {
            // Get the actual disclaimer text from the page
            string actualDisclaimerText = HomePage.GetDisclaimerText();

            // Get the expected disclaimer text from the registry
            string expectedDisclaimerText = _registry[ValidationKey.DisclaimerText];

            // Verify if the actual disclaimer text matches the expected text
            Assert.AreEqual(expectedDisclaimerText, actualDisclaimerText, "The footer disclaimer text does not match the expected text.");
        }

        [Then(@"all footer links should navigate to their expected destinations")]
        public void ThenAllFooterLinksShouldNavigateToTheirExpectedDestinations()
        {
            foreach (NavigationLink footerLink in HomePage.FooterLinks)
            {
                // Scroll to the footer section to ensure it is visible on the screen
                HomePage.ScrollToFooter();

                // Check if the subsection will open in a new tab (target="_blank") before clicking
                bool opensInNewTab = HomePage.GetFooterSubsectionLinkTarget(footerLink.SubSection) == "_blank";
                HomePage.ClickFooterSubsection(footerLink.SubSection);

                // Switch to the new tab if the link opens in a new tab (target="_blank")
                if (opensInNewTab)
                {
                    HomePage.SwitchToNewTab();
                }

                // Verify if the it navigates to the expected URL
                string actualUrl = HomePage.WaitForUrlToStabilize();
                Assert.Contains(footerLink.ExpectedUrl, actualUrl, $"The {footerLink.SubSection} link did not navigate to the expected URL.");

                // Close the new tab and switch back to the original tab if it opened in a new tab
                if (opensInNewTab)
                    HomePage.CloseCurrentTabAndSwitchBackToOriginalTab();
            }
        }

        [Then(@"all CARFAX Canada website social media links should navigate to their expected destinations")]
        public void ThenAllCarfaxCanadaWebsiteSocialMediaLinksShouldNavigateToTheirExpectedDestinations()
        {
            foreach (SocialMediaLink socialMediaLink in HomePage.SocialMediaLinks)
            {
                // Click the social media link & switch to the new tab
                HomePage.ClickSocialMediaLink(socialMediaLink.Platform);
                HomePage.SwitchToNewTab();

                // Verify if the new tab navigates to the expected URL
                string actualUrl = HomePage.WaitForUrlToStabilize();
                Assert.AreEqual(socialMediaLink.ExpectedUrl, actualUrl, $"The CARFAX Canada {socialMediaLink.Platform} link did not navigate to the expected URL.");

                // Close the new tab and switch back to the original tab
                HomePage.CloseCurrentTabAndSwitchBackToOriginalTab();
            }
        }

        [Then(@"all CARFAX Canada website footer utility links should navigate to their expected destinations")]
        public void ThenAllCarfaxCanadaWebsiteFooterUtilityLinksShouldNavigateToTheirExpectedDestinations()
        {
            foreach (UtilityLink utilityLink in HomePage.UtilityLinks)
            {
                // Scroll to the footer section to ensure it is visible on the screen
                HomePage.ScrollToFooter();

                // Click the utility link
                HomePage.ClickFooterUtilityLink(utilityLink.UtilityItem);

                // Verify if the it navigates to the expected URL
                string actualUrl = HomePage.WaitForUrlToStabilize();
                if (utilityLink.ExpectedUrl.EndsWith("#"))
                {
                    Assert.EndsWith("#", actualUrl, $"The {utilityLink.UtilityItem} utility link did not navigate to the expected URL.");
                }
                else
                {
                    Assert.Contains(utilityLink.ExpectedUrl, actualUrl, $"The {utilityLink.UtilityItem} utility link did not navigate to the expected URL.");
                }
            }
        }
        #endregion
    }
}
