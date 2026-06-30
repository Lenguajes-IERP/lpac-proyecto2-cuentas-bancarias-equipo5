# Proyecto 2 LPAC - Cuentas Bancarias Equipo 5

Aplicación para punto de venta del curso Lenguajes para Aplicaciones Comerciales.

## Equipo 5

CRUD asignado: gestión de cuentas bancarias de compañía.

| Integrante | GitHub | Rama asignada |
|---|---|---|
| Caleb Hernández Vega | `CalebHv21` | `feature/caleb-docs-http` |
| Sebastián Cordero | `cbastiancq-lab` | `feature/sebas-db-transacciones` |
| Josue Delgado Corrales | `JosueDelgadoCorrales` | `feature/josue-api-business` |
| Alejandro Porras | `axpew` | `feature/alejandro-wpf` |

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

## Mapa de responsabilidades

Cada módulo tiene un archivo `README_MODULO.md` con responsable, límites y reglas.

| Módulo | Carpeta | Responsable |
|---|---|---|
| Base de datos | `database/` | YO |
| Data / transacciones | `src/SalesPro.Data/` | YO |
| API REST | `src/SalesPro.Api/` | Josue |
| Business | `src/SalesPro.Business/` | Josue |
| WPF | `src/SalesPro.Wpf/` | Alejandro |
| Documentación | `docs/` | Caleb |
| Pruebas `.http` | `src/SalesPro.Api/SalesPro.Api.http` | Caleb |
| DTOs compartidos | `src/SalesPro.Contracts/` | Todos, con aviso en PR |
| Dominio | `src/SalesPro.Domain/` | YO |

Regla de equipo:

```text
Cada quien trabaja en su rama y módulo asignado.
Si alguien necesita tocar otro módulo, debe explicarlo en el Pull Request.
Si no puede explicar el código en defensa, no se acepta.
```

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
  ARRANQUE_RAPIDO.md
  GUIA_DEFENSA.md
  BITACORA_TAREAS.md
  GITHUB_ORGANIZACION.md
scripts/
  setup-localdb.ps1
```

## Preparar base de datos

Forma rápida:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\setup-localdb.ps1
```

Forma manual:

Abrir SQL Server Management Studio y ejecutar:

```text
database/00_create_salespro.sql
```

Más detalle:

```text
database/README_BASE_DATOS.md
database/README_MODULO.md
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

## Punto fuerte para defensa

La orden de venta usa `SqlTransaction` en la capa `SalesPro.Data`.

Si falla cualquier parte de la operación —cliente inválido, empleado inválido, producto inexistente, producto no vendible, stock insuficiente o error insertando detalle— se ejecuta rollback y no queda la base a medias.

## Organización del trabajo

Leer antes de trabajar:

```text
CONTRIBUTING.md
docs/ARRANQUE_RAPIDO.md
docs/PLAN_EQUIPO_5.md
docs/GUIA_DEFENSA.md
docs/BITACORA_TAREAS.md
docs/GITHUB_ORGANIZACION.md
```
