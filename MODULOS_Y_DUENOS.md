# Módulos y dueños - Equipo 5

Este archivo existe para evitar desorden.

## Regla principal

```text
Cada persona trabaja solo en su rama y módulo asignado.
Todo cambio entra por Pull Request.
main no se toca directo.
```

## Asignación oficial

| Persona | Rama | Carpeta principal | Responsabilidad |
|---|---|---|---|
| Sebastián Cordero | `feature/sebas-db-transacciones` | `Proyecto_backend/database/`, `Proyecto_backend/SalesPro.Data/` | SQL Server, repositorios, transacciones, integración |
| Josue | `feature/josue-api-business` | `Proyecto_backend/SalesPro.Api/`, `Proyecto_backend/SalesPro.Business/` | Controllers, servicios de negocio, validaciones |
| Alejandro | `feature/alejandro-wpf` | `Proyecto_WPF/SalesPro.Wpf/` | Interfaz WPF, ViewModels, consumo API |
| Caleb | `feature/caleb-docs-http` | `docs/`, `.http` | Documentación, pruebas manuales API |

## Carpetas compartidas

Estas carpetas pueden requerir coordinación:

| Carpeta | Regla |
|---|---|
| `Proyecto_compartido/SalesPro.Contracts/` | Cambios solo si API/WPF necesitan DTOs nuevos. Avisar en PR. |
| `Proyecto_compartido/SalesPro.Domain/` | Cambios solo si el modelo de dominio cambia. Revisar conmigo. |
| `README.md` | Cambios de documentación general. Revisar conmigo. |

## No hacer

- No trabajar directo en `main`.
- No crear ramas nuevas sin avisar.
- No cambiar la transacción de orden sin revisión.
- No meter credenciales reales.
- No subir código que no se pueda explicar en revisión o defensa.
- No tocar módulos ajenos sin explicar.

## Checklist antes de PR

- [ ] Estoy en mi rama asignada.
- [ ] Leí el `README_MODULO.md` de la carpeta que toqué.
- [ ] Compilé.
- [ ] Probé lo que cambié.
- [ ] Puedo explicar el código.
- [ ] Si toqué otra carpeta, expliqué por qué.
