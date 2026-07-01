# Evidencia: pruebas de transacciones y rollback

Ejecutado: 2026-07-01. API en `http://localhost:5294`. Base de datos: LocalDB `(localdb)\MSSQLLocalDB`, base `SalesPro`.

## 1. Inventario antes de la prueba

```sql
USE SalesPro;
SELECT product_id, nombre_etiqueta, existencia_en_stock FROM Producto ORDER BY product_id;
```

```
product_id  nombre_etiqueta          existencia_en_stock
----------- ------------------------ -------------------
          1 Laptop                                    10
          2 Mouse                                     50
          3 Keyboard                                  30
          4 Servicio instalacion                      99
Fecha de ejecución: 2026-07-01  
Ambiente: API local contra SQL Server del curso  
Archivo generado: `docs/evidencia_generada/api_transacciones_20260701164829.json`

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

```sql
USE SalesPro;
SELECT TOP 10 numero_orden, fk_cliente, total_orden, impuesto, fecha_orden
FROM Pos_Orden ORDER BY numero_orden DESC;
```

Consulta previa contra SQL Server:

```text
Bancos: 3
Compañías: 1
Clientes: 2
Productos: 4
Cuentas bancarias: 1
IVA: 13.0000
```
(0 rows affected)
```

No existían órdenes previas en la base de datos recién creada.

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

Respuesta HTTP `201 Created`:

```json
{
  "numeroOrden": 1,
  "clienteId": 1,
  "clienteNombre": "Valeria Mora",
  "fechaOrden": "2026-07-01T16:58:16.5566287",
  "empleadoId": 1,
  "subtotal": 625000.00,
  "impuesto": 81250.00,
  "total": 706250.00,
  "detalles": [
    {
      "productoId": 1,
      "nombreProducto": "Laptop",
      "precioUnitario": 600000.00,
      "cantidad": 1,
      "subtotal": 600000.00,
      "impuesto": 78000.00
    },
    {
      "productoId": 2,
      "nombreProducto": "Mouse",
      "precioUnitario": 12500.00,
      "cantidad": 2,
      "subtotal": 25000.00,
      "impuesto": 3250.00
    }
  ]
}
```

IVA aplicado: 13% (tomado de `ParametroSistema`). Subtotal: ₡625 000, impuesto: ₡81 250, total: ₡706 250.

## 4. Comprobación posterior a la orden válida

```sql
USE SalesPro;

SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto WHERE product_id IN (1, 2) ORDER BY product_id;

SELECT numero_orden, fk_cliente, total_orden, impuesto, fecha_orden
FROM Pos_Orden ORDER BY numero_orden DESC;

SELECT fk_pos_orden, fk_producto, cantidad, precio_unitario, precio_subtotal
FROM Pos_Orden_Detalle ORDER BY fk_pos_orden DESC;
```

POST /api/ordenes
```

Body:

```json
{
  "clienteId": 1,
  "empleadoId": 1,
  "detalles": [
    { "productoId": 1, "cantidad": 1 },
    { "productoId": 2, "cantidad": 1 }
  ]
}
```

Resultado:

```text
Orden creada: 1
Total: 692125.00
Stock producto 1 antes: 10
Stock producto 1 después: 9
```
product_id  nombre_etiqueta  existencia_en_stock
----------- ---------------- -------------------
          1 Laptop                             9
          2 Mouse                             48

numero_orden fk_cliente  total_orden  impuesto     fecha_orden
------------ ----------- ------------ ------------ ----------------------------
           1           1    706250.00     81250.00  2026-07-01 16:58:16.556...

fk_pos_orden fk_producto cantidad  precio_unitario  precio_subtotal
------------ ----------- --------- ---------------- ---------------
           1           2         2        12500.00        25000.00
           1           1         1       600000.00       600000.00
```

**Resultado:** inventario descontado correctamente (Laptop: 10→9, Mouse: 50→48). Orden y detalles insertados.

Conclusión: la orden válida se creó y el inventario fue actualizado.

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

Respuesta HTTP `409 Conflict`:

```json
{
  "status": 409,
  "code": "conflict",
  "message": "Stock insuficiente para 'Laptop'. Disponible: 9, solicitado: 999999."
}
```

## 6. Comprobación del rollback

```sql
USE SalesPro;

SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto WHERE product_id = 1;

SELECT COUNT(*) AS total_ordenes FROM Pos_Orden;
```

```
product_id  nombre_etiqueta  existencia_en_stock
----------- ---------------- -------------------
          1 Laptop                             9

total_ordenes
-------------
            1
```

**Resultado:** el stock de Laptop permanece en 9 (no fue modificado). El total de órdenes sigue siendo 1 (no se creó ninguna orden parcial). El rollback funcionó correctamente: ningún registro quedó en la base de datos a partir del intento fallido.

## Conclusión

La transacción de orden de venta cumple los requisitos:

- La orden válida insertó el encabezado, los dos detalles y descontó el inventario de ambos productos en una sola operación atómica.
- La orden inválida (stock insuficiente) no creó ningún registro parcial: ni la orden, ni los detalles, ni el descuento de inventario persistieron.
- El `SqlTransaction` en la capa `SalesPro.Data` garantiza la integridad de las operaciones.
## Rollback por inventario insuficiente

Request ejecutado:

```http
POST /api/ordenes
```

Body:

```json
{
  "clienteId": 1,
  "empleadoId": 1,
  "detalles": [
    { "productoId": 1, "cantidad": 999999 }
  ]
}
```

Resultado:

```text
Código HTTP recibido: 409
Código esperado: 409
Stock producto 1 antes del intento inválido: 9
Stock producto 1 después del intento inválido: 9
Rollback mantiene stock: true
```

Conclusión: el intento inválido no descontó inventario. El estado de la base se mantuvo consistente, por lo que el rollback quedó evidenciado.

## Observación

La orden válida modifica inventario de forma real en la base del curso. Para repetir la prueba desde el mismo estado inicial, se debe restaurar la base o ajustar la evidencia al nuevo stock.
