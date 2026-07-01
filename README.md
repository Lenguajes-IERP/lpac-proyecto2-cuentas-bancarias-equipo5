# SalesPro - Proyecto 2 LPAC

Aplicación para el Proyecto 2 del curso Lenguajes para Aplicaciones Comerciales. El sistema implementa un punto de venta con backend en ASP.NET Core Web API, frontend en WPF y base de datos SQL Server.

## Equipo 5

CRUD asignado: gestión de cuentas bancarias de compañía.

| Integrante | GitHub |
|---|---|
| Caleb Hernández Vega | `CalebHv21` |
| Sebastián Cordero | `cbastiancq-lab` |
| Josué Delgado Corrales | `JosueDelgadoCorrales` |
| Alejandro Porras | `axpew` |

## Stack

- Backend: ASP.NET Core Web API.
- Frontend: WPF.
- Base de datos: SQL Server.
- Acceso a datos: ADO.NET.
- Arquitectura: API/Controller → Business → Data → SQL Server.
- Transacciones: `SqlTransaction` en la capa de datos.

## Funcionalidades principales

1. CRUD de cuentas bancarias.
2. Registro de orden de venta con patrón maestro-detalle.

La orden permite:

- seleccionar cliente;
- buscar productos;
- indicar cantidad;
- remover productos;
- incrementar y decrementar cantidades;
- mostrar subtotal, impuesto y total;
- actualizar inventario al procesar;
- ejecutar el registro dentro de una transacción.

## Estructura del repositorio

```text
Proyecto_backend/
  SalesPro.Api         Endpoints REST, Swagger y configuración
  SalesPro.Business    Reglas de negocio y validaciones
  SalesPro.Data        Repositorios ADO.NET y SqlTransaction
  SalesPro.Domain      Entidades y excepciones del dominio
  SalesPro.Contracts   DTOs, requests y responses compartidos
  database/            Script SQL Server
  scripts/             Scripts de preparación de base

Proyecto_WPF/
  SalesPro.Wpf         Interfaz WPF con ViewModels y consumo de API

docs/
  ARRANQUE_RAPIDO.md
  CONFIGURACION_SQL_SERVER.md
  ESTRUCTURA_ENTREGA.md
  GUIA_DEFENSA.md
  BITACORA_TAREAS.md
  EVIDENCIA_TRANSACCIONES.md
  EVIDENCIA_CRUD_CUENTAS.md
  EVIDENCIA_ORDEN_WPF.md
```

## Configurar base de datos

Para el ambiente del curso, configurar SQL Server con:

```text
docs/CONFIGURACION_SQL_SERVER.md
```

El archivo local de conexión debe llamarse:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.json
```

Ese archivo no se sube al repositorio.

Para montar la base en SQL Server:

```powershell
powershell -ExecutionPolicy Bypass -File .\Proyecto_backend\scripts\setup-sqlserver.ps1 -Server "SERVIDOR_SQL" -User "USUARIO"
```

También se puede ejecutar manualmente en SQL Server Management Studio:

```text
Proyecto_backend/database/00_create_salespro.sql
```

## Ejecutar API

```powershell
dotnet run --project .\Proyecto_backend\SalesPro.Api\SalesPro.Api.csproj
```

URL por defecto:

```text
http://localhost:5294
```

Swagger:

```text
http://localhost:5294/swagger
```

Pruebas HTTP:

```text
Proyecto_backend/SalesPro.Api/SalesPro.Api.http
```

## Ejecutar WPF

Con la API levantada:

```powershell
dotnet run --project .\Proyecto_WPF\SalesPro.Wpf\SalesPro.Wpf.csproj
```

## Compilar

```powershell
dotnet restore SalesPro.slnx
dotnet build SalesPro.slnx
```

## Crear entregables comprimidos

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\crear-entregables.ps1
```

Genera:

```text
dist/Proyecto_backend_Equipo5.zip
dist/Proyecto_WPF_Equipo5.zip
```

## Puntos fuertes para defensa

1. La orden de venta usa `SqlTransaction`; si falla cualquier paso, se ejecuta rollback.
2. El inventario se descuenta dentro de la misma transacción que crea encabezado y detalles.
3. El IVA se obtiene desde `ParametroSistema`, no desde un valor quemado en WPF.
4. El CRUD de cuentas bancarias consume catálogos reales de bancos y compañías.
5. La interfaz WPF usa ViewModels y consume la API REST.
6. Las vistas incluyen medidas básicas de accesibilidad: nombres accesibles, ayudas, navegación por teclado y mensajes de estado.
