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
        #region Constructor
        public HeaderStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region When Steps
        [When(@"I hover over the {} section in the header")]
        public void WhenIHoverOverTheSectionInTheHeader(string sectionName)
        {
            HomePage.HoverHeaderSection(sectionName);
        }

        [When(@"I click on the {} subsection in the header")]
        public void WhenIClickOnTheSubsectionInTheHeader(string subSectionName)
        {
            // Check if the subsection will open in a new tab (target="_blank") before clicking
            bool opensInNewTab = HomePage.GetHeaderSubsectionLinkTarget(subSectionName) == "_blank";

            // Click on the subsection
            HomePage.ClickHeaderSubsection(subSectionName);

            // Switch to the new tab if the link opens in a new tab (target="_blank")
            if (opensInNewTab)
            {
                HomePage.SwitchToNewTab();
            }
        }
        #endregion
    }
}
