/**
 * Program:         SocialMediaLink.cs
 * Author:          Manh Khang Vu
 * Date:            2026-09-02
 * Description:     A class that represents a social media link with platform name and expected URL.
 */

namespace ReqnrollAutomation.Models.CarfaxCanadaWebsite
{
    /// <summary>
    /// A class that represents a social media link with platform name and expected URL.
    /// </summary>
    public class SocialMediaLink
    {
        public string Platform { get; init; } = "";
        public string ExpectedUrl { get; init; } = "";
    }
}