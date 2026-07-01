# Proyecto backend

Este directorio contiene la aplicación ASP.NET Core Web API y las capas de backend del Proyecto 2.

## Contenido

```text
SalesPro.Api        Controladores REST, configuración y Swagger
SalesPro.Business   Reglas de negocio y validaciones
SalesPro.Data       Repositorios ADO.NET y manejo transaccional
database/           Script SQL Server
scripts/            Script de preparación local de base de datos
```

## Dependencias compartidas

El backend referencia proyectos ubicados en:

```text
../Proyecto_compartido/SalesPro.Domain
../Proyecto_compartido/SalesPro.Contracts
```

## Ejecución

Desde la raíz del repositorio:

```powershell
powershell -ExecutionPolicy Bypass -File .\Proyecto_backend\scripts\setup-localdb.ps1
dotnet run --project .\Proyecto_backend\SalesPro.Api\SalesPro.Api.csproj
```

Swagger:

```text
http://localhost:5294/swagger
```

