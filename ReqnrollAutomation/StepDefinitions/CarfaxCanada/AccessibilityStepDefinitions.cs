/**
 * Program:         AccessibilityStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-07-18
 * Description:     A class that defines the step definitions for the accessibility feature on CARFAX Canada website.
 */

namespace ReqnrollAutomation.StepDefinitions.CarfaxCanada
{
    /// <summary>
    /// A class that defines the step definitions for the accessibility feature on CARFAX Canada website.
    /// </summary>
    [Binding]
    public class AccessibilityStepDefinitions : CarfaxCanadaBaseStepDefinitions
    {
        #region Constructor
        public AccessibilityStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region Given Steps
        [Given(@"I am on the CARFAX Canada home page")]
        public void GivenIAmOnTheCarfaxCanadaHomePage()
        {
            HomePage.Navigate();
        }

        [Given(@"the home page is displayed in {}")]
        public void GivenTheHomePageIsDisplayedIn(string initialLanguage)
        {
            string initialLanguageAbbr = initialLanguage[..2].ToLower();
            string currentLanguageAbbr = HomePage.GetCurrentLanguageCode();

            // If the current language does not match the initial language, toggle the language to switch to the desired language
            if (initialLanguageAbbr != currentLanguageAbbr)
            {
                HomePage.ToggleLanguage();
            }
        }
        #endregion

        #region When Steps
        [When(@"I click on the language toggle")]
        public void WhenIClickOnTheLanguageToggle()
        {
            HomePage.ToggleLanguage();
        }
        #endregion

        #region Then Steps
        [Then(@"the home page should switch to {}")]
        public void ThenTheHomePageShouldSwitchTo(string resultingLanguage)
        {
            string resultingLanguageAbbr = resultingLanguage[..2].ToLower();
            string currentLanguageAbbr = HomePage.GetCurrentLanguageCode();

            // Check if the current language matches the expected resulting language
            Assert.AreEqual(resultingLanguageAbbr, currentLanguageAbbr, $"Expected the home page to switch to {resultingLanguage}, but the 'lang' attribute is currently set to {currentLanguageAbbr}.");
        }

        [Then(@"the main heading should be {string}")]
        public void ThenTheMainHeadingShouldBe(string mainHeading)
        {
            string currentMainHeading = HomePage.GetMainHeadingText();
            Assert.AreEqual(mainHeading, currentMainHeading, $"Expected the main heading to be '{mainHeading}', but the current main heading is '{currentMainHeading}'.");
        }
        #endregion
    }
}
