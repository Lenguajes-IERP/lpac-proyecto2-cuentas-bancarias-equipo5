# Módulo API REST

Responsable principal:

```text
Josue Delgado Corrales (@JosueDelgadoCorrales)
```

Revisor obligatorio:

```text
YO (@cbastiancq-lab)
```

## Qué se puede tocar aquí

- Controllers REST.
- Registro de servicios en `Program.cs`.
- Middleware de errores.
- Configuración general del API.
- Archivo `.http` si el cambio afecta endpoints.

## Qué NO se debe hacer aquí

- No escribir SQL directo en controllers.
- No calcular reglas de negocio grandes en controllers.
- No modificar WPF desde esta carpeta.
- No poner credenciales reales en `appsettings.json`.

## Regla de defensa

Si un endpoint falla, debe devolver un error claro:

- `400` para validaciones.
- `404` para datos inexistentes.
- `409` para conflictos como duplicados o stock insuficiente.
- `500` solo para errores inesperados.
