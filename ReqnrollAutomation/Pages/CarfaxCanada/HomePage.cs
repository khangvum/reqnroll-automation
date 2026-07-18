using ReqnrollAutomation.Core.Extenstions;

namespace ReqnrollAutomation.Pages.CarfaxCanada
{
    public class HomePage : BasePage
    {
        #region Public Properties
        public override string PageUrl => "https://www.carfax.ca/";
        #endregion

        #region Page Locators
        private readonly By _languageToggleLocator = By.CssSelector("a.cfc-header_lang");
        #endregion

        #region Page Elements
        private IWebElement LanguageToggle => _driver.WaitAndFindElement(_languageToggleLocator);
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
        /// Toggles the language of the website by clicking on the language toggle element.
        /// </summary>
        public void ToggleLanguage() => LanguageToggle.Click();

        ///// <summary>
        ///// Gets the text of the language toggle element, which indicates the current language of the website.
        ///// </summary>
        ///// <returns></returns>
        //public string GetLanguageToggleText() => LanguageToggle.Text;
        #endregion
    }
}
