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
        #region Registry
        // Registry
        private enum ValidationKey
        {
            AccessibilityTheme
        }

        private readonly Dictionary<ValidationKey, string> _registry;

        // Scenario Context Keys
        private const string InitialColorKey = "InitialMainHeadingColor";
        #endregion

        #region Constructor
        public AccessibilityStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
            _registry = new()
            {
                { ValidationKey.AccessibilityTheme, "high-contrast" }
            };
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
        [When(@"I click the language toggle")]
        public void WhenIClickOnTheLanguageToggle()
        {
            HomePage.ToggleLanguage();
        }

        [When(@"I click the accessibility toggle")]
        public void WhenIClickOnTheAccessibilityToggle()
        {
            _scenarioContext[InitialColorKey] = HomePage.GetMainHeadingColor();
            HomePage.ToggleAccessibility();
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

        [Then(@"the theme should be set to high contrast")]
        public void ThenTheThemeShouldBeSetToHighContrast()
        {
            string currentTheme = HomePage.GetCurrentTheme();
            Assert.AreEqual(_registry[ValidationKey.AccessibilityTheme], currentTheme, $"Expected the theme to be set to 'high-contrast', but the current theme is '{currentTheme}'.");
        }

        [Then(@"the main heading color should change")]
        public void ThenTheMainHeadingColorShouldChange()
        {
            string initialColor = _scenarioContext[InitialColorKey] as string ?? throw new InvalidOperationException("Initial main heading color is not stored in the scenario context.");
            string currentColor = HomePage.GetMainHeadingColor();

            // Check if the main heading color has changed
            Assert.AreNotEqual(initialColor, currentColor, $"Expected the main heading color to change from '{initialColor}', but it is still '{currentColor}'.");
        }
        #endregion
    }
}
