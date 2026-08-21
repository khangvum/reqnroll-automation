/**
 * Program:         CartPage.cs
 * Author:          Manh Khang Vu
 * Date:            2026-08-20
 * Description:     A class that represents the checkout page of Swag Labs.
 */

namespace ReqnrollAutomation.Pages.SwagLabs
{
    public class CheckoutPage : BasePage
    {
        #region Public Properties
        // URL
        public override string PageUrl => "https://www.saucedemo.com/checkout-step-one.html";
        #endregion

        #region Constructor
        public CheckoutPage(IWebDriver driver) : base(driver)
        {
        }
        #endregion

        #region Page Locators
        // Checkout Step One Page locators
        private readonly By _firstNameInputLocator = By.CssSelector("input#first-name");
        private readonly By _lastNameInputLocator = By.CssSelector("input#last-name");
        private readonly By _postalCodeInputLocator = By.CssSelector("input#postal-code");
        private readonly By _continueButtonLocator = By.CssSelector("input#continue");
        #endregion

        #region Page Elements
        // Checkout Step One Page elements
        private IWebElement FirstNameInput => _driver.FindElement(_firstNameInputLocator);
        private IWebElement LastNameInput => _driver.FindElement(_lastNameInputLocator);
        private IWebElement PostalCodeInput => _driver.FindElement(_postalCodeInputLocator);
        private IWebElement ContinueButton => _driver.FindElement(_continueButtonLocator);
        #endregion

        #region Page Methods
        /// <summary>
        /// Clicks the continue button on the checkout information page to proceed to the next step of the checkout process.
        /// </summary>
        public void ClickContinue() => ContinueButton.Click();

        /// <summary>
        /// Enters the checkout information including first name, last name, and postal code into the respective input fields.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="postalCode"></param>
        public void EnterCheckoutInformation(string firstName, string lastName, string postalCode)
        {
            FirstNameInput.SendKeys(firstName);
            LastNameInput.SendKeys(lastName);
            PostalCodeInput.SendKeys(postalCode);
        }

        /// <summary>
        /// Generates and enters random checkout information including first name, last name, and postal code into the respective input fields.
        /// </summary>
        public void EnterRandomCheckoutInformation()
        {
            string[] firstNames = ["Khang", "Alex", "Jordan", "Taylor", "Morgan", "Sam", "Casey"];
            string[] lastNames = ["Vu", "Smith", "Johnson", "Brown", "Tremblay", "Roy", "MacDonald"];

            string firstName = firstNames[Random.Shared.Next(firstNames.Length)];
            string lastName = lastNames[Random.Shared.Next(lastNames.Length)];
            string postalCode = GenerateRandomCanadianPostalCode();

            EnterCheckoutInformation(firstName, lastName, postalCode);
        }
        #endregion

        #region Private Helper Methods
        /// <summary>
        /// Generates a random Canadian postal code (e.g., A1A 1A1 format).
        /// </summary>
        private static string GenerateRandomCanadianPostalCode()
        {
            string letters = "ABCEGHJKLMNPRSTVXY"; // Standard valid Canadian postal code starting letters
            string numbers = "0123456789";

            char l1 = letters[Random.Shared.Next(letters.Length)];
            char n1 = numbers[Random.Shared.Next(numbers.Length)];
            char l2 = letters[Random.Shared.Next(letters.Length)];
            char n2 = numbers[Random.Shared.Next(numbers.Length)];
            char l3 = letters[Random.Shared.Next(letters.Length)];
            char n3 = numbers[Random.Shared.Next(numbers.Length)];

            return $"{l1}{n1}{l2} {n2}{l3}{n3}";
        }
        #endregion
    }
}
