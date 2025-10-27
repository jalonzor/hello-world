namespace InventoryApp.Models;

public class SaleRecord
{
    public int PhoneId { get; set; }
    public int Quantity { get; set; }
    public decimal Revenue { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
