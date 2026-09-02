/**
 * Program:         NavigationLink.cs
 * Author:          Manh Khang Vu
 * Date:            2026-09-02
 * Description:     A class that represents a navigation link with section, subsection, and expected URL.
 */

namespace ReqnrollAutomation.Models.CarfaxCanadaWebsite
{
    /// <summary>
    /// A class that represents a navigation link with section, subsection, and expected URL.
    /// </summary>
    public class NavigationLink
    {
        public string Section { get; init; } = "";
        public string SubSection { get; init; } = "";
        public string ExpectedUrl { get; init; } = "";
    }
}