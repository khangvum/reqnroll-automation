/**
 * Program:         FooterComponentVerificationStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-14
 * Description:     A class that defines the step definitions for the footer component verification feature on Swag Labs website.
 */

using System.Text.RegularExpressions;

namespace ReqnrollAutomation.StepDefinitions.SwagLabs
{
    /// <summary>
    /// A class that defines the step definitions for the footer component verification feature on Swag Labs website.
    /// </summary>
    [Binding]
    public class FooterComponentStepDefinitions : SwagLabsBaseStepDefinitions
    {
        #region Registry
        private enum ValidationKey
        {
            CopyrightPattern
        }

        private readonly Dictionary<ValidationKey, string> _registry;
        #endregion

        #region Constructor
        public FooterComponentStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
            _registry = new()
            {
                { ValidationKey.CopyrightPattern, @"© \d{4} Sauce Labs\. All Rights Reserved\." }
            };
        }
        #endregion

        #region Given Steps
        [Given(@"the Swag Labs footer contains a link to {} social media page")]
        public void GivenTheFooterShouldContainALinkToSocialMediaPage(string platform)
        {
            // Scroll to the footer section to ensure it is visible on the screen
            InventoryPage.ScrollToFooter();

            // Verify if the footer contains a link to the specified social media platform
            bool isVisible = InventoryPage.IsSocialMediaLinkVisible(platform);
            if (!isVisible)
            {
                throw new InvalidOperationException($"Pre-condition Failed: The footer does not contain a link to {platform}");
            }
        }
        #endregion

        #region When Steps
        [When(@"I click on the {} social media link")]
        public void WhenIClickOnTheSocialMediaLink(string platform)
        {
            // Click the social media link & switch to the new tab
            InventoryPage.ClickSocialMediaLink(platform);
            InventoryPage.SwitchToNewTab();
        }
        #endregion

        #region Then Steps
        // Then Steps
        [Then(@"the Swag Labs footer copyright text should be visible")]
        public void ThenTheFooterCopyrightTextShouldBeVisible()
        {
            // Scroll to the footer section to ensure it is visible on the screen
            InventoryPage.ScrollToFooter();

            // Verify if the footer copyright text is visible
            Assert.IsTrue(InventoryPage.IsCopyrightTextVisible(), "The footer copyright text is not visible.");
        }

        [Then(@"the Swag Labs footer copyright text should display correctly")]
        public void ThenTheFooterCopyrightTextShouldDisplayCorrectly()
        {
            // Get the actual copyright text from the page
            string actualCopyrightText = InventoryPage.GetCopyrightText();

            // Define a regex pattern that matches any 4-digit year
            string copyrightPattern = _registry[ValidationKey.CopyrightPattern];

            // Verify if the footer copyright text is displayed correctly
            Assert.IsTrue(Regex.IsMatch(actualCopyrightText, copyrightPattern), "The footer copyright text is not displayed correctly.");
        }

        [Then(@"the Swag Labs {} link should navigate to {string}")]
        public void ThenTheSocialMediaLinkShouldNavigateTo(string platform, string expectedUrl)
        {
            // Verify if the new tab navigates to the expected URL
            string actualUrl = Driver.Url;
            Assert.AreEqual(expectedUrl, actualUrl, $"The Swag Labs {platform} link did not navigate to the expected URL.");
        }
        #endregion
    }
}
