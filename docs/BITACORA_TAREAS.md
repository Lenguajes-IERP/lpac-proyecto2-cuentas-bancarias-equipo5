# Bitácora de tareas - Equipo 5

Este archivo registra el trabajo real realizado en el proyecto.

| Fecha | Persona | Rama | Tarea | Evidencia/PR | Estado |
|---|---|---|---|---|---|
| 2026-06-29 | Sebastian Cordero | `feature/sebas-db-transacciones` | Estructura base del proyecto: README, CONTRIBUTING, PLAN, Swagger, docs iniciales, ramas | commit `aef294e`, `a121f81`, `ce7099b`, `46172a1` | Completado |
| 2026-06-29 | Sebastian Cordero | `feature/sebas-db-transacciones` | Limpiar docs de organización innecesarios, actualizar nombres del equipo | commit `d44d345`, `a553bbf`, `7e9d734`, `a260d75` | Completado |
| 2026-06-30 | Sebastian Cordero | `feature/sebas-db-transacciones` | Estabilizar organización del proyecto y estructura de módulos | commit `cebaee4`, PR #1 | Completado |
| 2026-06-30 | Sebastian Cordero | `feature/sebas-db-transacciones` | Agregar accesibilidad básica en WPF (nombres accesibles, orden tabulación) | commit `e79a330` | Completado |
| 2026-06-30 | Sebastian Cordero | `feature/sebas-db-transacciones` | Agregar informe de avance técnico (borrador) | commit `d5f8b33` | Completado |
| 2026-06-30 | Sebastian Cordero | `feature/sebas-db-transacciones` | Agregar búsqueda y selección de cliente en la orden de venta (pop-up) | commit `6186da3`, PR #2 | Completado |
| 2026-06-30 | Sebastian Cordero | `feature/sebas-db-transacciones` | Separar proyecto backend y WPF en carpetas diferenciadas | commit `eed8d21`, PR #3 | Completado |
| 2026-07-01 | Sebastian Cordero | `feature/sebas-db-transacciones` | Preparar plantillas de evidencias y entregables finales | commit `9575639` | Completado |
| 2026-07-01 | Sebastian Cordero | `feature/sebas-db-transacciones` | Soportar configuración SQL Server del curso mediante `appsettings.Local.json` | commit `9238f7a` | Completado |
| 2026-07-01 | Josue Delgado | `feature/josue-api-business` | Completar casos `.http` del CRUD de cuentas bancarias | commit `446348d`, PR #4 | Completado |
| 2026-07-01 | Josue Delgado | `feature/josue-api-business` | Documentar respuestas HTTP de CuentasBancariasController | commit `9b56631` | Completado |
| 2026-07-01 | Josue Delgado | `feature/josue-api-business` | Agregar casos negativos de orden al `.http` | commit `f15f930` | Completado |
| 2026-07-01 | Sebastian Cordero | `main` | Mover Proyecto_compartido al backend según lineamientos | commit `a8b0d44` | Completado |
| 2026-07-01 | Josue Delgado | `feature/josue-api-business` | Documentar respuestas HTTP de Ordenes y Catalogos | commit `6c55371`, PR #5, #6 | Completado |
| 2026-07-01 | Caleb Hernández | `feature/caleb-docs-http` | Limpiar duplicados del `.http`, agregar casos error de PUT, actualizar bitácora e informe final | (este PR) | Completado |

## Como llenar esto
Esta bitácora resume el avance verificable del proyecto. No reemplaza la defensa individual; sirve como soporte para explicar qué se construyó y dónde se evidencia.

Nota de trazabilidad: hasta el cierre revisado, los aportes técnicos verificables por Git corresponden principalmente a Sebastián Cordero y Josué Delgado Corrales. No se registran aportes técnicos independientes de Caleb Hernández Vega ni Alejandro Porras en los commits revisados para esta versión del informe.

| Fecha | Persona / área | Rama o PR | Tarea | Evidencia | Estado |
|---|---|---|---|---|---|
| 2026-06-29 | Sebastián Cordero / base y control del repo | `feature/sebas-db-transacciones` | Organización inicial del proyecto, reglas de ramas, estructura base, base de datos y transacciones. | PRs de integración y commits de backend/data. | Integrado |
| 2026-06-29 | Equipo / estructura | `main` | Separación física de carpetas para entrega: `Proyecto_backend` y `Proyecto_WPF`. | `docs/ESTRUCTURA_ENTREGA.md`, `tools/crear-entregables.ps1`. | Integrado |
| 2026-06-30 | Backend/API | PR #6 / `feature/josue-api-business` | Documentación de respuestas HTTP y ajustes en endpoints de catálogos, cuentas bancarias y órdenes. | Commits `6c55371`, `9b56631`. | Integrado |
| 2026-06-30 | Pruebas API | `feature/sebas-db-transacciones` | Casos `.http` para CRUD de cuentas bancarias y órdenes, incluyendo casos negativos. | Commits `446348d`, `f15f930`. | Integrado |
| 2026-07-01 | Configuración SQL Server | `main` | Soporte para configuración local con `appsettings.Local.json` y script `setup-sqlserver.ps1`. | Commit `9238f7a`, `docs/CONFIGURACION_SQL_SERVER.md`. | Integrado |
| 2026-07-01 | Entrega | `main` | Generación de ZIPs separados para backend y WPF; validación de compilación desde los ZIPs. | `tools/crear-entregables.ps1`, carpeta `dist/`. | Integrado |
| 2026-07-01 | Documentación final | `main` | Limpieza de README, estructura de entrega, `.http`, bitácora e informe final base. | Documentos en `docs/`. | En cierre |
| 2026-07-01 | Pruebas finales API | `main` | Ejecución de pruebas contra SQL Server del curso: CRUD cuentas, orden válida y rollback por stock insuficiente. | `docs/evidencia_generada/api_transacciones_20260701164829.json`, `docs/EVIDENCIA_TRANSACCIONES.md`. | Ejecutado |

1. pone fecha;
2. pone su rama;
3. explica que hizo en una frase normal;
4. pega link del PR o commit;
5. marca estado.
## Pendientes para cierre final

- Probar WPF manualmente y pegar capturas.
- Completar conclusiones del informe con resultados reales.
- Exportar el informe final a PDF.
