using InventoryApp.Data;
using InventoryApp.Models;

namespace InventoryApp.Services;

public class InventoryService
{
    private readonly InventoryRepository _repository;

    public InventoryService(InventoryRepository repository)
    {
        _repository = repository;
    }

    public bool AddPhone(Phone phone) => _repository.AddPhone(phone);

    public bool TryGetPhone(int id, out Phone? phone) => _repository.TryGetPhone(id, out phone);

    public void UpdatePhone(Phone phone) => _repository.UpdatePhone(phone);

    public bool RemovePhone(int id) => _repository.RemovePhone(id);

    public IReadOnlyCollection<Phone> GetAllPhones() => _repository.GetAllPhones().ToList();
}
