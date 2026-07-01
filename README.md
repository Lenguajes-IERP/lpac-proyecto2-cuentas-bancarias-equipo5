# SalesPro - Proyecto 2 LPAC

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
Proyecto_backend/
  SalesPro.Api         Endpoints REST
  SalesPro.Business    Reglas de negocio y validaciones
  SalesPro.Data        Repositorios ADO.NET y SqlTransaction
  database/            Script SQL Server
  scripts/             Utilidades de preparacion local
Proyecto_WPF/
  SalesPro.Wpf         Interfaz WPF con MVVM manual
Proyecto_compartido/
  SalesPro.Domain      Entidades y excepciones compartidas
  SalesPro.Contracts   DTOs / requests / responses
docs/
  PLAN_EQUIPO_5.md
  ARRANQUE_RAPIDO.md
  ESTRUCTURA_ENTREGA.md
  GUIA_DEFENSA.md
  BITACORA_TAREAS.md
```

## Preparar base de datos

Forma rápida:

```powershell
powershell -ExecutionPolicy Bypass -File .\Proyecto_backend\scripts\setup-localdb.ps1
```

Forma manual:

Abrir SQL Server Management Studio y ejecutar:

```text
Proyecto_backend/database/00_create_salespro.sql
```

Más detalle:

```text
Proyecto_backend/database/README_BASE_DATOS.md
```

## Ejecutar API

```powershell
$env:NUGET_PACKAGES='C:\Users\sebas\.nuget\packages'
dotnet run --project Proyecto_backend\SalesPro.Api\SalesPro.Api.csproj
```

URL por defecto:

```text
http://localhost:5294
```

Pruebas manuales:

```text
Proyecto_backend/SalesPro.Api/SalesPro.Api.http
```

## Ejecutar WPF

Con la API levantada:

```powershell
$env:NUGET_PACKAGES='C:\Users\sebas\.nuget\packages'
dotnet run --project Proyecto_WPF\SalesPro.Wpf\SalesPro.Wpf.csproj
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
2. **Documentación interactiva de API:** La API implementa OpenAPI/Swagger en `http://localhost:5294/swagger`, lo que permite revisar y probar endpoints durante el desarrollo.
3. **Accesibilidad digital:** Las vistas WPF incluyen nombres accesibles, ayudas de controles, orden de tabulación y mensajes de estado para apoyar navegación por teclado y lectores de pantalla, en línea con criterios de accesibilidad asociados a la Ley 7600.
4. **Arquitectura por capas:** El proyecto separa dominio, contratos, datos, negocio, API y WPF para mantener trazabilidad entre diseño, código y pruebas.

## Organización del trabajo

Leer:

```text
CONTRIBUTING.md
docs/ARRANQUE_RAPIDO.md
docs/ESTRUCTURA_ENTREGA.md
docs/PLAN_EQUIPO_5.md
docs/GUIA_DEFENSA.md
docs/BITACORA_TAREAS.md
```
