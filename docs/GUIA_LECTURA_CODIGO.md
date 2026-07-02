# Guía rápida para leer y defender el código

Esta guía complementa los comentarios agregados al código. La idea es ubicar rápido dónde está cada parte importante del proyecto y qué se puede explicar en defensa.

## Flujo general

El sistema se divide en dos aplicaciones:

- `Proyecto_backend`: API REST en ASP.NET Core.
- `Proyecto_WPF`: aplicación de escritorio que consume la API.

El WPF nunca consulta SQL Server directamente. Todo pasa por la API:

```text
WPF -> ApiClientService -> Controllers -> Services -> Repositories -> SQL Server
```

## Crear una orden de venta

Archivos principales:

- `Proyecto_WPF/SalesPro.Wpf/ViewModels/NuevaOrdenViewModel.cs`
- `Proyecto_backend/SalesPro.Api/Controllers/OrdenesController.cs`
- `Proyecto_backend/SalesPro.Business/Services/OrdenService.cs`
- `Proyecto_backend/SalesPro.Data/Repositories/OrdenRepository.cs`

Qué decir:

- WPF arma una orden con cliente, empleado y productos.
- El frontend solo manda ids y cantidades.
- El backend vuelve a calcular precios, IVA, total e inventario.
- La transacción vive en `OrdenRepository`, porque ahí están las operaciones SQL.
- Si falla un detalle o no alcanza el stock, se hace rollback.

## Manejo transaccional

Archivo clave:

- `Proyecto_backend/SalesPro.Data/Repositories/OrdenRepository.cs`

Puntos de defensa:

- Se abre una conexión SQL.
- Se inicia `BeginTransaction`.
- Se valida cliente, empleado y productos.
- Se bloquea el producto con `UPDLOCK` y `ROWLOCK`.
- Se inserta encabezado.
- Se descuenta inventario.
- Se insertan detalles.
- Si todo sale bien, `Commit`.
- Si algo falla, `Rollback`.

## CRUD de cuentas bancarias

Archivos principales:

- `Proyecto_WPF/SalesPro.Wpf/ViewModels/CuentasBancariasViewModel.cs`
- `Proyecto_backend/SalesPro.Api/Controllers/CuentasBancariasController.cs`
- `Proyecto_backend/SalesPro.Business/Services/CuentaBancariaService.cs`
- `Proyecto_backend/SalesPro.Data/Repositories/CuentaBancariaRepository.cs`

Qué decir:

- El actualizador asignado al Equipo 5 es cuentas bancarias.
- El ViewModel maneja formulario, tabla y botones.
- El controller expone GET, POST, PUT y DELETE.
- El service valida reglas de negocio.
- El repository ejecuta SQL con ADO.NET.

## MVVM en WPF

Archivos principales:

- `Proyecto_WPF/SalesPro.Wpf/Infrastructure/ViewModelBase.cs`
- `Proyecto_WPF/SalesPro.Wpf/Infrastructure/RelayCommand.cs`
- `Proyecto_WPF/SalesPro.Wpf/Infrastructure/AsyncRelayCommand.cs`
- `Proyecto_WPF/SalesPro.Wpf/ViewModels/MainViewModel.cs`

Qué decir:

- Las vistas XAML se enlazan a propiedades del ViewModel.
- `ViewModelBase` notifica cambios con `INotifyPropertyChanged`.
- `RelayCommand` conecta botones con métodos normales.
- `AsyncRelayCommand` conecta botones con métodos async, por ejemplo llamadas HTTP.
- La vista no tiene lógica de negocio pesada.

## Consumo de API desde WPF

Archivo clave:

- `Proyecto_WPF/SalesPro.Wpf/Services/ApiClientService.cs`

Qué decir:

- Centraliza `HttpClient`.
- Lee la URL base desde `appsettings.json`.
- Serializa y deserializa JSON.
- Si la API devuelve error, extrae el mensaje para mostrarlo en pantalla.

## Manejo de errores

Archivo clave:

- `Proyecto_backend/SalesPro.Api/Middleware/ExceptionHandlingMiddleware.cs`

Qué decir:

- Las excepciones propias del dominio se convierten en respuestas HTTP claras.
- Validaciones devuelven 400.
- No encontrados devuelven 404.
- Conflictos devuelven 409.
- Errores no esperados devuelven 500 sin exponer detalles internos.

## Temas y accesibilidad

Archivos principales:

- `Proyecto_WPF/SalesPro.Wpf/App.xaml`
- `Proyecto_WPF/SalesPro.Wpf/Themes/DarkTheme.xaml`
- `Proyecto_WPF/SalesPro.Wpf/Themes/LightTheme.xaml`
- `Proyecto_WPF/SalesPro.Wpf/App.xaml.cs`

Qué decir:

- Los colores están en diccionarios de recursos.
- La aplicación puede alternar tema claro y oscuro.
- Las vistas usan `DynamicResource`, por eso el cambio se refleja sin reabrir la app.
- Se agregaron `AutomationProperties`, navegación por teclado y textos de ayuda para reforzar accesibilidad.

## Pruebas

Archivos principales:

- `Proyecto_backend/SalesPro.Business.Tests/CuentaBancariaServiceTests.cs`
- `Proyecto_backend/SalesPro.Business.Tests/OrdenServiceTests.cs`
- `Proyecto_backend/SalesPro.Api/SalesPro.Api.http`

Qué decir:

- Las pruebas unitarias verifican reglas de negocio.
- El archivo `.http` prueba endpoints reales de la API.
- La evidencia de transacciones demuestra que inventario y rollback funcionan contra SQL Server.
