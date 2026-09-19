

using ReqnrollAutomation.Core.Extensions;
using ReqnrollAutomation.Models.KhangvumPortfolio;

/**
 * Program:         ProjectsComponentStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-09-18
 * Description:     A class that defines the step definitions for the projects component verification feature on the Khangvum Portfolio website.
 */

namespace ReqnrollAutomation.StepDefinitions.KhangvumPortfolio
{
    /// <summary>
    /// A class that defines the step definitions for the projects component verification feature on the Khangvum Portfolio website.
    /// </summary>
    [Binding]
    public class ProjectsComponentStepDefinitions : KhangvumPortfolioBaseStepDefinitions
    {
        #region Constructor
        public ProjectsComponentStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion

        #region When Steps
        [When(@"I click on a random project card")]
        public void WhenIClickOnARandomProjectCard()
        {
            // Scroll to the projects section
            PortfolioPage.ScrollToProjectsSection();

            // Click on a random project card
            ProjectCard projectCard = PortfolioPage.ClickRandomProjectCard();
            _scenarioContext.SetValue(ProjectCardKey, projectCard);
        }
        #endregion

        #region Then Steps
        [Then(@"the project card should navigate to the correct GitHub repository")]
        public void ThenTheProjectCardShouldNavigateToTheCorrectGitHubRepository()
        {
            // Retrieve the project card from the scenario context
            ProjectCard projectCard = _scenarioContext.GetValue<ProjectCard>(ProjectCardKey);

            // Switch to new tab
            PortfolioPage.SwitchToNewTab();

            // Verify if the new tab navigates to the expected URL
            string actualUrl = PortfolioPage.WaitForUrlToStabilize();
            Assert.AreEqual(projectCard.ExpectedUrl, actualUrl, $"The URL for the '{projectCard.ProjectName}' project card does not match the expected GitHub repository URL.");
        }
        #endregion
    }
}
