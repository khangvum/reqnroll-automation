/**
 * Program:         SwagLabsBaseStepDefinitions.cs
 * Author:          Manh Khang Vu
 * Date:            2026-07-17
 * Description:     A class that defines base step definitions for the automation framework for Swag Labs.
 */

using ReqnrollAutomation.Pages.SwagLabs;

namespace ReqnrollAutomation.StepDefinitions.SwagLabs
{
    public class SwagLabsBaseStepDefinitions : BaseStepDefinitions
    {
        #region Private Attributes
        // Pages
        private LoginPage? _loginPage;
        private InventoryPage? _inventoryPage;
        private CartPage? _cartPage;
        private CheckoutPage? _checkoutPage;
        #endregion

        #region Protected Attributes
        // Context Keys
        protected const string AddedInventoryItemsDetailsKey = nameof(AddedInventoryItemsDetailsKey);
        #endregion

        #region Public Properties
        /// <summary>
        /// Lazy initialization of the LoginPage instance.
        /// </summary>
        public LoginPage LoginPage
        {
            get
            {
                _loginPage ??= new(Driver);
                return _loginPage;
            }
        }

        /// <summary>
        /// Lazy initialization of the InventoryPage instance.
        /// </summary>
        public InventoryPage InventoryPage
        {
            get
            {
                _inventoryPage ??= new(Driver);
                return _inventoryPage;
            }
        }

        /// <summary>
        /// Lazy initialization of the CartPage instance.
        /// </summary>
        public CartPage CartPage
        {
            get
            {
                _cartPage ??= new(Driver);
                return _cartPage;
            }
        }

        public CheckoutPage CheckoutPage
        {
            get
            {
                _checkoutPage ??= new(Driver);
                return _checkoutPage;
            }
        }
        #endregion

            #region Constructor
        public SwagLabsBaseStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) : base(scenarioContext, featureContext)
        {
        }
        #endregion
    }
}
