# Evidencia: pruebas de transacciones y rollback

Fecha de ejecución: 2026-07-01  
Ambiente: API local contra SQL Server del curso  
API utilizada: `http://localhost:5294`  
Base de datos: `SalesPro` en SQL Server, administrada desde SQL Server Management Studio  
Archivo de respaldo generado: `docs/evidencia_generada/api_transacciones_20260701164829.json`

## Preparación

La API se levantó con:

```powershell
dotnet run --project .\Proyecto_backend\SalesPro.Api\SalesPro.Api.csproj --urls http://localhost:5294
```

La conexión real se tomó desde:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.json
```

## Validación de datos base

Consulta ejecutada en SQL Server Management Studio:

```sql
USE SalesPro;

SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto
ORDER BY product_id;

SELECT TOP 10 numero_orden, fk_cliente, total_orden, impuesto, fecha_orden
FROM Pos_Orden
ORDER BY numero_orden DESC;
```

Resultado inicial de inventario:

```text
product_id  nombre_etiqueta          existencia_en_stock
----------- ------------------------ -------------------
1           Laptop                   10
2           Mouse                    50
3           Keyboard                 30
4           Servicio instalacion     99
```

Datos base registrados:

```text
Bancos: 3
Compañías: 1
Clientes: 2
Productos: 4
Cuentas bancarias: 1
IVA: 13.0000
```

No existían órdenes previas en la base de datos recién preparada.

## Orden válida

Request ejecutado:

```http
POST http://localhost:5294/api/ordenes
Content-Type: application/json

{
  "clienteId": 1,
  "empleadoId": 1,
  "detalles": [
    { "productoId": 1, "cantidad": 1 },
    { "productoId": 2, "cantidad": 2 }
  ]
}
```

Respuesta HTTP esperada: `201 Created`.

Resultado obtenido:

```json
{
  "numeroOrden": 1,
  "clienteId": 1,
  "clienteNombre": "Valeria Mora",
  "empleadoId": 1,
  "subtotal": 625000.00,
  "impuesto": 81250.00,
  "total": 706250.00
}
```

IVA aplicado: 13%, tomado del parámetro `IVA` en `ParametroSistema`.

## Comprobación posterior a la orden válida

Consulta ejecutada:

```sql
USE SalesPro;

SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto
WHERE product_id IN (1, 2)
ORDER BY product_id;

SELECT numero_orden, fk_cliente, total_orden, impuesto, fecha_orden
FROM Pos_Orden
ORDER BY numero_orden DESC;

SELECT fk_pos_orden, fk_producto, cantidad, precio_unitario, precio_subtotal
FROM Pos_Orden_Detalle
ORDER BY fk_pos_orden DESC;
```

Resultado:

```text
product_id  nombre_etiqueta  existencia_en_stock
----------- ---------------- -------------------
1           Laptop           9
2           Mouse            48

numero_orden fk_cliente  total_orden  impuesto
------------ ----------- ------------ ------------
1            1           706250.00    81250.00

fk_pos_orden fk_producto cantidad  precio_unitario  precio_subtotal
------------ ----------- --------- ---------------- ---------------
1            2           2         12500.00         25000.00
1            1           1         600000.00        600000.00
```

Resultado: la orden válida insertó encabezado, detalles y descontó inventario correctamente dentro de una única operación.

## Rollback por inventario insuficiente

Request ejecutado:

```http
POST http://localhost:5294/api/ordenes
Content-Type: application/json

{
  "clienteId": 1,
  "empleadoId": 1,
  "detalles": [
    { "productoId": 1, "cantidad": 999999 }
  ]
}
```

Respuesta HTTP esperada: `409 Conflict`.

Respuesta obtenida:

```json
{
  "status": 409,
  "code": "conflict",
  "message": "Stock insuficiente para 'Laptop'. Disponible: 9, solicitado: 999999."
}
```

## Comprobación posterior al rollback

Consulta ejecutada:

```sql
USE SalesPro;

SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto
WHERE product_id = 1;

SELECT COUNT(*) AS total_ordenes
FROM Pos_Orden;
```

Resultado:

```text
product_id  nombre_etiqueta  existencia_en_stock
----------- ---------------- -------------------
1           Laptop           9

total_ordenes
-------------
1
```

Resultado: el stock de Laptop permaneció en 9 y no se creó ninguna orden parcial. El rollback funcionó correctamente.

## Conclusión

La transacción de orden de venta cumple el requisito del proyecto:

- La orden válida inserta encabezado y detalles, y descuenta inventario en una única operación.
- La orden inválida por stock insuficiente no deja datos parciales.
- La capa `SalesPro.Data` usa `SqlTransaction`, por lo que el `Commit` solo se ejecuta si todo el flujo termina correctamente.
- Si ocurre un error, se ejecuta `Rollback` y la base de datos mantiene su consistencia.

## Observación

La orden válida modifica inventario de forma real en la base del curso. Para repetir la prueba desde el mismo estado inicial, se debe restaurar la base o ajustar la evidencia al stock vigente.
