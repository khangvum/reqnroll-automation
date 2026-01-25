using ReqnrollAutomation.Core.Extensions;
using System.Text.RegularExpressions;

namespace ReqnrollAutomation.Pages.CarfaxCanada
{
    public class HomePage : BasePage
    {
        #region Public Properties
        public override string PageUrl => "https://www.carfax.ca/";
        #endregion

        #region Page Locators
        // Accessbility locators
        private readonly By _languageToggleLocator = By.CssSelector("a.cfc-header_lang");
        private readonly By _accessibilityToggleLocator = By.Id("cfc-theme-toggle");

        // Header locators
        private readonly By _headerLogoLocator = By.CssSelector("a.navbar-brand.cfc-logo");
        private static By GetHeaderSectionLocator(string sectionName) => By.XPath($"//button[contains(@class,'cfc-header-title') and normalize-space(text())='{sectionName}']");
        private static By GetHeaderSubsectionLocator(string subSectionName) => By.XPath($"//a[contains(@class,'cfc-header-link') and normalize-space(text())='{subSectionName}']");

        // Main body locators
        private readonly By _mainHeadingLocator = By.CssSelector("h1.cfc-heading-text-type-");
        #endregion

        #region Page Elements
        // Accessbility elements
        private IWebElement LanguageToggle => _driver.WaitAndFindElement(_languageToggleLocator);
        private IWebElement AccessibilityToggle => _driver.WaitAndFindElement(_accessibilityToggleLocator);

        // Header elements
        private IWebElement HeaderLogo => _driver.WaitAndFindElement(_headerLogoLocator);

        // Main body elements
        private IWebElement MainHeading => _driver.WaitAndFindElement(_mainHeadingLocator);
        #endregion

        #region Constructor
        public HomePage(IWebDriver driver) : base(driver)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clicks on a section in the header based on the provided section name.
        /// </summary>
        /// <param name="sectionName">The name of the section to click.</param>
        public void HoverHeaderSection(string sectionName)
        {
            IWebElement sectionElement = _driver.WaitAndFindElement(GetHeaderSectionLocator(sectionName));
            sectionElement.Hover();
        }

        /// <summary>
        /// Clicks on a subsection in the header based on the provided subsection name.
        /// </summary>
        /// <param name="subSectionName">The name of the subsection to click.</param>
        public void ClickHeaderSubsection(string subSectionName)
        {
            IWebElement subSectionElement = _driver.WaitAndFindElement(GetHeaderSubsectionLocator(subSectionName));
            subSectionElement.Click();
        }

        /// <summary>
        /// Gets the current language code of the website by retrieving the 'lang' attribute from the <html> element.
        /// </summary>
        /// <returns>The current language code.</returns>
        /// <exception cref="InvalidOperationException">Throws an exception if the 'lang' attribute is not found on the <html> element.</exception>
        public string GetCurrentLanguageCode()
        {
            // Wait for the lang attribute to be set
            IWebElement htmlElement = _wait.Until(driver =>
            {
                IWebElement html = driver.WaitAndFindElement(By.TagName("html"));
                var lang = html.GetAttribute("lang");
                return !string.IsNullOrEmpty(lang) ? html : null;
            });

            string langAttribute = htmlElement.GetAttribute("lang") ?? throw new InvalidOperationException("The 'lang' attribute is not found on the <html> element.");
            return langAttribute[..2].ToLower();
        }

        /// <summary>
        /// Gets the current theme of the website by retrieving the 'data-bs-theme' attribute from the <html> element.
        /// </summary>
        /// <returns>The current theme.</returns>
        /// <exception cref="InvalidOperationException">Throws an exception if the 'data-bs-theme' attribute is not found on the <html> element.</exception>
        public string GetCurrentTheme()
        {
            // Wait for the data-bs-theme attribute to be set
            IWebElement htmlElement = _wait.Until(driver =>
            {
                IWebElement html = driver.WaitAndFindElement(By.TagName("html"));
                var theme = html.GetAttribute("data-bs-theme");
                return !string.IsNullOrEmpty(theme) ? html : null;
            });

            string themeAttribute = htmlElement.GetAttribute("data-bs-theme") ?? throw new InvalidOperationException("The 'data-bs-theme' attribute is not found on the <html> element.");
            return themeAttribute;
        }

        /// <summary>
        /// Gets the text of the main heading on the home page, normalizing whitespace and trimming leading/trailing spaces.
        /// </summary>
        /// <returns>The text of the main heading.</returns>
        public string GetMainHeadingText() => Regex.Replace(MainHeading.Text, @"\s+", " ").Trim();

        /// <summary>
        /// Gets the color of the main heading on the home page by retrieving the CSS 'color' property.
        /// </summary>
        /// <returns>The color of the main heading.</returns>
        public string GetMainHeadingColor() => MainHeading.GetCssValue("color");

        /// <summary>
        /// Gets the 'target' attribute of a subsection link in the header based on the provided subsection name.
        /// </summary>
        /// <param name="subSectionName">The name of the subsection.</param>
        /// <returns>The 'target' attribute of the subsection link.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the 'target' attribute is not found on the subsection link.</exception>
        public string GetHeaderSubsectionLinkTarget(string subSectionName)
        {
            IWebElement subSectionElement = _driver.WaitAndFindElement(GetHeaderSubsectionLocator(subSectionName));
            return subSectionElement.GetAttribute("target") ?? throw new InvalidOperationException($"The 'target' attribute is not found on the subsection '{subSectionName}'.");
        }

        /// <summary>
        /// Toggles the language of the website by clicking on the language toggle element.
        /// </summary>
        public void ToggleLanguage() => LanguageToggle.Click();

        /// <summary>
        /// Toggles the accessibility mode of the website by clicking on the accessibility toggle element.
        /// </summary>
        public void ToggleAccessibility() => AccessibilityToggle.Click();
        #endregion
    }
}
