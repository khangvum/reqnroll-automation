

using ReqnrollAutomation.Models.SwagLabs;

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

        // Registry
        public enum ValidationKey
        {
            OrderConfirmationHeader,
            OrderConfirmationMessage
        }

        public readonly Dictionary<ValidationKey, string> Registry;

        // Constants
        public const decimal TaxRate = 0.08m; // 8% tax rate
        #endregion

        #region Constructor
        public CheckoutPage(IWebDriver driver) : base(driver)
        {
            Registry = new()
            {
                { ValidationKey.OrderConfirmationHeader, "Thank you for your order!" },
                { ValidationKey.OrderConfirmationMessage, "Your order has been dispatched, and will arrive just as fast as the pony can get there!" }
            };
        }
        #endregion

        #region Page Locators
        // 1. Checkout Step One Page locators
        // - Input Field locators
        private readonly By _firstNameInputLocator = By.CssSelector("input#first-name");
        private readonly By _lastNameInputLocator = By.CssSelector("input#last-name");
        private readonly By _postalCodeInputLocator = By.CssSelector("input#postal-code");
        private readonly By _continueButtonLocator = By.CssSelector("input#continue");

        // - Cart List locators
        private readonly By _cartItemLocator = By.CssSelector("div.cart_item");

        // - Cart Item locators (relative to each cart item)
        private readonly By _itemNameLocator = By.CssSelector(".inventory_item_name");
        private readonly By _itemDescLocator = By.CssSelector(".inventory_item_desc");
        private readonly By _itemPriceLocator = By.CssSelector(".inventory_item_price");

        // 2. Checkout Step Two Page locators
        // - Price Total locators
        private readonly By _itemTotalLocator = By.CssSelector("div.summary_subtotal_label");
        private readonly By _taxLocator = By.CssSelector("div.summary_tax_label");
        private readonly By _totalLocator = By.CssSelector("div.summary_total_label");

        // - Finish Button locators
        private readonly By _finishButtonLocator = By.CssSelector("button#finish");

        // 3. Checkout Completion Page locators
        private readonly By _orderConfirmationHeaderLocator = By.CssSelector("h2.complete-header");
        private readonly By _orderConfirmationMessageLocator = By.CssSelector("div.complete-text");
        #endregion

        #region Page Elements
        // 1. Checkout Step One Page elements
        // - Input Field elements
        private IWebElement FirstNameInput => _driver.FindElement(_firstNameInputLocator);
        private IWebElement LastNameInput => _driver.FindElement(_lastNameInputLocator);
        private IWebElement PostalCodeInput => _driver.FindElement(_postalCodeInputLocator);
        private IWebElement ContinueButton => _driver.FindElement(_continueButtonLocator);

        // - Cart List elements
        private IReadOnlyList<IWebElement> CartItems => _driver.FindElements(_cartItemLocator);

        // 2. Checkout Step Two Page elements
        // - Price Total elements
        private IWebElement ItemTotal => _driver.FindElement(_itemTotalLocator);
        private IWebElement Tax => _driver.FindElement(_taxLocator);
        private IWebElement Total => _driver.FindElement(_totalLocator);

        // - Finish Button elements
        private IWebElement FinishButton => _driver.FindElement(_finishButtonLocator);

        // 3. Checkout Completion Page elements
        private IWebElement OrderConfirmationHeader => _driver.FindElement(_orderConfirmationHeaderLocator);
        private IWebElement OrderConfirmationMessage => _driver.FindElement(_orderConfirmationMessageLocator);
        #endregion

        #region Page Methods
        /// <summary>
        /// Clicks the continue button on the checkout information page to proceed to the next step of the checkout process.
        /// </summary>
        public void ClickContinue() => ContinueButton.Click();

        /// <summary>
        /// Clicks the finish button on the checkout overview page to complete the checkout process.
        /// </summary>
        public void ClickFinish() => FinishButton.Click();

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

        /// <summary>
        /// Retrieves the item total amount displayed on the checkout overview page.
        /// </summary>
        /// <returns>The item total amount as a decimal.</returns>
        public decimal GetItemTotal()
        {
            string itemTotalText = ItemTotal.Text.Replace("Item total: $", "");
            return decimal.Parse(itemTotalText);
        }

        /// <summary>
        /// Retrieves the tax amount displayed on the checkout overview page.
        /// </summary>
        /// <returns>The tax amount as a decimal.</returns>
        public decimal GetTax()
        {
            string taxText = Tax.Text.Replace("Tax: $", "");
            return decimal.Parse(taxText);
        }

        /// <summary>
        /// Retrieves the total amount displayed on the checkout overview page, which includes the item total and tax.
        /// </summary>
        /// <returns>The total amount as a decimal.</returns>
        public decimal GetTotal()
        {
            string totalText = Total.Text.Replace("Total: $", "");
            return decimal.Parse(totalText);
        }

        /// <summary>
        /// Retrieves the text of the order confirmation header displayed on the checkout completion page.
        /// </summary>
        /// <returns>The text of the order confirmation header.</returns>
        public string GetOrderConfirmationHeader() => OrderConfirmationHeader.Text;

        /// <summary>
        /// Retrieves the text of the order confirmation message displayed on the checkout completion page.
        /// </summary>
        /// <returns>The text of the order confirmation message.</returns>
        public string GetOrderConfirmationMessage() => OrderConfirmationMessage.Text;

        /// <summary>
        /// Gets the details of all items in the checkout overview, including their name, description, and price.
        /// </summary>
        /// <returns>A list of InventoryItemDetails objects containing the name, description, and price of each item in the checkout overview.</returns>
        public IReadOnlyList<InventoryItemDetails> GetCheckoutOverviewItemsDetails()
        {
            List<InventoryItemDetails> cartItemsDetails = [];
            foreach (IWebElement cartItem in CartItems)
            {
                // Get the details of each item
                string name = cartItem.FindElement(_itemNameLocator).Text;
                string description = cartItem.FindElement(_itemDescLocator).Text;
                string priceText = cartItem.FindElement(_itemPriceLocator).Text.Replace("$", "");
                decimal price = decimal.Parse(priceText);

                // Store the details of the item
                cartItemsDetails.Add(new InventoryItemDetails
                {
                    Name = name,
                    Description = description,
                    Price = price,
                });
            }

            return cartItemsDetails;
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
