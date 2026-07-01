# Desarrollo de una aplicación web para gestión de punto de venta

Equipo 5  
Lenguajes para Aplicaciones Comerciales  
Proyecto 2  
Fecha de entrega: 2 de julio de 2026

## Integrantes

| Integrante | GitHub |
|---|---|
| Caleb Hernández Vega | `CalebHv21` |
| Sebastian Cordero | `cbastiancq-lab` |
| Josue Delgado Corrales | `JosueDelgadoCorrales` |
| Alejandro Porras | `axpew` |

## Resumen

El proyecto implementa una aplicación de punto de venta con backend en ASP.NET Core Web API y frontend en WPF. El backend expone servicios RESTful, aplica una arquitectura por capas y utiliza ADO.NET contra SQL Server. El frontend consume la API mediante ViewModels y permite ejecutar las dos funcionalidades solicitadas: el CRUD asignado al Equipo 5, correspondiente a cuentas bancarias, y el registro de una orden de venta con estructura maestro-detalle.

El registro de órdenes contempla selección de cliente, búsqueda de productos, cantidades, subtotal, impuesto, total e impacto sobre inventario. La persistencia de la orden se ejecuta dentro de una transacción SQL para evitar inconsistencias si ocurre un error durante la operación.

Estado de evidencia: se anexó evidencia real de API, CRUD de cuentas bancarias y transacciones contra SQL Server del curso. Quedan pendientes las capturas manuales de WPF para respaldar visualmente la interfaz.

## Objetivos

### Objetivo general

Desarrollar una aplicación de punto de venta con backend RESTful en ASP.NET Core Web API, frontend WPF y persistencia en SQL Server, cumpliendo los requerimientos funcionales asignados al Equipo 5.

### Objetivos específicos

- Implementar el CRUD de cuentas bancarias.
- Implementar una orden de venta maestro-detalle.
- Consumir la API REST desde WPF.
- Aplicar una arquitectura por capas: API, negocio, datos y dominio.
- Utilizar ADO.NET y transacciones SQL donde corresponda.
- Documentar endpoints, modelo entidad-relación, modelo de dominio, pruebas y bitácora.

## Especificación de la necesidad

El sistema permite administrar cuentas bancarias de compañía y registrar órdenes de venta. La orden debe asociarse a un cliente, contener productos con cantidades, calcular importes, procesarse contra la base de datos y actualizar inventario.

El sistema se divide en:

- Backend: expone API RESTful y ejecuta reglas de negocio.
- Capa de datos: consulta y modifica SQL Server mediante ADO.NET.
- WPF: consume la API y muestra las interfaces de cuentas bancarias y nueva orden.

## Arquitectura

| Capa | Proyecto | Responsabilidad |
|---|---|---|
| API | `SalesPro.Api` | Controladores REST, Swagger, configuración y manejo de errores. |
| Negocio | `SalesPro.Business` | Validaciones y reglas de negocio. |
| Datos | `SalesPro.Data` | Repositorios ADO.NET, consultas SQL y transacciones. |
| Dominio | `SalesPro.Domain` | Entidades base y excepciones del dominio. |
| Contratos | `SalesPro.Contracts` | DTOs, requests y responses compartidos con WPF. |
| Frontend | `SalesPro.Wpf` | Vistas WPF, ViewModels y consumo de API. |

## API RESTful desarrollada

| Recurso | URL | Método | Descripción | Respuestas principales |
|---|---|---:|---|---|
| Estado API | `/` | GET | Verifica que la API responde. | 200 |
| Bancos | `/api/catalogos/bancos` | GET | Lista bancos disponibles. | 200 |
| Compañías | `/api/catalogos/companias` | GET | Lista compañías disponibles. | 200 |
| Clientes | `/api/catalogos/clientes?buscar={texto}` | GET | Busca clientes activos. | 200 |
| Productos | `/api/catalogos/productos?buscar={texto}` | GET | Busca productos disponibles. | 200 |
| Cuentas bancarias | `/api/cuentas-bancarias` | GET | Lista o filtra cuentas bancarias. | 200 |
| Cuentas bancarias | `/api/cuentas-bancarias/{id}` | GET | Obtiene una cuenta por id. | 200, 404 |
| Cuentas bancarias | `/api/cuentas-bancarias` | POST | Crea una cuenta bancaria. | 201, 400, 409 |
| Cuentas bancarias | `/api/cuentas-bancarias/{id}` | PUT | Actualiza una cuenta bancaria. | 200, 400, 404, 409 |
| Cuentas bancarias | `/api/cuentas-bancarias/{id}` | DELETE | Elimina una cuenta bancaria. | 204, 404 |
| Órdenes | `/api/ordenes` | POST | Crea una orden de venta. | 201, 400, 404, 409 |
| Órdenes | `/api/ordenes/{numeroOrden}` | GET | Obtiene una orden por número. | 200, 404 |

El archivo de pruebas HTTP se encuentra en:

```text
Proyecto_backend/SalesPro.Api/SalesPro.Api.http
```

## Modelo entidad-relación

El modelo entidad-relación se documenta en:

```text
docs/diagramas/ER.md
```

Entidades principales:

- `Banco`
- `Compania`
- `Cliente`
- `Empleado`
- `Producto`
- `Compania_Cuenta_Bancaria`
- `Pos_Orden`
- `Pos_Orden_Detalle`
- `ParametroSistema`

## Modelo de dominio

El modelo de dominio se documenta en:

```text
docs/diagramas/DOMINIO.md
```

El diseño separa contratos de transporte, entidades del dominio, reglas de negocio y acceso a datos.

## Funcionalidades implementadas

### CRUD de cuentas bancarias

La funcionalidad permite:

- listar cuentas;
- buscar cuentas;
- crear cuenta bancaria;
- editar cuenta bancaria;
- eliminar cuenta bancaria;
- validar banco y compañía existentes;
- validar tipos de cuenta y divisa;
- evitar duplicados por banco y número de cuenta.

### Registro de orden

La funcionalidad permite:

- seleccionar cliente;
- buscar producto;
- agregar productos con cantidad;
- incrementar y decrementar cantidades;
- remover productos;
- visualizar subtotal, IVA estimado y total estimado;
- procesar la orden en backend;
- actualizar inventario al procesar.

## Manejo transaccional

La creación de órdenes se implementa en `SalesPro.Data.Repositories.OrdenRepository`. El flujo abre una conexión SQL, inicia una transacción con `BeginTransaction`, valida cliente, empleado y productos, calcula montos, inserta encabezado, descuenta inventario e inserta detalles.

Si ocurre una excepción antes del `Commit`, se ejecuta `Rollback`. Además, la lectura de productos para venta usa bloqueo con `UPDLOCK` y `ROWLOCK`, lo que ayuda a evitar inconsistencias durante el descuento de inventario.

La evidencia se anexó en:

```text
docs/EVIDENCIA_TRANSACCIONES.md
```

## Pruebas

| Área | Archivo de evidencia | Estado |
|---|---|---|
| API REST | `Proyecto_backend/SalesPro.Api/SalesPro.Api.http` | Preparado y ejecutado parcialmente mediante pruebas API |
| Transacciones | `docs/EVIDENCIA_TRANSACCIONES.md` | Evidencia real anexada |
| CRUD cuentas | `docs/EVIDENCIA_CRUD_CUENTAS.md` | Evidencia API anexada; capturas WPF pendientes |
| Orden | `docs/EVIDENCIA_ORDEN_WPF.md` | Evidencia API/transacción anexada; capturas WPF pendientes |

## Accesibilidad

Las vistas WPF incluyen medidas básicas de accesibilidad:

- nombres accesibles mediante `AutomationProperties.Name`;
- textos de ayuda mediante `AutomationProperties.HelpText`;
- navegación por tabulación;
- mensajes de estado con notificación para tecnologías de apoyo;
- controles identificables para acciones principales.

Estas medidas apoyan criterios de acceso y uso para personas con distintas necesidades, en línea con la preocupación indicada por el profesor sobre accesibilidad.

## Conclusiones

El proyecto cuenta con una estructura funcional alineada con el enunciado: backend RESTful, WPF, SQL Server, CRUD de cuentas bancarias y orden maestro-detalle. La parte transaccional se encuentra implementada en la capa de datos, que es donde corresponde según el diseño por capas.

Para cerrar la entrega, falta anexar las capturas manuales de WPF y exportar este informe a PDF. La evidencia de API, inventario y rollback ya fue registrada contra SQL Server del curso.

## Bitácora

La bitácora se encuentra en:

```text
docs/BITACORA_TAREAS.md
```

## Anexos

- Script SQL: `Proyecto_backend/database/00_create_salespro.sql`
- Pruebas HTTP: `Proyecto_backend/SalesPro.Api/SalesPro.Api.http`
- Diagrama ER: `docs/diagramas/ER.md`
- Modelo dominio: `docs/diagramas/DOMINIO.md`
- Evidencia transacciones: `docs/EVIDENCIA_TRANSACCIONES.md`
- Evidencia CRUD: `docs/EVIDENCIA_CRUD_CUENTAS.md`
- Evidencia orden WPF: `docs/EVIDENCIA_ORDEN_WPF.md`

## Referencias

Microsoft. (s. f.). *ASP.NET Core documentation*. Microsoft Learn.

Microsoft. (s. f.). *Windows Presentation Foundation documentation*. Microsoft Learn.

Microsoft. (s. f.). *SQL Server documentation*. Microsoft Learn.
