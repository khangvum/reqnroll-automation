/**
 * Program:         ErrorMessageHelper.cs
 * Author:          Manh Khang Vu
 * Date:            2026-05-17
 * Description:     A class that includes helper methods for handling error messages and exceptions.
 */

namespace ReqnrollAutomation.Helpers
{
    /// <summary>
    /// A class that includes helper methods for handling error messages and exceptions.
    /// </summary>
    internal class ErrorMessageHelper
    {
        /// <summary>
        /// Formats an exception into a user-friendly error message and the 
        /// full exception details in a collapsible section for debugging purposes.
        /// </summary>
        /// <param name="ex">The exception to format.</param>
        /// <returns>The formatted error message with details.</returns>
        public static string FormatExceptionMessage(Exception ex)
        {
            string friendlyMessage = GetFriendlyErrorMessage(ex);
            string message = ex.Message;
            return $"<br><span style='color:#e74c3c;font-weight:bold;'>[ERROR] {friendlyMessage}</span><br><details><summary style='cursor:pointer,color:#888;'>[DETAILS]</summary><pre style='font-size:12px;color:#666;whitespace:pre-wrap;'>{ex}</pre></details>";
        }

        /// <summary>
        /// Returns a user-friendly error message that describes the specified exception.
        /// </summary>
        /// <param name="ex">The exception for which to generate a user-friendly error message.</param>
        /// <returns>The user-friendly error message describing the exception.</returns>
        private static string GetFriendlyErrorMessage(Exception ex)
        {
            return ex.GetType().Name switch
            {
                "AssertionException" => $"An assertion failed: {ex.Message}",
                "ElementClickInterceptedException" => "Element was found but could not be clicked. Another element may be covering it (e.g., a modal or overlay) or it may not be interactable.",
                "ElementNotInteractableException" => "Element was found but is not currently interactable. It may be hidden, disabled, or covered by another element.",
                "InvalidOperationException" => "The operation is not valid in the current state. This may occur if the WebDriver session has ended or if an action is attempted on a closed browser.",
                "InvalidSelectorException" => "Element locator is invalid. Check the syntax of the locator (e.g., XPath, CSS selector).",
                "NoSuchFrameException" => "Frame was not found. Check if the frame exists and if you have switched to the correct context.",
                "NoSuchElementException" => "Element was not found on the page. The locator may be incorrect or the element has not been loaded yet.",
                "NoSuchWindowException" => "Window or tab was not found. It might have been closed unexpectedly.",
                "StaleElementReferenceException" => "Element was found but is no longer attached to the DOM. The page may have been refreshed or updated since the element was located.",
                "UnhandledAlertException" => "An unexpected browser alert/popup was present. Handle the alert before interacting with the page.",
                "WebDriverTimeoutException" => "Timed out waiting for an element or page to load.",
                _ => ex.Message,
            };
        }
    }
}
