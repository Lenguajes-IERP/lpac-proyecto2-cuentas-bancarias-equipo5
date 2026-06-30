# Arranque rapido

Este documento resume los pasos minimos para preparar el ambiente local, montar la base de datos, compilar y ejecutar el proyecto.

## 1. Clonar el proyecto

```powershell
git clone https://github.com/Lenguajes-IERP/lpac-proyecto2-cuentas-bancarias-equipo5.git
cd lpac-proyecto2-cuentas-bancarias-equipo5
```

## 2. Pasarse a su rama

Cada quien trabaja en su rama, no en `main`.

| Persona | Rama |
|---|---|
| YO | `feature/sebas-db-transacciones` |
| Josue | `feature/josue-api-business` |
| Alejandro | `feature/alejandro-wpf` |
| Caleb | `feature/caleb-docs-http` |

Ejemplo:

```powershell
git checkout feature/josue-api-business
```

Si la rama no les aparece:

```powershell
git fetch origin
git checkout feature/josue-api-business
```

## 3. Montar la base de datos

Ocupamos SQL Server / LocalDB. Para hacerlo facil, ya hay script:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\setup-localdb.ps1
```

Eso crea/actualiza la base `SalesPro`, carga bancos, compania, clientes, productos, cuentas bancarias demo y el parametro de IVA.

La API ya viene apuntando a:

```text
Server=(localdb)\MSSQLLocalDB;Database=SalesPro;Trusted_Connection=True;TrustServerCertificate=True;
```

## 4. Compilar

```powershell
dotnet restore SalesPro.slnx
dotnet build SalesPro.slnx
```

## 5. Levantar API

```powershell
dotnet run --project .\src\SalesPro.Api\SalesPro.Api.csproj
```

La API queda normalmente en:

```text
http://localhost:5294
```

## 6. Levantar WPF

En otra terminal, con la API viva:

```powershell
dotnet run --project .\src\SalesPro.Wpf\SalesPro.Wpf.csproj
```

## Como se divide el brete

Esto no es para inventar arquitectura nueva cada uno. La base del proyecto sigue la idea del Laboratorio 3:

```text
Domain       -> entidades del dominio
Contracts    -> DTOs/requests/responses entre API y WPF
Data         -> ADO.NET, SQL Server, repositorios y transacciones
Business     -> validaciones y reglas
Api          -> controllers REST
Wpf          -> pantallas, ViewModels y consumo de API
database     -> scripts SQL
docs         -> documentacion, diagramas, bitacora y pruebas
```

## Reparto por rama y puntos que esta cubriendo cada uno

### YO - base de datos, Data y transacciones

Rama:

```text
feature/sebas-db-transacciones
```

Toca:

- `database/`
- `src/SalesPro.Data/`
- revisar que `src/SalesPro.Domain/` y `src/SalesPro.Contracts/` calcen con la base
- transaccion de la orden

Puntos que defiende esta parte:

- 10% manejo de transacciones.
- Parte del 35% de registrar orden, porque al procesar la orden se debe actualizar inventario y si algo falla se hace rollback.
- Parte del 20% del CRUD de cuentas bancarias, porque la base tiene que soportar buscar, crear, editar y eliminar.
- Parte del 10% de dominio/trazabilidad, porque la BD debe calzar con las clases.

Reglas:

- La orden se procesa dentro de `SqlTransaction`.
- El inventario se descuenta dentro de la misma transaccion, no despues.
- El IVA sale de `ParametroSistema`, no quemado a lo loco en WPF.
- Bancos y companias se seleccionan desde datos existentes, no texto libre inventado.

### Josue - API y Business

Rama:

```text
feature/josue-api-business
```

Toca:

- `src/SalesPro.Api/`
- `src/SalesPro.Business/`
- endpoints REST
- validaciones antes de llegar a Data
- mensajes de error claros

Puntos que defiende esta parte:

- 20% CRUD correcto de cuentas bancarias.
- Parte del 35% de orden, porque WPF consume la API para crear la orden.
- Parte del 10% de pruebas `.http`, porque los endpoints tienen que poder probarse.
- Parte del 10% dominio/trazabilidad.

Minimo que debe quedar fino:

- `GET` listar/buscar cuentas bancarias.
- `GET` por id.
- `POST` crear cuenta.
- `PUT` editar cuenta.
- `DELETE` eliminar cuenta.
- Endpoints para catalogos: bancos, clientes, productos, companias.
- Endpoint para crear orden.
- Respuestas correctas: 200/201/400/404/409 segun corresponda.

### Alejandro - WPF

Rama:

```text
feature/alejandro-wpf
```

Toca:

- `src/SalesPro.Wpf/`
- vistas
- ViewModels
- consumo de API con `HttpClient`
- UX de CRUD y orden

Puntos que defiende esta parte:

- 35% registrar orden de trabajo.
- 20% CRUD de cuentas bancarias desde interfaz.
- Parte del 10% dominio/trazabilidad porque WPF debe usar contratos/modelos coherentes.

Ojo con esto porque la rubrica lo pregunta casi con lupa:

- Debe haber pantalla maestro-detalle de orden.
- Debe haber busqueda/seleccion de cliente con ventana/dialogo, no solo escribir un id a mano.
- Debe haber busqueda/agregado de producto con cantidad.
- Debe verse cliente y fecha.
- El grid debe mostrar codigo, nombre, precio unitario, cantidad y subtotal.
- Debe poder eliminar lineas.
- Debe poder subir/bajar cantidades.
- Debe verse impuesto de venta y total.
- Debe consumir la API, no simular datos quemados.

### Caleb - documentacion y pruebas `.http`

Rama:

```text
feature/caleb-docs-http
```

Toca:

- `docs/`
- `src/SalesPro.Api/SalesPro.Api.http`
- bitacora
- tabla de API REST
- diagramas ER y dominio
- conclusiones

Puntos que defiende esta parte:

- 15% documentacion.
- 10% pruebas de todos los recursos expuestos en `.http`.
- Parte del 10% dominio/trazabilidad, porque los diagramas tienen que calzar con el codigo.

La documentacion debe traer:

- portada
- tabla de contenidos
- objetivos
- especificacion de la necesidad
- cuadro de API RESTful con recursos, URLs y metodos
- entidad-relacion de BD
- modelo de dominio
- conclusiones
- bitacora de tareas

## Notas del profe del audio

Esto es lo que hay que respetar para no perder puntos por una tontera:

- Equipo 5 = gestion de cuentas bancarias.
- Cuenta bancaria debe manejar datos tipo numero, tipo de cuenta, moneda, estado, banco, compania y dueno/referencia segun el modelo.
- Banco y compania ya deben existir; no meter banco como texto libre.
- Para este proyecto, eliminar fisico esta bien si no estamos manejando auditoria/logica de borrado.
- Transacciones solo donde corresponde; la importante es procesar la orden.
- En la transaccion, si falla cualquier paso, rollback.
- El impuesto se calcula desde parametro del sistema/BD, no quemado en una pantalla.
- El archivo `.http` es para demostrar los endpoints y probar casos buenos y algunos malos.
- Stored procedures no son obligatorios si el enunciado no lo pide.
- La busqueda de producto/cliente puede ser popup, ventana o dialogo, pero tiene que existir.
- Si una tarea no se completa, se documenta en bitacora para coordinarla a tiempo.

## Regla de calidad

Todo cambio debe poder explicarse y defenderse. Regla simple:

- si no puede explicar el codigo, no se mergea;
- si mete arquitectura nueva que nadie pidio, no se mergea;
- si rompe otra capa sin avisar, no se mergea;
- si cambia transacciones/inventario/impuesto sin explicarlo, no se mergea.

