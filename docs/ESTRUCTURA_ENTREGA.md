# Estructura física para entrega

El repositorio quedó separado físicamente según los entregables solicitados.

## Proyecto_backend

Carpeta:

```text
Proyecto_backend/
```

Incluye:

- API REST ASP.NET Core;
- capa de negocio;
- capa de datos ADO.NET;
- script SQL Server;
- script para preparar LocalDB.

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

## Proyecto_compartido

Carpeta:

```text
Proyecto_compartido/
```

Incluye:

- dominio;
- contratos/DTOs compartidos entre backend y WPF.

Esta carpeta debe incluirse junto a los dos proyectos al compilar localmente. Para la entrega comprimida, se puede copiar `Proyecto_compartido` dentro de cada paquete o incluirlo como carpeta compartida según indique el profesor.

