/**
 * Program:         HeaderLink.cs
 * Author:          Manh Khang Vu
 * Date:            2026-09-09
 * Description:     A class that represents a header link on the Khangvum Portfolio website with section and expected URL.
 */

namespace ReqnrollAutomation.Models.KhangvumPortfolio
{
    /// <summary>
    /// A class that represents a header link on the Khangvum Portfolio website with section and expected URL.
    /// </summary>
    public class HeaderLink
    {
        public string Section { get; init; } = "";
        public string ExpectedUrl { get; init; } = "";
    }
}
