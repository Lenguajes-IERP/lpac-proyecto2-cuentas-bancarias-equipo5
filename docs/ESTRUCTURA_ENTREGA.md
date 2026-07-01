# Estructura física para entrega

El repositorio está separado según los entregables solicitados por Mediación Virtual.

## Proyecto_backend

Carpeta:

```text
Proyecto_backend/
```

Incluye:

- API REST ASP.NET Core;
- capa de negocio;
- capa de datos ADO.NET;
- dominio;
- contratos/DTOs;
- script SQL Server;
- scripts de preparación.

El ZIP generado para entrega es:

```text
dist/Proyecto_backend_Equipo5.zip
```

Dentro del ZIP se incluye:

```text
Proyecto_backend.slnx
```

para compilar únicamente backend.

## Proyecto_WPF

Carpeta:

```text
Proyecto_WPF/
```

Incluye:

- aplicación WPF;
- vistas;
- ViewModels;
- consumo de API.

El ZIP generado para entrega es:

```text
dist/Proyecto_WPF_Equipo5.zip
```

Dentro del ZIP se incluye:

```text
Proyecto_WPF.slnx
```

para compilar únicamente WPF. El paquete WPF incluye los contratos necesarios para compilar.

## Generación de entregables

Desde la raíz del repositorio:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\crear-entregables.ps1
```

El script copia los proyectos a una carpeta temporal, elimina `bin/obj` en la copia y genera los ZIP finales en `dist/`.
