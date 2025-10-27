using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Services;

namespace InventoryApp;

internal class Program
{
    private static void Main()
    {
        var repository = new InventoryRepository();
        var inventoryService = new InventoryService(repository);
        var salesService = new SalesService(repository);
        var reportService = new ReportService(repository, salesService);
        var app = new ConsoleApp(inventoryService, salesService, reportService);

        app.Run();
    }
}

internal class ConsoleApp
{
    private readonly InventoryService _inventoryService;
    private readonly SalesService _salesService;
    private readonly ReportService _reportService;

    public ConsoleApp(InventoryService inventoryService, SalesService salesService, ReportService reportService)
    {
        _inventoryService = inventoryService;
        _salesService = salesService;
        _reportService = reportService;
    }

    public void Run()
    {
        bool exit = false;
        while (!exit)
        {
            ShowMenu();
            Console.Write("Seleccione una opción: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    RegisterPhone();
                    break;
                case "2":
                    UpdatePhone();
                    break;
                case "3":
                    RemovePhone();
                    break;
                case "4":
                    ShowInventory();
                    break;
                case "5":
                    ShowInventoryCost();
                    break;
                case "6":
                    ShowSaleMargin();
                    break;
                case "7":
                    RegisterSale();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida, intente de nuevo.\n");
                    break;
            }
        }

        Console.WriteLine("Hasta luego");
    }

    private static void ShowMenu()
    {
        Console.WriteLine("==============================");
        Console.WriteLine(" Control de Inventario Móvil ");
        Console.WriteLine("==============================");
        Console.WriteLine("1. Alta de equipo");
        Console.WriteLine("2. Modificar equipo");
        Console.WriteLine("3. Baja de equipo");
        Console.WriteLine("4. Inventario disponible");
        Console.WriteLine("5. Costo total de inventario");
        Console.WriteLine("6. Margen de venta proyectado");
        Console.WriteLine("7. Registrar venta");
        Console.WriteLine("0. Salir");
    }

    private void RegisterPhone()
    {
        Console.WriteLine("\n--- Alta de equipo ---");
        var phone = new Phone
        {
            Id = ReadInt("ID del equipo: "),
            Brand = ReadRequiredString("Marca: "),
            Model = ReadRequiredString("Modelo: "),
            PurchasePrice = ReadDecimal("Costo de compra: "),
            SalePrice = ReadDecimal("Precio de venta: "),
            Quantity = ReadInt("Cantidad inicial: ")
        };

        if (_inventoryService.AddPhone(phone))
        {
            Console.WriteLine("Equipo agregado correctamente.\n");
        }
        else
        {
            Console.WriteLine("Ya existe un equipo con ese ID.\n");
        }
    }

    private void UpdatePhone()
    {
        Console.WriteLine("\n--- Modificar equipo ---");
        var id = ReadInt("ID del equipo a modificar: ");
        if (_inventoryService.TryGetPhone(id, out var phone))
        {
            Console.WriteLine($"Equipo actual: {phone!.Brand} {phone.Model} (Cantidad: {phone.Quantity})");
            var newBrand = ReadOptionalString($"Marca ({phone.Brand}): ");
            var newModel = ReadOptionalString($"Modelo ({phone.Model}): ");
            var newPurchasePrice = ReadOptionalDecimal($"Costo de compra ({phone.PurchasePrice:C}): ");
            var newSalePrice = ReadOptionalDecimal($"Precio de venta ({phone.SalePrice:C}): ");
            var newQuantity = ReadOptionalInt($"Cantidad ({phone.Quantity}): ");

            phone.Brand = newBrand ?? phone.Brand;
            phone.Model = newModel ?? phone.Model;
            phone.PurchasePrice = newPurchasePrice ?? phone.PurchasePrice;
            phone.SalePrice = newSalePrice ?? phone.SalePrice;
            phone.Quantity = newQuantity ?? phone.Quantity;

            _inventoryService.UpdatePhone(phone);
            Console.WriteLine("Equipo actualizado correctamente.\n");
        }
        else
        {
            Console.WriteLine("No se encontró un equipo con ese ID.\n");
        }
    }

    private void RemovePhone()
    {
        Console.WriteLine("\n--- Baja de equipo ---");
        var id = ReadInt("ID del equipo a eliminar: ");
        if (_inventoryService.RemovePhone(id))
        {
            Console.WriteLine("Equipo eliminado correctamente.\n");
        }
        else
        {
            Console.WriteLine("No se encontró un equipo con ese ID.\n");
        }
    }

    private void ShowInventory()
    {
        Console.WriteLine("\n--- Inventario disponible ---");
        var phones = _inventoryService.GetAllPhones();
        if (!phones.Any())
        {
            Console.WriteLine("No hay equipos registrados.\n");
            return;
        }

        foreach (var phone in phones)
        {
            Console.WriteLine($"ID: {phone.Id} | {phone.Brand} {phone.Model} | Cantidad: {phone.Quantity} | Compra: {phone.PurchasePrice:C} | Venta: {phone.SalePrice:C}");
        }

        Console.WriteLine();
    }

    private void ShowInventoryCost()
    {
        Console.WriteLine("\n--- Costo total de inventario ---");
        var totalCost = _reportService.CalculateTotalInventoryCost();
        Console.WriteLine($"Costo acumulado: {totalCost:C}\n");
    }

    private void ShowSaleMargin()
    {
        Console.WriteLine("\n--- Margen de venta proyectado ---");
        var margin = _reportService.CalculateProjectedMargin();
        Console.WriteLine($"Margen potencial: {margin:C}\n");
    }

    private void RegisterSale()
    {
        Console.WriteLine("\n--- Registrar venta ---");
        var id = ReadInt("ID del equipo vendido: ");
        var quantity = ReadInt("Cantidad vendida: ");

        var result = _salesService.RegisterSale(id, quantity);
        if (!result.Success)
        {
            Console.WriteLine($"No se pudo registrar la venta: {result.ErrorMessage}\n");
            return;
        }

        Console.WriteLine($"Venta registrada. Ingreso: {result.Revenue:C}\n");
    }

    private static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (int.TryParse(input, out var value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Ingrese un número entero válido.");
        }
    }

    private static int? ReadOptionalInt(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (int.TryParse(input, out var value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Ingrese un número entero válido.");
        }
    }

    private static decimal ReadDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (decimal.TryParse(input, out var value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Ingrese un valor decimal válido.");
        }
    }

    private static decimal? ReadOptionalDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (decimal.TryParse(input, out var value) && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Ingrese un valor decimal válido.");
        }
    }

    private static string ReadRequiredString(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("Este campo es obligatorio.");
        }
    }

    private static string? ReadOptionalString(string message)
    {
        Console.Write(message);
        var input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
    }
}
