using ReqnrollAutomation.Core.Extensions;
using ReqnrollAutomation.Models.KhangvumPortfolio;

namespace ReqnrollAutomation.Pages.KhangvumPortfolio
{
    public class PortfolioPage : BasePage
    {
        #region Public Properties
        public override string PageUrl => "https://khangvum.com/";

        // Constants
        // - Header links
        /// <summary>
        /// A read-only list of header links on the Khangvum Portfolio page, each containing a section name and its expected URL.
        /// </summary>
        public readonly IReadOnlyList<HeaderLink> HeaderLinks =
        [
            new(){ Section = "About", ExpectedUrl = "https://khangvum.com/#about" },
            new(){ Section = "Experience", ExpectedUrl = "https://khangvum.com/#experience" },
            new(){ Section = "Projects", ExpectedUrl = "https://khangvum.com/#projects" },
            new(){ Section = "Contact", ExpectedUrl = "https://khangvum.com/#contact" }
        ];

        // - Projects links
        /// <summary>
        /// A read-only list of project links on the Khangvum Portfolio page, each containing a project name and its expected URL.
        /// </summary>
        public readonly IReadOnlyList<ProjectCard> ProjectCards =
        [
            new(){ ProjectName = "Infrastructure as Code (IaC) Homelab", ExpectedUrl = "https://github.com/khangvum/homelab-iac" },
            new(){ ProjectName = "Answer Files", ExpectedUrl = "https://github.com/khangvum/answer-files" },
            new(){ ProjectName = "Reqnroll Automation", ExpectedUrl = "https://github.com/khangvum/reqnroll-automation" },
            new(){ ProjectName = "Expression Evaluator", ExpectedUrl = "https://github.com/khangvum/exprevaluator" },
            new(){ ProjectName = "NixOS-WSL Configuration", ExpectedUrl = "https://github.com/khangvum/nixos-wsl" },
            new(){ ProjectName = "Chemical Equation Balancer", ExpectedUrl = "https://github.com/khangvum/chemicalbalancer" }
        ];

        // - Footer social media links
        /// <summary>
        /// A read-only list of social media links in the footer of the Khangvum Portfolio page.
        /// </summary>
        public readonly IReadOnlyList<SocialMediaLink> SocialMediaLinks =
        [
            new() { Platform = "GitHub", ExpectedUrl = "https://github.com/khangvum" },
            //new() { Platform = "LinkedIn", ExpectedUrl = "https://linkedin.com/in/khangvum" },    // LinkedIn will force to login page to view personal profiles
            new() { Platform = "Gmail", ExpectedUrl = "mailto:manhkhang0305@gmail.com" }
        ];
        #endregion

        #region Page Locators
        // Header locators
        /// <summary>
        /// Gets the locator for a header section link based on the provided section name.
        /// </summary>
        /// <param name="section">The name of the section for which to get the locator.</param>
        /// <returns>The locator for the header section link.</returns>
        private By GetHeaderSectionLocator(string section) => By.XPath($"//a[contains(@class, 'nav-link') and text()='{section}']");

        /// <summary>
        /// Gets the locator for a section on the page based on the provided section name.
        /// </summary>
        /// <param name="section">The name of the section for which to get the locator.</param>
        /// <returns>The locator for the section.</returns>
        private By GetSectionLocator(string section) => By.Id(section.ToLower());

        // Projects locator
        /// <summary>
        /// Gets the locator for a project card based on the provided index.
        /// </summary>
        /// <param name="index">The index of the project card for which to get the locator.</param>
        /// <returns>The locator for the project card.</returns>
        private By GetStationCardLocator(Index index) => By.XPath($"//div[contains(@class, 'stations-force-grid')]/div[contains(@class, 'station-node')][{index.GetOffset(ProjectCards.Count) + 1}]");

        // Footer locators
        /// <summary>
        /// Gets the locator for a social media button in the footer based on the provided platform name.
        /// </summary>
        /// <param name="platform">The name of the social media platform.</param>
        /// <returns>The locator for the social media button.</returns>
        private By GetFooterSocialMediaButtonLocator(string platform) => By.XPath($"//a[contains(@class, 'contact-icon-btn') and contains(@href, '{platform.ToLower()}')]");
        #endregion

        #region Page Elements
        // Header elements
        /// <summary>
        /// Gets the element for a header section link based on the provided section name.
        /// </summary>
        /// <param name="section">The name of the section for which to get the element.</param>
        /// <returns>The header section link element.</returns>
        private IWebElement GetHeaderSectionElement(string section) => _driver.WaitAndFindElement(GetHeaderSectionLocator(section));

        /// <summary>
        /// Gets the element for a section on the page based on the provided section name.
        /// </summary>
        /// <param name="section">The name of the section for which to get the element.</param>
        /// <returns>The section element.</returns>
        private IWebElement GetSectionElement(string section) => _driver.WaitAndFindElement(GetSectionLocator(section));

        // Project elements
        /// <summary>
        /// Gets the element for a project card based on the provided index.
        /// </summary>
        /// <param name="index">The index of the project card for which to get the element.</param>
        /// <returns>The project card element.</returns>
        private IWebElement GetStationCardElement(Index index) => _driver.WaitAndFindElement(GetStationCardLocator(index));

        // Footer elements
        /// <summary>
        /// Gets the element for a social media button in the footer based on the provided platform name.
        /// </summary>
        /// <param name="platform">The name of the social media platform.</param>
        /// <returns>The social media button element.</returns>
        private IWebElement GetFooterSocialMediaButton(string platform) => _driver.WaitAndFindElement(GetFooterSocialMediaButtonLocator(platform));
        #endregion

        #region Constructor
        public PortfolioPage(IWebDriver driver) : base(driver)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clicks on a header section link based on the provided section name.
        /// </summary>
        /// <param name="section">The section name for which to click the link.</param>
        public void ClickHeaderLink(string section)
        {
            IWebElement headerLink = GetHeaderSectionElement(section);
            headerLink.Click();
        }

        /// <summary>
        /// Clicks on a social media button in the footer based on the provided platform name.
        /// </summary>
        /// <param name="platform">The name of the social media platform.</param>
        public void ClickFooterSocialMediaButton(string platform) => GetFooterSocialMediaButton(platform).Click();

        /// <summary>
        /// CLicks on a project card based on the provided index.
        /// </summary>
        /// <param name="index">The index of the project card to click.</param>
        public void ClickProjectCard(Index index) => GetStationCardElement(index).Click();

        /// <summary>
        /// Clicks on a random project card from the list of project links and returns the corresponding ProjectLink object.
        /// </summary>
        /// <returns>The ProjectLink object for the clicked project card.</returns>
        public ProjectCard ClickRandomProjectCard()
        {
            int randomIndex = Random.Shared.Next(ProjectCards.Count);
            ClickProjectCard(randomIndex);
            return ProjectCards[randomIndex];
        }

        /// <summary>
        /// Checks if a section is in the viewport based on the provided section name.
        /// </summary>
        /// <param name="section">The name of the section to check.</param>
        /// <returns>True if the section is in the viewport, false otherwise.</returns>
        public bool IsSectionInViewport(string section)
        {
            IWebElement sectionElement = GetSectionElement(section);

            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            return (bool)js.ExecuteScript("""
                var rect = arguments[0].getBoundingClientRect();
                var windowHeight = window.innerHeight || document.documentElement.clientHeight;
                
                // Checks if the section is visible anywhere within the current viewport
                return (rect.top <= windowHeight && rect.bottom >= 0);
                """, sectionElement)!;
        }

        /// <summary>
        /// Scrolls the page to the footer section.
        /// </summary>
        public void ScrollToFooter() => ScrollIntoView(GetSectionElement(HeaderLinks[^1].Section));

        /// <summary>
        /// Scrolls the page to the "Projects" section.
        /// </summary>
        public void ScrollToProjectsSection() => ScrollIntoView(GetSectionElement(HeaderLinks[2].Section));
        #endregion
    }
}
