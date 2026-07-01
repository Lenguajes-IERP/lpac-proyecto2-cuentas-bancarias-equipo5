# Plan de trabajo - Proyecto 2 Equipo 5

Este plan define la organizacion del trabajo del Proyecto 2. Cada integrante trabaja en su rama y modulo asignado; si necesita modificar otro modulo, debe explicarlo en el Pull Request.

Primero leer:

```text
docs/ARRANQUE_RAPIDO.md
docs/GUIA_DEFENSA.md
docs/BITACORA_TAREAS.md
```

## Que estamos haciendo

Proyecto 2 de LPAC:

- Backend: ASP.NET Core Web API.
- Frontend: WPF.
- Base de datos: SQL Server.
- Acceso a datos: ADO.NET.
- Arquitectura: Controller/API -> Business -> Data -> SQL Server.
- Frontend WPF con ViewModel/MVVM.
- Equipo 5: CRUD de cuentas bancarias.
- Segunda funcionalidad obligatoria: registrar orden de trabajo/venta maestro-detalle.

## Estructura base tipo Laboratorio 3

La idea es parecida al Laboratorio 3, solo que adaptada a este proyecto:

| Capa | Proyecto/carpeta | Para que sirve |
|---|---|---|
| Dominio | `src/SalesPro.Domain/` | Entidades y excepciones del negocio |
| Contratos | `src/SalesPro.Contracts/` | DTOs, requests y responses entre API y WPF |
| Datos | `src/SalesPro.Data/` | Repositorios ADO.NET, SQL Server y transacciones |
| Negocio | `src/SalesPro.Business/` | Validaciones y reglas antes de guardar |
| API | `src/SalesPro.Api/` | Controllers REST |
| WPF | `src/SalesPro.Wpf/` | Vistas, ViewModels y consumo de API |
| BD | `database/` | Script SQL Server |
| Docs | `docs/` | Documentacion, diagramas, bitacora y plan |

No hay que inventar otra arquitectura. Si se ocupa cambiar algo estructural, se habla primero.

## Puntos de la rubrica

| Rubro | Valor | Que tenemos que demostrar |
|---|---:|---|
| Documentacion | 15% | Portada, TOC, objetivos, necesidad, API, ER, dominio, conclusiones, bitacora |
| Transacciones | 10% | Orden con commit/rollback real en Data |
| Pruebas `.http` | 10% | Todos los recursos REST probados |
| Dominio y trazabilidad | 10% | Clases coherentes en backend/frontend y calzando con diseno |
| CRUD cuentas bancarias | 20% | Buscar, crear, editar y eliminar |
| Registrar orden | 35% | Maestro-detalle, cliente, productos, cantidades, impuesto, total, inventario y API |

## Reparto por rama

### YO - base de datos, Data y transacciones

Rama:

```text
feature/sebas-db-transacciones
```

Toca:

- `database/`
- `src/SalesPro.Data/`
- apoyar `src/SalesPro.Domain/`
- apoyar `src/SalesPro.Contracts/`
- revisar integracion final

Debe quedar:

- base `SalesPro` montable con `scripts/setup-localdb.ps1`;
- tablas y datos semilla listos;
- cuentas bancarias relacionadas con banco y compania;
- IVA como parametro del sistema;
- orden con `SqlTransaction`;
- descuento de inventario dentro de la transaccion;
- rollback si falla cliente, producto, stock o detalle.

Puntos que cubre:

- 10% transacciones;
- parte fuerte del 35% de orden;
- soporte del 20% CRUD;
- soporte del 10% dominio/trazabilidad.

### Josue - API y Business

Rama:

```text
feature/josue-api-business
```

Toca:

- `src/SalesPro.Api/`
- `src/SalesPro.Business/`
- `src/SalesPro.Contracts/` solo si ocupa DTOs nuevos

Debe quedar:

- controllers REST limpios;
- validaciones en Business;
- mensajes de error entendibles;
- endpoints de cuentas bancarias completos;
- endpoints de catalogos para WPF;
- endpoint de crear orden;
- codigos HTTP correctos.

Puntos que cubre:

- 20% CRUD;
- parte del 35% orden;
- parte del 10% `.http`;
- parte del 10% dominio/trazabilidad.

### Alejandro - WPF

Rama:

```text
feature/alejandro-wpf
```

Toca:

- `src/SalesPro.Wpf/Views/`
- `src/SalesPro.Wpf/ViewModels/`
- `src/SalesPro.Wpf/Services/`

Debe quedar:

- pantalla CRUD de cuentas bancarias consumiendo API;
- pantalla orden maestro-detalle;
- busqueda/seleccion de cliente en ventana/dialogo;
- busqueda/agregado de producto en ventana/dialogo;
- cantidad editable;
- eliminar producto de la orden;
- subir/bajar cantidades;
- mostrar subtotal, impuesto y total;
- mensajes decentes cuando API falle.

Puntos que cubre:

- 35% orden;
- 20% CRUD;
- parte del 10% dominio/trazabilidad.

### Caleb - documentacion y pruebas `.http`

Rama:

```text
feature/caleb-docs-http
```

Toca:

- `docs/`
- `src/SalesPro.Api/SalesPro.Api.http`
- tablas de API;
- diagramas ER/dominio;
- bitacora.

Debe quedar:

- documentacion completa del enunciado;
- tabla REST con recurso, URL y metodos;
- pruebas `.http` para cuentas bancarias, catalogos y orden;
- pruebas de casos malos: id inexistente, datos invalidos, stock insuficiente;
- bitacora real por persona.

Puntos que cubre:

- 15% documentacion;
- 10% `.http`;
- parte del 10% dominio/trazabilidad.

## Notas del profe/audio que hay que respetar

- Nuestro CRUD asignado es gestion de cuentas bancarias.
- Cuenta bancaria no es solo un numero: debe tener banco, compania, tipo, moneda, estado y datos del dueno/referencia segun el modelo.
- Banco y compania deben existir; no se deben escribir como texto libre.
- Para este proyecto se permite eliminar fisico si no estamos manejando auditoria.
- Las transacciones van donde corresponden; la mas importante es procesar la orden.
- Si falla algo en la orden, rollback.
- El impuesto debe salir de un parametro/tablas de la base, no quemado en WPF.
- `.http` se usa para demostrar y probar la API.
- Stored procedures no son obligatorios si el enunciado no los exige.
- Busqueda de cliente/producto puede ser ventana, popup o dialogo, pero debe existir.
- Si alguien no hace su parte, se apunta en bitacora y se habla con el profe.

## Pendientes criticos antes de entregar

- [x] Base inicial del proyecto por capas.
- [x] Script SQL Server base.
- [x] Script rapido para montar LocalDB.
- [x] Ramas por persona.
- [x] Proteccion de `main` por Pull Request.
- [ ] Abrir/mergear PR de base inicial.
- [ ] CRUD completo cuentas bancarias probado en WPF.
- [ ] Busqueda de cliente en WPF.
- [ ] Busqueda de producto en WPF revisada contra rubrica.
- [ ] Orden completa creando encabezado/detalle.
- [ ] Descuento de inventario probado.
- [ ] Rollback por stock insuficiente probado.
- [ ] `.http` completo.
- [ ] Documentacion final completa.

## Regla final

Main no se modifica directamente. Si un cambio no se puede explicar en defensa, se devuelve el Pull Request. Es preferible codigo claro y defendible que una implementacion grande sin trazabilidad.

