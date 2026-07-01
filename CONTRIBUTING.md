# Guía de trabajo - Equipo 5

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
| Sebastián Cordero | `feature/sebas-db-transacciones` | Base de datos, Data, transacciones |
| Josue | `feature/josue-api-business` | API, Business, Contracts necesarios |
| Alejandro | `feature/alejandro-wpf` | WPF |
| Caleb | `feature/caleb-docs-http` | Documentación y `.http` |

## Comandos iniciales

```powershell
git clone https://github.com/Lenguajes-IERP/lpac-proyecto2-cuentas-bancarias-equipo5.git
cd lpac-proyecto2-cuentas-bancarias-equipo5
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
| YO | `@cbastiancq-lab` | Base de datos, transacciones, integración | `Proyecto_backend/database/`, `SalesPro.Data`, documentación técnica |
| Caleb Hernández | `@CalebHv21` | Documentación, pruebas `.http`, apoyo WPF | `docs/`, `.http`, partes acordadas de WPF |
| Josue Delgado | `@JosueDelgadoCorrales` | API y reglas de negocio | `SalesPro.Api`, `SalesPro.Business`, DTOs acordados |
| Alejandro Porras | `@axpew` | Interfaz WPF | `SalesPro.Wpf` |

## Archivos de orientación

```text
Proyecto_backend/database/README_MODULO.md
Proyecto_backend/SalesPro.Api/README_MODULO.md
Proyecto_backend/SalesPro.Business/README_MODULO.md
Proyecto_backend/SalesPro.Data/README_MODULO.md
Proyecto_backend/SalesPro.Contracts/README_MODULO.md
Proyecto_backend/SalesPro.Domain/README_MODULO.md
Proyecto_WPF/SalesPro.Wpf/README_MODULO.md
docs/README_MODULO.md
docs/ARRANQUE_RAPIDO.md
docs/GUIA_DEFENSA.md
docs/BITACORA_TAREAS.md
```

## Credenciales

Las credenciales reales del servidor del curso se configuran en:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.json
```

Ese archivo no se versiona en Git por seguridad. Si el entregable comprimido debe correr directamente contra el servidor del curso, se genera el ZIP desde la máquina donde exista ese archivo local.

## Regla de calidad

No se acepta código que el autor no pueda explicar en defensa.

No se acepta código si:

- no compila;
- rompe otra capa;
- mete dependencias innecesarias;
- cambia archivos fuera del módulo asignado sin explicación;
- elimina validaciones/transacciones existentes.

## Antes de abrir Pull Request

- [ ] Compilé sin errores.
- [ ] No rompí endpoints existentes.
- [ ] Leí el `README_MODULO.md` de la carpeta que toqué.
- [ ] Solo trabajé en mi rama asignada.
- [ ] Si cambié base de datos, actualicé `Proyecto_backend/database/00_create_salespro.sql`.
- [ ] Si cambié API, actualicé `Proyecto_backend/SalesPro.Api/SalesPro.Api.http`.
- [ ] Si cambié UI, probé WPF con API levantada.
- [ ] Puedo explicar el código que subí.
