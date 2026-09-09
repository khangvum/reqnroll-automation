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
            new() { Section = "About", ExpectedUrl = "https://khangvum.com/#about" },
            new(){ Section = "Experience", ExpectedUrl = "https://khangvum.com/#experience" },
            new(){ Section = "Projects", ExpectedUrl = "https://khangvum.com/#projects" },
            new(){ Section = "Contact", ExpectedUrl = "https://khangvum.com/#contact" }
        ];
        #endregion

        #region Page Locators
        // Header locators
        /// <summary>
        /// Gets the locator for a header section link based on the provided section name.
        /// </summary>
        /// <param name="section">The name of the section for which to get the locator.</param>
        /// <returns>The By object representing the locator for the header section link.</returns>
        private By GetHeaderSectionLocator(string section) => By.XPath($"//a[contains(@class, 'nav-link') and text()='{section}']");

        /// <summary>
        /// Gets the locator for a section on the page based on the provided section name.
        /// </summary>
        /// <param name="section">The name of the section for which to get the locator.</param>
        /// <returns>The By object representing the locator for the section.</returns>
        private By GetSectionLocator(string section) => By.Id(section.ToLower());
        #endregion

        #region Page Elements
        // Header elements
        /// <summary>
        /// Gets the IWebElement for a header section link based on the provided section name.
        /// </summary>
        /// <param name="section">The name of the section for which to get the element.</param>
        /// <returns>The IWebElement representing the header section link.</returns>
        private IWebElement GetHeaderSectionElement(string section) => _driver.WaitAndFindElement(GetHeaderSectionLocator(section));

        /// <summary>
        /// Gets the IWebElement for a section on the page based on the provided section name.
        /// </summary>
        /// <param name="section">The name of the section for which to get the element.</param>
        /// <returns>The IWebElement representing the section.</returns>
        private IWebElement GetSectionElement(string section) => _driver.WaitAndFindElement(GetSectionLocator(section));
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
        #endregion
    }
}
