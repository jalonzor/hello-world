# Control de Inventario de Teléfonos

Aplicación de consola escrita en C# para administrar el inventario de teléfonos celulares. Permite realizar altas, modificaciones y bajas de equipos, consultar el inventario, calcular el costo total, estimar el margen de venta proyectado y registrar salidas por venta.

## Estructura del proyecto

```
InventoryApp/
├── InventoryApp.csproj
├── Program.cs
├── Data/
│   └── InventoryRepository.cs
├── Models/
│   ├── Phone.cs
│   └── SaleRecord.cs
└── Services/
    ├── InventoryService.cs
    ├── ReportService.cs
    └── SalesService.cs
```

## Ejecución

1. Instala el SDK de .NET 6 o superior.
2. Desde la carpeta `InventoryApp` ejecuta:

```bash
dotnet run
```

El programa mostrará un menú con las opciones disponibles para administrar el inventario.
