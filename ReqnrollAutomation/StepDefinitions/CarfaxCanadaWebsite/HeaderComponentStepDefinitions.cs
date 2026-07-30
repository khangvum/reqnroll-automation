/**
 * Program:         HeaderStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-07-21
 * Description:     A class that defines the step definitions for the header component verification feature on CARFAX Canada website.
 */

namespace ReqnrollAutomation.StepDefinitions.CarfaxCanadaWebsite
{
    /// <summary>
    /// A class that defines the step definitions for the header component verification feature on CARFAX Canada website.
    /// </summary>
    [Binding]
    public class HeaderComponentStepDefinitions : CarfaxCanadaBaseStepDefinitions
    {
        #region Constructor
        public HeaderComponentStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region Given Steps
        [Given(@"I am on a random CARFAX Canada subpage")]
        public void GivenIAmOnARandomCarfaxCanadaSubpage()
        {
            // Pick a random URL from the filtered list of subpage links
            string randomSubpageUrl = HomePage.InternalSubpageLinks[Random.Shared.Next(HomePage.InternalSubpageLinks.Count)];

            // Navigate to the random subpage
            Driver.Navigate().GoToUrl(randomSubpageUrl);
        }
        #endregion

        #region When Steps
        [When(@"I click on the CARFAX Canada logo")]
        public void WhenIClickOnTheCarfaxCanadaLogo()
        {
            HomePage.ClickHeaderLogo();
        }
        #endregion

        #region Then Steps
        [Then(@"all header links should navigate to their expected destinations")]
        public void ThenAllHeaderLinksShouldNavigateToTheirExpectedDestinations()
        {
            foreach ((string section, string subSection, string expectedUrl) in HomePage.HeaderLinks)
            {
                // Hover over the section to reveal the subsections
                HomePage.HoverHeaderSection(section);

                // Check if the subsection will open in a new tab (target="_blank") before clicking
                bool opensInNewTab = HomePage.GetHeaderSubsectionLinkTarget(subSection) == "_blank";
                HomePage.ClickHeaderSubsection(subSection);

                // Switch to the new tab if the link opens in a new tab (target="_blank")
                if (opensInNewTab)
                    HomePage.SwitchToNewTab();

                // Verify if the it navigates to the expected URL
                string actualUrl = HomePage.WaitForUrlToStabilize();
                Assert.AreEqual(expectedUrl, actualUrl, $"The {subSection} link did not navigate to the expected URL.");

                // Close the new tab and switch back to the original tab if it opened in a new tab
                if (opensInNewTab)
                    HomePage.CloseCurrentTabAndSwitchBackToOriginalTab();
            }
        }

        [Then(@"I should be redirected to the CARFAX Canada home page")]
        public void ThenIShouldBeRedirectedToTheCarfaxCanadaHomePage()
        {
            string expectedUrl = HomePage.PageUrl;
            string actualUrl = HomePage.WaitForUrlToStabilize();
            Assert.AreEqual(expectedUrl, actualUrl, "Clicking the CARFAX Canada logo did not redirect to the home page.");
        }
        #endregion
    }
}
