# Proyecto backend

Este directorio contiene la aplicación ASP.NET Core Web API y las capas de backend del Proyecto 2.

## Contenido

```text
SalesPro.Api        Controladores REST, configuración, Swagger y .http
SalesPro.Business   Reglas de negocio y validaciones
SalesPro.Data       Repositorios ADO.NET y manejo transaccional
SalesPro.Domain     Entidades y excepciones del dominio
SalesPro.Contracts  DTOs, requests y responses compartidos con WPF
database/           Script SQL Server
scripts/            Scripts para preparar base de datos
```

## Configuración de base

Leer:

```text
docs/CONFIGURACION_SQL_SERVER.md
```

La conexión real se configura en:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.json
```

Ese archivo no se versiona.

## Ejecución

Desde la raíz del repositorio:

```powershell
dotnet run --project .\Proyecto_backend\SalesPro.Api\SalesPro.Api.csproj
```

Swagger:

```text
http://localhost:5294/swagger
```

Pruebas HTTP:

```text
Proyecto_backend/SalesPro.Api/SalesPro.Api.http
```
