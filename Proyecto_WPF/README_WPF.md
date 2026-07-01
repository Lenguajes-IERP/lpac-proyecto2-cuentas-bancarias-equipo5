# Proyecto WPF

Este directorio contiene la aplicación de escritorio WPF del Proyecto 2.

## Contenido

```text
SalesPro.Wpf        Vistas, ViewModels, servicios HttpClient y modelos visuales
```

## Dependencias

La aplicación WPF referencia los contratos compartidos ubicados en:

```text
../Proyecto_backend/SalesPro.Contracts
```

`SalesPro.Contracts` referencia el dominio ubicado en:

```text
../Proyecto_backend/SalesPro.Domain
```

## Ejecución

Primero debe estar levantada la API del backend. Luego, desde la raíz del repositorio:

```powershell
dotnet run --project .\Proyecto_WPF\SalesPro.Wpf\SalesPro.Wpf.csproj
```
