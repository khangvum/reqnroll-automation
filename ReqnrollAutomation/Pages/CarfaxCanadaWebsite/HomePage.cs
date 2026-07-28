using OpenQA.Selenium.Support.UI;
using ReqnrollAutomation.Config;
using ReqnrollAutomation.Core.Extensions;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace ReqnrollAutomation.Pages.CarfaxCanadaWebsite
{
    public class HomePage : BasePage
    {
        #region Public Properties
        // URL
        public override string PageUrl => "https://www.carfax.ca/";

        // Registry
        public enum ValidationKey
        {
            AccessibilityTheme
        }

        public readonly Dictionary<ValidationKey, string> Registry;

        // Constants
        // - Header links
        /// <summary>
        /// A read-only list of tuples representing the header links on the CARFAX Canada website.
        /// </summary>
        public readonly IReadOnlyList<(string Section, string SubSection, string ExpectedUrl)> HeaderLinks =
        [
            ("Vehicle History", "Vehicle History Reports", "https://www.carfax.ca/vehicle-history/vehicle-history-report"),
            ("Vehicle History", "View a Sample Report", "https://www.carfax.ca/vehicle-history/sample-report"),
            ("Vehicle Fraud", "What is VIN Fraud?", "https://www.carfax.ca/what-is-vin-fraud"),
            ("Vehicle Fraud", "VIN Fraud Check", "https://www.carfax.ca/vin-fraud-check"),
            ("Vehicle Fraud", "Vehicle Monitoring Subscription", "https://www.carfax.ca/vehicle-monitoring-subscription"),
            ("What’s My Car Worth", "Car Value", "https://www.carfax.ca/whats-my-car-worth/car-value/ymm"),
            ("What’s My Car Worth", "History Based Value", "https://www.carfax.ca/whats-my-car-worth/history-based-value"),
            ("Tools", "VIN Decoder", "https://www.carfax.ca/tools/vin-decode"),
            ("Tools", "Recall Check", "https://www.carfax.ca/tools/recall-check"),
            ("Tools", "Car Care", "https://www.carfax.ca/Service"),
            ("Resources", "Learn", "https://www.carfax.ca/learn"),
            ("Resources", "Support", "https://support.carfax.ca/en/support/home")
        ];

        /// <summary>
        /// Gets a list of expected URLs for internal subpages of the CARFAX Canada website, excluding "Support" and "Car Care" links.
        /// </summary>
        public List<string> InternalSubpageLinks => HeaderLinks
            .Where(link => link.SubSection != "Support" && link.SubSection != "Car Care")
            .Select(link => link.ExpectedUrl)
            .ToList();

        // - Footer links
        /// <summary>
        /// A read-only list of tuples representing the footer links on the CARFAX Canada website, including section, subsection, and expected URL.
        /// </summary>
        public readonly IReadOnlyList<(string Section, string SubSection, string ExpectedUrl)> FooterLinks =
            [
            ("Products", "CARFAX Canada Vehicle History Reports", "https://www.carfax.ca/vehicle-history/vehicle-history-report"),
            ("Products", "CARFAX Canada VIN Fraud Check", "https://www.carfax.ca/vin-fraud-check"),
            ("Products", "CARFAX Canada Vehicle Monitoring", "https://www.carfax.ca/vehicle-monitoring-subscription"),
            ("Products", "CARFAX Canada History-Based Value", "https://www.carfax.ca/whats-my-car-worth/history-based-value"),
            ("Products", "CARFAX Canada VIN Decoder", "https://www.carfax.ca/tools/vin-decode"),
            ("Products", "CARFAX Canada Recall Check", "https://www.carfax.ca/tools/recall-check"),
            ("Products", "CARFAX Canada Car Care", "https://www.carfax.ca/Service"),
            ("Resources", "Learn", "https://www.carfax.ca/learn"),
            ("Resources", "Support", "https://support.carfax.ca/en/support/home"),
            ("Resources", "Media", "https://www.carfax.ca/media"),
            ("Company", "About", "https://www.carfax.ca/about"),
            ("Company", "Contact", "https://www.carfax.ca/contact"),
            ("Company", "Careers", "https://www.carfax.ca/careers"),
            ("Company", "Partners", "https://www.carfax.ca/partners"),
            ("Company", "Vehicle Fraud", "https://www.carfax.ca/vehicle-fraud"),
            ("Company", "CARFAX Canada Data", "https://www.carfax.ca/vehicle-history-data"),
            ("Company", "CARPROOF is CARFAX Canada", "https://www.carfax.ca/carproof"),
            ("Business Solutions", "Dealer Login", "https://authentication.carfax.ca/"),
            ("Business Solutions", "Become a Dealer Member", "https://www.carfax.ca/become-a-member"),
            ("Business Solutions", "Banking, Insurance and Government", "https://go.carfax.ca/en-ca/big/home"),
            ("Business Solutions", "Automotive Remarketing and OEM", "https://go.carfax.ca/aro")
        ];

        // - Footer social media links
        /// <summary>
        /// A read-only list of tuples representing the social media links in the footer of the CARFAX Canada website, including platform name and expected URL.
        /// </summary>
        public readonly IReadOnlyList<(string Platform, string ExpectedUrl)> SocialMediaLinks =
        [
            ("Facebook", "https://www.facebook.com/CARFAXCanada/"),
            //("Instagram", "https://www.instagram.com/carfaxca/?hl=en"),   // Instagram will force to login page if not logged in
            ("LinkedIn", "https://www.linkedin.com/company/carfax-canada/"),
            ("YouTube", "https://www.youtube.com/user/CarProof")
        ];

        // - Footer utility links
        /// <summary>
        /// A read-only list of tuples representing the utility links in the footer of the CARFAX Canada website, including utility item name and expected URL.
        /// </summary>
        public readonly IReadOnlyList<(string UtilityItem, string ExpectedUrl)> UtilityLinks =
        [
            ("Privacy/Legal", "https://www.carfax.ca/privacy-legal"),
            ("Accessibility", "https://www.carfax.ca/accessibility"),
            ("Conditions Of Use", "https://www.carfax.ca/privacy-legal/conditions-of-use"),
            ("© 2026 CARFAX Canada ULC. All Rights Reserved.", "#")
        ];
        #endregion

        #region Page Locators
        // Accessbility locators
        private readonly By _languageToggleLocator = By.CssSelector("a.cfc-header_lang");
        private readonly By _accessibilityToggleLocator = By.Id("cfc-theme-toggle");

        // Header locators
        private readonly By _headerLogoLocator = By.CssSelector("a.navbar-brand.cfc-logo");

        /// <summary>
        /// Gets the locator for a header section based on the provided section name.
        /// </summary>
        /// <param name="sectionName">The name of the section (e.g., "Vehicle History", "Vehicle Fraud", "What's My Car Worth", "Tools", "Resources").</param>
        /// <returns>The locator for the header section.</returns>
        private By GetHeaderSectionLocator(string sectionName) => By.XPath($"//button[contains(@class,'cfc-header-title') and normalize-space(text())='{sectionName}']");

        /// <summary>
        /// Gets the locator for a header subsection based on the provided subsection name.
        /// </summary>
        /// <param name="subSectionName">The name of the subsection.</param>
        /// <returns>The locator for the header subsection.</returns>
        private By GetHeaderSubsectionLocator(string subSectionName) => By.XPath($"//a[contains(@class,'cfc-header-link') and normalize-space(text())='{subSectionName}']");

        // Main body locators
        private readonly By _mainHeadingLocator = By.CssSelector("h1.cfc-heading-text-type-");

        // Footer locators
        private readonly By _footerContainerLocator = By.CssSelector("div.cfc-footer");
        private readonly By _footerDisclaimerTextLocator = By.CssSelector("p.cfc-footer__copy");
        private readonly By _socialMediaSectionLocator = By.CssSelector("ul.cfc-footer__logos");
        private readonly By _footerUtilitySectionLocator = By.CssSelector("ul.cfc-footer__utilities");

        /// <summary>
        /// Gets the locator for a footer subsection based on the provided subsection name.
        /// </summary>
        /// <param name="subSectionName">The name of the subsection.</param>
        /// <returns>The locator for the footer subsection.</returns>
        private By GetFooterSubsectionLocator(string subSectionName) =>
            By.XPath($"//a[contains(@class,'cfc-footer-section__item') and normalize-space(text())='{subSectionName}']");

        /// <summary>
        /// Gets the locator for a social media link based on the provided platform name.
        /// </summary>
        /// <param name="platform">The social media platform (e.g., "Facebook", "Instagram", "LinkedIn", "YouTube").</param>
        /// <returns>The locator for the social media link.</returns>
        private By GetSocialMediaLinkLocator(string platform) => By.XPath($"//ul[contains(@class,'cfc-footer__logos')]//img[contains(@alt,'{platform}')]/ancestor::a");

        /// <summary>
        /// Gets the locator for a footer utility link based on the provided utility name.
        /// </summary>
        /// <param name="utilityName">The name of the utility (e.g., "Privacy/Legal", "Accessibility", "Conditions Of Use").</param>
        /// <returns>The locator for the footer utility link.</returns>
        private By GetFooterUtilityLinkLocator(string utilityName) => By.XPath($"//ul[contains(@class,'cfc-footer__utilities')]//a[normalize-space(text())='{utilityName}' or @aria-label='{utilityName}']");

        // Cookie banner locators
        private readonly By _cookieBannerContainerLocator = By.CssSelector("div.cookie-banner");
        private readonly By _cookieBannerAcceptButtonLocator = By.CssSelector("div.cookie-banner button.cookie-banner-accept");
        #endregion

        #region Page Elements
        // Accessbility elements
        private IWebElement LanguageToggle => _driver.WaitAndFindElement(_languageToggleLocator);
        private IWebElement AccessibilityToggle => _driver.WaitAndFindElement(_accessibilityToggleLocator);

        // Header elements
        private IWebElement HeaderLogo => _driver.WaitAndFindElement(_headerLogoLocator);

        // Main body elements
        private IWebElement MainHeading => _driver.WaitAndFindElement(_mainHeadingLocator);

        // Footer elements
        private IWebElement FooterContainer => _driver.WaitAndFindElement(_footerContainerLocator);
        private IWebElement FooterDisclaimerText => _driver.WaitAndFindElement(_footerDisclaimerTextLocator);
        private IWebElement SocialMediaSection => _driver.WaitAndFindElement(_socialMediaSectionLocator);
        private IWebElement FooterUtilitySection => _driver.WaitAndFindElement(_footerUtilitySectionLocator);

        /// <summary>
        /// Gets the social media link element for the specified platform.
        /// </summary>
        /// <param name="platform">The social media platform (e.g., "Facebook", "Instagram", "LinkedIn", "YouTube").</param>
        /// <returns>The social media link element.</returns>
        private IWebElement GetSocialMediaLink(string platform) => _driver.WaitAndFindElement(GetSocialMediaLinkLocator(platform));

        /// <summary>
        /// Gets the footer utility link element for the specified utility name.
        /// </summary>
        /// <param name="utilityName">The name of the utility (e.g., "Privacy/Legal", "Accessibility", "Conditions Of Use").</param>
        /// <returns>The footer utility link element.</returns>
        private IWebElement GetFooterUtilityLink(string utilityName) => _driver.WaitAndFindElement(GetFooterUtilityLinkLocator(utilityName));

        // Cookie banner elements
        private IWebElement CookieBannerContainer => _driver.WaitAndFindElement(_cookieBannerContainerLocator);
        private IWebElement CookieBannerAcceptButton => _driver.WaitAndFindElement(_cookieBannerAcceptButtonLocator);
        #endregion

        #region Constructor
        public HomePage(IWebDriver driver) : base(driver)
        {
            Registry = new()
            {
                { ValidationKey.AccessibilityTheme, "high-contrast" }
            };
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Checks if the cookie banner is present and accepts cookies if it is displayed.
        /// </summary>
        public void AcceptCookiesIfPresent()
        {
            // Use shorter wait to check for the presence of the cookie banner
            WebDriverWait shortWait = new(_driver, TimeSpan.FromSeconds(ConfigManager.DefaultTimeout / 5));

            try
            {
                IWebElement cookieBannerAcceptButton = shortWait.Until(driver =>
                {
                    ReadOnlyCollection<IWebElement> elements = driver.FindElements(_cookieBannerAcceptButtonLocator);
                    return elements.Count > 0 && elements[0].Displayed ? elements[0] : null;
                });

                Console.WriteLine("[LOG] Cookie banner is displayed. Accepting cookies...");
                cookieBannerAcceptButton.Click();
            }
            catch (Exception ex) when (ex is NoSuchElementException || ex is WebDriverTimeoutException)
            {
                // Cookie banner is not present, do nothing
                Console.WriteLine("[LOG] Cookie banner is not present. No action taken.");
            }
        }

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
        /// Clicks on the header logo.
        /// </summary>
        public void ClickHeaderLogo()
        {
            HeaderLogo.Click();
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
        /// Clicks on a subsection in the footer based on the provided subsection name.
        /// </summary>
        /// <param name="subSectionName">The name of the subsection to click.</param>
        public void ClickFooterSubsection(string subSectionName)
        {
            IWebElement subSectionElement = _driver.WaitAndFindElement(GetFooterSubsectionLocator(subSectionName));
            subSectionElement.Click();
        }

        /// <summary>
        /// Clicks on a social media link in the footer based on the provided platform name.
        /// </summary>
        /// <param name="platform">The social media platform (e.g., "Facebook", "Instagram", "LinkedIn", "YouTube").</param>
        public void ClickSocialMediaLink(string platform) => GetSocialMediaLink(platform).Click();

        /// <summary>
        /// Clicks on a utility link in the footer based on the provided utility name.
        /// </summary>
        /// <param name="utilityName">The name of the utility (e.g., "Privacy/Legal", "Accessibility", "Conditions Of Use").</param>
        public void ClickFooterUtilityLink(string utilityName) => GetFooterUtilityLink(utilityName).Click();

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
        /// Gets the 'target' attribute of a subsection link in the footer based on the provided subsection name.
        /// </summary>
        /// <param name="subSectionName">The name of the subsection.</param>
        /// <returns>The 'target' attribute of the subsection link.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the 'target' attribute is not found on the subsection link.</exception>
        public string GetFooterSubsectionLinkTarget(string subSectionName)
        {
            IWebElement subSectionElement = _driver.WaitAndFindElement(GetFooterSubsectionLocator(subSectionName));
            return subSectionElement.GetAttribute("target") ?? throw new InvalidOperationException($"The 'target' attribute is not found on the footer subsection '{subSectionName}'.");
        }

        /// <summary>
        /// Retrieves the text of the footer disclaimer element.
        /// </summary>
        /// <returns>The text of the footer disclaimer.</returns>
        public string GetDisclaimerText() => FooterDisclaimerText.Text;

        /// <summary>
        /// Checks if the footer disclaimer text is visible on the page.
        /// </summary>
        /// <returns>True if the disclaimer text is visible, otherwise false.</returns>
        public bool IsDisclaimerTextVisible() => FooterDisclaimerText.Displayed;

        /// <summary>
        /// Checks if the social media section in the footer is visible on the page.
        /// </summary>
        /// <returns>True if the social media section is visible, otherwise false.</returns>
        public bool IsSocialMediaSectionVisible() => SocialMediaSection.Displayed;

        /// <summary>
        /// Checks if the utility links section in the footer is visible on the page.
        /// </summary>
        /// <returns>True if the utility links section is visible, otherwise false.</returns>
        public bool IsUtilityLinksSectionVisible() => FooterUtilitySection.Displayed;

        /// <summary>
        /// Toggles the language of the website by clicking on the language toggle element.
        /// </summary>
        public void ToggleLanguage() => LanguageToggle.Click();

        /// <summary>
        /// Toggles the accessibility mode of the website by clicking on the accessibility toggle element.
        /// </summary>
        public void ToggleAccessibility() => AccessibilityToggle.Click();

        /// <summary>
        /// Scrolls the page to the footer section.
        /// </summary>
        public void ScrollToFooter() => ScrollIntoView(FooterContainer);
        #endregion
    }
}
