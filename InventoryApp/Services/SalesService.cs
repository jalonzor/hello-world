using InventoryApp.Data;
using InventoryApp.Models;

namespace InventoryApp.Services;

public class SalesService
{
    private readonly InventoryRepository _repository;

    public SalesService(InventoryRepository repository)
    {
        _repository = repository;
    }

    public SaleResult RegisterSale(int phoneId, int quantity)
    {
        if (quantity <= 0)
        {
            return SaleResult.Failure("La cantidad debe ser mayor a cero.");
        }

        if (!_repository.TryGetPhone(phoneId, out var phone) || phone is null)
        {
            return SaleResult.Failure("El equipo no existe.");
        }

        if (phone.Quantity < quantity)
        {
            return SaleResult.Failure("Inventario insuficiente.");
        }

        phone.Quantity -= quantity;
        var revenue = phone.SalePrice * quantity;
        _repository.RegisterSale(new SaleRecord
        {
            PhoneId = phoneId,
            Quantity = quantity,
            Revenue = revenue
        });

        return SaleResult.Successful(revenue);
    }

    public IReadOnlyCollection<SaleRecord> GetSales() => _repository.GetSales().ToList();
}

public record SaleResult(bool Success, decimal Revenue, string? ErrorMessage)
{
    public static SaleResult Successful(decimal revenue) => new(true, revenue, null);

    public static SaleResult Failure(string message) => new(false, 0m, message);
}
