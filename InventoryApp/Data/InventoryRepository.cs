using InventoryApp.Models;

namespace InventoryApp.Data;

public class InventoryRepository
{
    private readonly List<Phone> _phones = new();
    private readonly List<SaleRecord> _sales = new();

    public IEnumerable<Phone> GetAllPhones() => _phones.OrderBy(p => p.Id);

    public bool AddPhone(Phone phone)
    {
        if (_phones.Any(p => p.Id == phone.Id))
        {
            return false;
        }

        _phones.Add(phone);
        return true;
    }

    public bool TryGetPhone(int id, out Phone? phone)
    {
        phone = _phones.FirstOrDefault(p => p.Id == id);
        return phone is not null;
    }

    public void UpdatePhone(Phone phone)
    {
        // Nothing to do here because Phone is a reference type and already updated.
    }

    public bool RemovePhone(int id)
    {
        var phone = _phones.FirstOrDefault(p => p.Id == id);
        if (phone is null)
        {
            return false;
        }

        _phones.Remove(phone);
        return true;
    }

    public void RegisterSale(SaleRecord sale)
    {
        _sales.Add(sale);
    }

    public IEnumerable<SaleRecord> GetSales() => _sales.AsReadOnly();
}
