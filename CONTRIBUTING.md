# Guía de trabajo - Equipo 5

## Flujo obligatorio

1. Actualizar `main`.
2. Crear rama propia.
3. Hacer cambios solo en el módulo asignado.
4. Compilar antes de subir.
5. Abrir Pull Request.
6. Esperar revisión del responsable del módulo.

## Nombres de ramas

Usar este formato:

```text
feature/nombre-persona-modulo
fix/nombre-persona-descripcion
docs/nombre-persona-descripcion
```

Ejemplos:

```text
feature/sebas-database
feature/josue-api-cuentas
feature/caleb-tests-docs
feature/alejandro-wpf-orden
```

## Comando de compilación

```powershell
$env:NUGET_PACKAGES='C:\Users\sebas\.nuget\packages'
dotnet build SalesPro.slnx --no-restore
```

Si falla restore:

```powershell
$env:NUGET_PACKAGES='C:\Users\sebas\.nuget\packages'
dotnet restore SalesPro.slnx --ignore-failed-sources
dotnet build SalesPro.slnx --no-restore
```

## Reglas por módulo

| Persona | GitHub | Responsable principal | Puede tocar |
|---|---|---|---|
| Sebastián Cordero | `@cbastiancq-lab` | Base de datos, transacciones, integración | `database/`, `SalesPro.Data`, documentación técnica |
| Caleb Hernández | `@CalebHv21` | Documentación, pruebas `.http`, apoyo WPF | `docs/`, `.http`, partes acordadas de WPF |
| Josue Delgado | `@JosueDelgadoCorrales` | API y reglas de negocio | `SalesPro.Api`, `SalesPro.Business`, DTOs acordados |
| Alejandro Porras | `@axpew` | Interfaz WPF | `SalesPro.Wpf` |

## Regla de oro

Si una persona necesita tocar un módulo que no le toca, debe explicarlo en el Pull Request.

Ejemplo:

```text
Tuve que modificar SalesPro.Contracts porque el endpoint nuevo necesitaba un DTO.
```

## Antes de abrir Pull Request

- [ ] Compilé sin errores.
- [ ] No dejé credenciales nuevas en el código.
- [ ] No rompí endpoints existentes.
- [ ] Si cambié base de datos, actualicé `database/00_create_salespro.sql`.
- [ ] Si cambié API, actualicé `src/SalesPro.Api/SalesPro.Api.http`.
- [ ] Si cambié UI, probé WPF con API levantada.
