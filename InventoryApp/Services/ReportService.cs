using InventoryApp.Data;

namespace InventoryApp.Services;

public class ReportService
{
    private readonly InventoryRepository _repository;
    private readonly SalesService _salesService;

    public ReportService(InventoryRepository repository, SalesService salesService)
    {
        _repository = repository;
        _salesService = salesService;
    }

    public decimal CalculateTotalInventoryCost()
    {
        return _repository.GetAllPhones().Sum(phone => phone.InventoryCost);
    }

    public decimal CalculateProjectedMargin()
    {
        return _repository.GetAllPhones().Sum(phone => phone.PotentialMargin);
    }

    public decimal CalculateTotalRevenue()
    {
        return _salesService.GetSales().Sum(sale => sale.Revenue);
    }
}
