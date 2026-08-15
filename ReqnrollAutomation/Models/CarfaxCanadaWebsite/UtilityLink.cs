/**
 * Program:         UtilityLink.cs
 * Author:          Manh Khang Vu
 * Date:            2026-08-15
 * Description:     A class that represents a utility link with item name and expected URL.
 */

namespace ReqnrollAutomation.Models.CarfaxCanadaWebsite
{
    /// <summary>
    /// A class that represents a utility link with item name and expected URL.
    /// </summary>
    public class UtilityLink
    {
        public string UtilityItem { get; init; } = "";
        public string ExpectedUrl { get; init; } = "";
    }
}