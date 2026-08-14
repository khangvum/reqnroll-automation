/**
 * Program:         SwagLabsCredentials.cs
 * Author:          Manh Khang Vu
 * Date:            2026-06-10
 * Description:     A class that represents the credentials for Swag Labs.
 */
namespace ReqnrollAutomation.Models.SwagLabs
{
    /// <summary>
    /// A class that represents the credentials for Swag Labs.
    /// </summary>
    internal class SwagLabsCredentials
    {
        public Dictionary<string, string> Accounts { get; set; } = new();
        public string SharedPassword { get; set; } = "";
    }
}
