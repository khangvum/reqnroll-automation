using AventStack.ExtentReports.Model;
using ReqnrollAutomation.Core.Extenstions;
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

        // Main body locators
        private readonly By _mainHeadingLocator = By.CssSelector("h1.cfc-heading-text-type-");
        #endregion

        #region Page Elements
        // Accessbility elements
        private IWebElement LanguageToggle => _driver.WaitAndFindElement(_languageToggleLocator);

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
        /// Gets the current language code of the website by retrieving the 'lang' attribute from the <html> element.
        /// </summary>
        /// <returns>The current language code.</returns>
        /// <exception cref="InvalidOperationException">Throws an exception if the 'lang' attribute is not found on the <html> element.</exception>
        public string GetCurrentLanguageCode()
        {
            // Wait for the lang attribute to be set (up to 10 seconds)
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
        /// Gets the text of the main heading on the home page, normalizing whitespace and trimming leading/trailing spaces.
        /// </summary>
        /// <returns>The text of the main heading.</returns>
        public string GetMainHeadingText() => Regex.Replace(MainHeading.Text, @"\s+", " ").Trim();

        /// <summary>
        /// Toggles the language of the website by clicking on the language toggle element.
        /// </summary>
        public void ToggleLanguage() => LanguageToggle.Click();
        #endregion
    }
}
