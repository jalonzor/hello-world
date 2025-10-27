namespace InventoryApp.Models;

public class Phone
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public int Quantity { get; set; }

    public decimal InventoryCost => PurchasePrice * Quantity;
    public decimal PotentialMargin => (SalePrice - PurchasePrice) * Quantity;
}
