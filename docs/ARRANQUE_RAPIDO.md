# Arranque rápido

Pasos mínimos para preparar, compilar y ejecutar el proyecto.

## 1. Clonar el proyecto

```powershell
git clone https://github.com/Lenguajes-IERP/lpac-proyecto2-cuentas-bancarias-equipo5.git
cd lpac-proyecto2-cuentas-bancarias-equipo5
```

## 2. Configurar SQL Server

Para el curso se debe usar SQL Server. El detalle está en:

```text
docs/CONFIGURACION_SQL_SERVER.md
```

Crear el archivo local:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.json
```

tomando como base:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.example.json
```

No subir `appsettings.Local.json` al repositorio.

## 3. Montar la base

Opción recomendada:

```powershell
powershell -ExecutionPolicy Bypass -File .\Proyecto_backend\scripts\setup-sqlserver.ps1 -Server "SERVIDOR_SQL" -User "USUARIO"
```

El script solicita la clave en pantalla.

Opción manual:

```text
Proyecto_backend/database/00_create_salespro.sql
```

ejecutado desde SQL Server Management Studio.

## 4. Compilar

```powershell
dotnet restore SalesPro.slnx
dotnet build SalesPro.slnx
```

## 5. Levantar API

```powershell
dotnet run --project .\Proyecto_backend\SalesPro.Api\SalesPro.Api.csproj
```

La API queda normalmente en:

```text
http://localhost:5294
```

## 6. Probar endpoints

Abrir y ejecutar:

```text
Proyecto_backend/SalesPro.Api/SalesPro.Api.http
```

## 7. Levantar WPF

En otra terminal, con la API viva:

```powershell
dotnet run --project .\Proyecto_WPF\SalesPro.Wpf\SalesPro.Wpf.csproj
```

## División técnica

```text
SalesPro.Domain      Entidades y excepciones del dominio
SalesPro.Contracts   DTOs/requests/responses entre API y WPF
SalesPro.Data        ADO.NET, SQL Server, repositorios y transacciones
SalesPro.Business    Validaciones y reglas
SalesPro.Api         Controllers REST y Swagger
SalesPro.Wpf         Vistas, ViewModels y consumo de API
database             Scripts SQL
docs                 Documentación, diagramas, bitácora y evidencias
```

## Puntos que no se deben romper

- La orden se procesa dentro de `SqlTransaction`.
- El inventario se descuenta dentro de la misma transacción.
- El IVA sale de `ParametroSistema`.
- Banco y compañía se seleccionan desde catálogos existentes.
- La búsqueda de cliente y producto debe existir en WPF.
- El archivo `.http` debe conservar casos exitosos y errores controlados.
