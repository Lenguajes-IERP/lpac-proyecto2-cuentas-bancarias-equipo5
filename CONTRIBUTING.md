# Guía de trabajo - Equipo 5

Este proyecto es para el curso LPAC, Proyecto 2. Para evitar conflictos, nadie debe trabajar directo sobre `main`.

## Flujo obligatorio

1. Actualizar el repo.
2. Cambiarse a la rama asignada.
3. Hacer cambios solo en el módulo asignado.
4. Compilar antes de subir.
5. Abrir Pull Request.
6. Esperar revisión del responsable del módulo.

## Ramas oficiales asignadas

| Persona | Rama | Módulo |
|---|---|---|
| Sebastián | `feature/sebas-db-transacciones` | Base de datos, Data, transacciones |
| Josue | `feature/josue-api-business` | API, Business, Contracts necesarios |
| Alejandro | `feature/alejandro-wpf` | WPF |
| Caleb | `feature/caleb-docs-http` | Documentación y `.http` |

No crear ramas nuevas sin avisar.

No trabajar en `main`.

No trabajar en ramas ajenas.

## Comandos iniciales

```powershell
git clone https://github.com/Lenguajes-IERP/lpac-proyecto2-cuentas-bancarias-equipo5.git
cd lpac-proyecto2-cuentas-bancarias-equipo5
```

Luego cada persona se cambia a su rama:

```powershell
git checkout feature/alejandro-wpf
```

Antes de subir cambios:

```powershell
dotnet build SalesPro.slnx
git status
git add .
git commit -m "Describe el cambio"
git push
```

## Reglas por módulo

| Persona | GitHub | Responsable principal | Puede tocar |
|---|---|---|---|
| Sebastián Cordero | `@cbastiancq-lab` | Base de datos, transacciones, integración | `database/`, `SalesPro.Data`, documentación técnica |
| Caleb Hernández | `@CalebHv21` | Documentación, pruebas `.http`, apoyo WPF | `docs/`, `.http`, partes acordadas de WPF |
| Josue Delgado | `@JosueDelgadoCorrales` | API y reglas de negocio | `SalesPro.Api`, `SalesPro.Business`, DTOs acordados |
| Alejandro Porras | `@axpew` | Interfaz WPF | `SalesPro.Wpf` |

## Archivos de orientación

Antes de tocar un módulo, leer el archivo correspondiente:

```text
database/README_MODULO.md
src/SalesPro.Api/README_MODULO.md
src/SalesPro.Business/README_MODULO.md
src/SalesPro.Data/README_MODULO.md
src/SalesPro.Wpf/README_MODULO.md
src/SalesPro.Contracts/README_MODULO.md
src/SalesPro.Domain/README_MODULO.md
docs/README_MODULO.md
```

## Regla de oro

Si una persona necesita tocar un módulo que no le toca, debe explicarlo en el Pull Request.

Ejemplo:

```text
Tuve que modificar SalesPro.Contracts porque el endpoint nuevo necesitaba un DTO.
```

## Regla anti-despiche

No se acepta código que el autor no pueda explicar en defensa.

No se acepta código pegado por IA si:

- no compila;
- rompe otra capa;
- mete dependencias innecesarias;
- cambia archivos fuera del módulo asignado sin explicación;
- agrega credenciales o datos sensibles;
- elimina validaciones/transacciones existentes.

## Antes de abrir Pull Request

- [ ] Compilé sin errores.
- [ ] No dejé credenciales nuevas en el código.
- [ ] No rompí endpoints existentes.
- [ ] Leí el `README_MODULO.md` de la carpeta que toqué.
- [ ] Solo trabajé en mi rama asignada.
- [ ] Si cambié base de datos, actualicé `database/00_create_salespro.sql`.
- [ ] Si cambié API, actualicé `src/SalesPro.Api/SalesPro.Api.http`.
- [ ] Si cambié UI, probé WPF con API levantada.
- [ ] Puedo explicar el código que subí.
