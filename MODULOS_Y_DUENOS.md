# Módulos y dueños - Equipo 5

Este archivo existe para evitar desorden.

## Regla principal

```text
Cada persona trabaja en su rama y módulo asignado.
Todo cambio entra por Pull Request.
main no se toca directo salvo integración autorizada.
```

## Asignación oficial

| Persona | Rama | Carpeta principal | Responsabilidad |
|---|---|---|---|
| Sebastián Cordero | `feature/sebas-db-transacciones` | `Proyecto_backend/database/`, `Proyecto_backend/SalesPro.Data/` | SQL Server, repositorios, transacciones, integración |
| Josue | `feature/josue-api-business` | `Proyecto_backend/SalesPro.Api/`, `Proyecto_backend/SalesPro.Business/` | Controllers, servicios de negocio, validaciones |
| Alejandro | `feature/alejandro-wpf` | `Proyecto_WPF/SalesPro.Wpf/` | Interfaz WPF, ViewModels, consumo API |
| Caleb | `feature/caleb-docs-http` | `docs/`, `.http` | Documentación, pruebas manuales API |

## Carpetas de coordinación

| Carpeta | Regla |
|---|---|
| `Proyecto_backend/SalesPro.Contracts/` | Cambios solo si API/WPF necesitan DTOs nuevos. Avisar en PR. |
| `Proyecto_backend/SalesPro.Domain/` | Cambios solo si el modelo de dominio cambia. Revisar impacto. |
| `README.md` | Cambios de documentación general. Revisar antes de entregar. |

## Credenciales reales

Las credenciales reales del DBMS del curso van en:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.json
```

Ese archivo queda fuera de Git por seguridad. Para entregar ZIPs ejecutables contra el servidor del curso, generar los ZIPs desde la máquina que tenga ese archivo local.

## No hacer

- No trabajar directo en `main` sin autorización.
- No crear ramas nuevas sin avisar.
- No cambiar la transacción de orden sin revisión.
- No subir contraseñas reales al historial de Git.
- No subir código que no se pueda explicar en revisión o defensa.
- No tocar módulos ajenos sin explicar.

## Checklist antes de PR

- [ ] Estoy en mi rama asignada.
- [ ] Leí el `README_MODULO.md` de la carpeta que toqué.
- [ ] Compilé.
- [ ] Probé lo que cambié.
- [ ] Puedo explicar el código.
- [ ] Si toqué otra carpeta, expliqué por qué.
