namespace ReqnrollAutomation.StepDefinitions.CarfaxCanadaWebsite
{
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
            foreach ((string section, string subSection, string expectedUrl) in HomePage.FooterLinks)
            {
                // Scroll to the footer section to ensure it is visible on the screen
                HomePage.ScrollToFooter();

                // Check if the subsection will open in a new tab (target="_blank") before clicking
                bool opensInNewTab = HomePage.GetFooterSubsectionLinkTarget(subSection) == "_blank";
                HomePage.ClickFooterSubsection(subSection);

                // Switch to the new tab if the link opens in a new tab (target="_blank")
                if (opensInNewTab)
                {
                    HomePage.SwitchToNewTab();
                }

                // Verify if the it navigates to the expected URL
                string actualUrl = HomePage.WaitForUrlToStabilize();
                Assert.Contains(expectedUrl, actualUrl, "The {subSection} link did not navigate to the expected URL.");

                // Close the new tab and switch back to the original tab if it opened in a new tab
                if (opensInNewTab)
                    HomePage.CloseCurrentTabAndSwitchBackToOriginalTab();
            }
        }
        #endregion
    }
}
