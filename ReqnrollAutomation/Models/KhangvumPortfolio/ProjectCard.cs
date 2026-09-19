/**
 * Program:         ProjectLink.cs
 * Author:          Manh Khang Vu
 * Date:            2026-09-18
 * Description:     A class that represents a project link on the Khangvum Portfolio website with a project name and expected URL.
 */

namespace ReqnrollAutomation.Models.KhangvumPortfolio
{
    /// <summary>
    /// A class that represents a project link on the Khangvum Portfolio website with a project name and expected URL.
    /// </summary>
    public class ProjectCard
    {
        public string ProjectName { get; init; } = "";
        public string ExpectedUrl { get; init; } = "";
    }
}
