/**
 * Program:         InventoryItemDetails.cs
 * Author:          Manh Khang Vu
 * Date:            2026-08-14
 * Description:     A class that represents the details of an inventory item in Swag Labs.
 */

namespace ReqnrollAutomation.Models.SwagLabs
{
    /// <summary>
    /// A class that represents the details of an inventory item in Swag Labs.
    /// </summary>
    public class InventoryItemDetails
    {
        public string Name { get; init; } = "";
        public string Description { get; init; } = "";
        public decimal Price { get; init; }
    }
}
