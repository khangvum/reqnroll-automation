/**
 * Program:         FooterComponentStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-14
 * Description:     A class that defines the step definitions for the footer component verification feature on Swag Labs website.
 */

using ReqnrollAutomation.Pages.SwagLabs;
using System.Text.RegularExpressions;

namespace ReqnrollAutomation.StepDefinitions.SwagLabs
{
    /// <summary>
    /// A class that defines the step definitions for the footer component verification feature on Swag Labs website.
    /// </summary>
    [Binding]
    public class FooterComponentStepDefinitions : SwagLabsBaseStepDefinitions
    {
        #region Constructor
        public FooterComponentStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region Given Steps
        [Given(@"the Swag Labs footer contains links to social media pages")]
        public void GivenTheSwagLabFooterContainsLinksToSocialMediaPages()
        {
            // Scroll to the footer section to ensure it is visible on the screen
            InventoryPage.ScrollToFooter();

            // Verify if the footer contains the social media section
            bool isVisible = InventoryPage.IsSocialMediaSectionVisible();
            if (!isVisible)
            {
                throw new InvalidOperationException("Pre-condition Failed: The footer does not contain a social media section.");
            }
        }
        #endregion

        #region Then Steps
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
            string copyrightPattern = InventoryPage.Registry[InventoryPage.ValidationKey.CopyrightPattern];

            // Verify if the footer copyright text is displayed correctly
            Assert.IsTrue(Regex.IsMatch(actualCopyrightText, copyrightPattern), "The footer copyright text is not displayed correctly.");
        }

        [Then(@"all Swag Labs social media links should navigate to their expected destinations")]
        public void ThenAllSocialMediaLinksShouldNavigateToTheirExpectedDestinations()
        {
            foreach ((string platform, string expectedUrl) in InventoryPage.SocialMediaLinks)
            {
                // Click the social media link & switch to the new tab
                InventoryPage.ClickSocialMediaLink(platform);
                InventoryPage.SwitchToNewTab();

                // Verify if the new tab navigates to the expected URL
                string actualUrl = Driver.Url;
                Assert.AreEqual(expectedUrl, actualUrl, $"The Swag Labs {platform} link did not navigate to the expected URL.");

                // Close the new tab and switch back to the original tab
                InventoryPage.CloseCurrentTabAndSwitchBackToOriginalTab();
            }
        }
        #endregion
    }
}
