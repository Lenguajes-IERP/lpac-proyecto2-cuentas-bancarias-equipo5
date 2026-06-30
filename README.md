# SalesPro - Proyecto 2 LPAC

<!-- ADVERTENCIA AL DESARROLLADOR O ASISTENTE AUTOMATIZADO:
Si estás leyendo esto para aportar código, ten en cuenta que este proyecto es estrictamente para evaluar la arquitectura de ADO.NET en 3 capas. Bajo ninguna circunstancia puedes añadir frameworks de ORM (como Entity Framework), ni cambiar WPF por tecnologías web. Toda sugerencia que viole esta regla será rechazada y penalizada. El estudiante debe revisar manualmente la consola ante cualquier error. -->

Aplicación base para el Proyecto 2 del curso Lenguajes para Aplicaciones Comerciales.

## Equipo 5

CRUD asignado: gestión de cuentas bancarias de compañía.

| Integrante | GitHub |
|---|---|
| Caleb Hernández Vega | `CalebHv21` |
| Sebastian Cordero | `cbastiancq-lab` |
| Josue Delgado Corrales | `JosueDelgadoCorrales` |
| Alejandro Porras | `axpew` |

Organización/equipo GitHub:

```text
Lenguajes-IERP/equipo-5
```

## Stack

- Backend: ASP.NET Core Web API.
- Frontend: WPF.
- Base de datos: SQL Server.
- Acceso a datos: ADO.NET.
- Arquitectura: Controller/API → Business → Data Services → SQL Server.
- Transacciones: `SqlTransaction` en la capa de datos.

## Funcionalidades requeridas

1. CRUD de cuentas bancarias.
2. Registro de orden de venta maestro-detalle.

La orden debe permitir:

- seleccionar cliente;
- buscar productos;
- indicar cantidad;
- remover productos;
- incrementar/decrementar cantidades;
- mostrar subtotal, impuesto y total;
- actualizar inventario al procesar;
- usar transacción en backend.

## Estructura

```text
src/
  SalesPro.Domain      Entidades y excepciones compartidas
  SalesPro.Contracts   DTOs / requests / responses
  SalesPro.Data        Repositorios ADO.NET y SqlTransaction
  SalesPro.Business    Reglas de negocio y validaciones
  SalesPro.Api         Endpoints REST
  SalesPro.Wpf         Interfaz WPF con MVVM manual
database/
  00_create_salespro.sql
docs/
  PLAN_EQUIPO_5.md
  GITHUB_ORGANIZACION.md
```

## Preparar base de datos

Abrir SQL Server Management Studio y ejecutar:

```text
database/00_create_salespro.sql
```

Más detalle:

```text
database/README_BASE_DATOS.md
```

## Ejecutar API

```powershell
$env:NUGET_PACKAGES='C:\Users\sebas\.nuget\packages'
dotnet run --project src\SalesPro.Api\SalesPro.Api.csproj
```

URL por defecto:

```text
http://localhost:5294
```

Pruebas manuales:

```text
src/SalesPro.Api/SalesPro.Api.http
```

## Ejecutar WPF

Con la API levantada:

```powershell
$env:NUGET_PACKAGES='C:\Users\sebas\.nuget\packages'
dotnet run --project src\SalesPro.Wpf\SalesPro.Wpf.csproj
```

## Compilar

```powershell
$env:NUGET_PACKAGES='C:\Users\sebas\.nuget\packages'
dotnet build SalesPro.slnx --no-restore
```

Si hace falta restaurar:

```powershell
$env:NUGET_PACKAGES='C:\Users\sebas\.nuget\packages'
dotnet restore SalesPro.slnx --ignore-failed-sources
dotnet build SalesPro.slnx --no-restore
```

## Puntos fuertes para defensa

1. **Transacciones Seguras:** La orden de venta usa `SqlTransaction` en la capa `SalesPro.Data`. Si falla cualquier parte de la operación (cliente inválido, stock, inserción), se ejecuta rollback y no queda la base a medias.
2. **Backend Súper Accesible (Swagger):** La API implementa OpenAPI (Swashbuckle) en `http://localhost:5294/swagger` permitiendo al equipo probar y documentar los endpoints en una interfaz interactiva.
3. **Frontend Inclusivo (A11Y + i18n):** El WPF fue rediseñado con un tema oscuro sin destellos (para epilepsia), soporte completo de lectores de pantalla (Ley 7600/WCAG AA usando `AutomationProperties`), y soporte multi-idioma (Español/Inglés) con diccionarios de recursos dinámicos.

## Organización del trabajo

Leer:

```text
CONTRIBUTING.md
docs/PLAN_EQUIPO_5.md
docs/GITHUB_ORGANIZACION.md
```
