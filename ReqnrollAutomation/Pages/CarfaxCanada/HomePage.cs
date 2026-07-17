using ReqnrollAutomation.Core.Extenstions;

namespace ReqnrollAutomation.Pages.CarfaxCanada
{
    public class HomePage : BasePage
    {
        #region Public Properties
        public override string PageUrl => "https://www.saucedemo.com/";
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
        /// Toggles the language of the website by clicking on the language toggle element.
        /// </summary>
        public void ToggleLanguage() => LanguageToggle.Click();
        #endregion
    }
}
