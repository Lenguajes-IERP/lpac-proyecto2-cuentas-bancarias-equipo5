# Proyecto WPF

Este directorio contiene la aplicación de escritorio WPF del Proyecto 2.

## Contenido

```text
SalesPro.Wpf        Vistas, ViewModels, servicios HttpClient y modelos visuales
```

## Dependencias compartidas

La aplicación WPF referencia:

```text
../Proyecto_compartido/SalesPro.Contracts
```

El proyecto `SalesPro.Contracts` referencia a su vez el dominio compartido.

## Ejecución

Primero debe estar levantada la API del backend. Luego, desde la raíz del repositorio:

```powershell
dotnet run --project .\Proyecto_WPF\SalesPro.Wpf\SalesPro.Wpf.csproj
```

