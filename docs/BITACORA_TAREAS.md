# Bitácora de tareas - Equipo 5

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

## Pendientes para cierre final

- Probar WPF manualmente y pegar capturas.
- Completar conclusiones del informe con resultados reales.
- Exportar el informe final a PDF.
