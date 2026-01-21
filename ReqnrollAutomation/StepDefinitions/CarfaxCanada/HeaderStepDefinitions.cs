

using Microsoft.CodeAnalysis;

/**
 * Program:         HeaderStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-07-21
 * Description:     A class that defines the step definitions for the header component verification feature on CARFAX Canada website.
 */

namespace ReqnrollAutomation.StepDefinitions.CarfaxCanada
{
    /// <summary>
    /// A class that defines the step definitions for the header component verification feature on CARFAX Canada website.
    /// </summary>
    [Binding]
    public class HeaderStepDefinitions : CarfaxCanadaBaseStepDefinitions
    {
        public static readonly List<(string Section, string SubSection, string ExpectedUrl)> HeaderLinks =
        [
            ("Vehicle History", "Vehicle History Reports", "https://www.carfax.ca/vehicle-history/vehicle-history-report"),
            ("Vehicle History", "View a Sample Report", "https://www.carfax.ca/vehicle-history/sample-report"),
            ("Vehicle Fraud", "What is VIN Fraud?", "https://www.carfax.ca/what-is-vin-fraud"),
            ("Vehicle Fraud", "VIN Fraud Check", "https://www.carfax.ca/vin-fraud-check"),
            ("Vehicle Fraud", "Vehicle Monitoring Subscription", "https://www.carfax.ca/vehicle-monitoring-subscription"),
            ("What’s My Car Worth", "Car Value", "https://www.carfax.ca/whats-my-car-worth/car-value/ymm"),
            ("What’s My Car Worth", "History Based Value", "https://www.carfax.ca/whats-my-car-worth/history-based-value"),
            ("Tools", "VIN Decoder", "https://www.carfax.ca/tools/vin-decode"),
            ("Tools", "Recall Check", "https://www.carfax.ca/tools/recall-check"),
            ("Tools", "Car Care", "https://www.carfax.ca/Service"),
            ("Resources", "Learn", "https://www.carfax.ca/learn"),
            ("Resources", "Support", "https://support.carfax.ca/en/support/home")
        ];

        #region Constructor
        public HeaderStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region Given Steps
        [Given(@"I am on a random CARFAX Canada subpage")]
        public void GivenIAmOnARandomCarfaxCanadaSubpage()
        {
            // Filter out Support link from the list of header links
            // since it is not a subpage of www.carfax.ca
            List<string> subpageLinks = HeaderLinks
                .Where(link => link.SubSection != "Support")
                .Select(link => link.ExpectedUrl)
                .ToList();

            // Pick a random URL from the filtered list of subpage links
            string randomSubpageUrl = subpageLinks[Random.Shared.Next(subpageLinks.Count)];

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
            foreach ((string section, string subSection, string expectedUrl) in HeaderLinks)
            {
                // Hover over the section to reveal the subsections
                HomePage.HoverHeaderSection(section);

                // Check if the subsection will open in a new tab (target="_blank") before clicking
                bool opensInNewTab = HomePage.GetHeaderSubsectionLinkTarget(subSection) == "_blank";
                HomePage.ClickHeaderSubsection(subSection);

                // Switch to the new tab if the link opens in a new tab (target="_blank")
                if (opensInNewTab)
                {
                    HomePage.SwitchToNewTab();
                }

                // Verify if the it navigates to the expected URL
                string actualUrl = HomePage.WaitForUrlToStabilize();
                Assert.AreEqual(expectedUrl, actualUrl, $"The {subSection} link did not navigate to the expected URL.");

                // Close the new tab and switch back to the original tab if it opened in a new tab
                if (opensInNewTab)
                {
                    HomePage.CloseCurrentTabAndSwitchBackToOriginalTab();
                }
            }
        }
        #endregion
    }
}
