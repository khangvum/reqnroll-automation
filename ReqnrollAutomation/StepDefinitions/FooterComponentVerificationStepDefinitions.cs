/**
 * Program:         FooterComponentVerificationStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-14
 * Description:     A class that defines the step definitions for the footer component verification feature.
 */

namespace ReqnrollAutomation.StepDefinitions
{
    /// <summary>
    /// A class that defines the step definitions for the footer component verification feature.
    /// </summary>
    [Binding]
    public class FooterComponentVerificationStepDefinitions : BaseStepDefinitions
    {
        #region Registry
        private enum ValidationKey
        {
            CopyrightText
        }

        private readonly Dictionary<ValidationKey, string> _registry;
        #endregion

        #region Constructor
        public FooterComponentVerificationStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
            _registry = new()
            {
                { ValidationKey.CopyrightText, "© 2026 Sauce Labs. All Rights Reserved." }
            };
        }
        #endregion

        #region Step Definitions
        // Then Steps
        [Then("the footer copyright text should be visible")]
        public void ThenTheFooterCopyrightTextShouldBeVisible()
        {
            // Scroll to the footer section to ensure it is visible on the screen
            InventoryPage.ScrollToFooter();

            // Verify if the footer copyright text is visible
            Assert.IsTrue(InventoryPage.IsCopyrightTextVisible(), "The footer copyright text is not visible.");
        }

        [Then("the footer copyright text should display correctly")]
        public void ThenTheFooterCopyrightTextShouldDisplayCorrectly()
        {
            // Get the actual copyright text from the page
            string actualCopyrightText = InventoryPage.GetCopyrightText();

            // Get the expected copyright text from the registry
            string expectedCopyrightText = _registry[ValidationKey.CopyrightText];

            // Verify if the footer copyright text is displayed correctly
            Assert.Contains(expectedCopyrightText, actualCopyrightText, "The footer copyright text is not displayed correctly.");
        }

        [Then("the footer should contain a link to {string}")]
        public void ThenTheFooterShouldContainALinkTo(string platform)
        {
            // Scroll to the footer section to ensure it is visible on the screen
            InventoryPage.ScrollToFooter();

            // Verify if the footer contains a link to the specified social media platform
            bool isVisible = InventoryPage.IsSocialMediaLinkVisible(platform);
            Assert.IsTrue(isVisible, $"The footer does not contain a link to {platform}.");
        }

        [Then("the {string} link should navigate to {string}")]
        public void ThenTheSocialMediaLinkShouldNavigateTo(string platform, string expectedUrl)
        {
            // Click the social media link & switch to the new tab
            InventoryPage.ClickSocialMediaLink(platform);
            InventoryPage.SwitchToNewTab();

            // Verify if the new tab navigates to the expected URL
            string actualUrl = Driver.Url;
            Assert.AreEqual(expectedUrl, actualUrl, $"The {platform} link did not navigate to the expected URL.");
        }
        #endregion
    }
}
