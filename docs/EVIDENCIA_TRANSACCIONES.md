# Plantilla de evidencia: pruebas de transacciones y rollback

Este documento sirve para registrar evidencia real de que la creación de órdenes se ejecuta dentro de una transacción y que, ante un error consistente, no quedan registros parciales.

Estado actual: pendiente de completar con resultados reales de ejecución.

## Preparación

- Levantar la base de datos `SalesPro`.
- Levantar la API:

```powershell
dotnet run --project .\Proyecto_backend\SalesPro.Api\SalesPro.Api.csproj
```

- Ejecutar los requests desde [SalesPro.Api.http](../Proyecto_backend/SalesPro.Api/SalesPro.Api.http).
- Pegar aquí las respuestas HTTP y consultas SQL obtenidas.

## 1. Inventario antes de la prueba

```sql
USE SalesPro;

SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto
ORDER BY product_id;
```

Resultados antes:

```text
PENDIENTE: pegar salida SQL.
```

## 2. Órdenes antes de la prueba

```sql
USE SalesPro;

SELECT TOP 10 numero_orden, fk_cliente, total_orden, impuesto, fecha_orden
FROM Pos_Orden
ORDER BY numero_orden DESC;
```

Resultados antes:

```text
PENDIENTE: pegar salida SQL.
```

## 3. Orden válida

Ejecutar el request `Orden: crear venta con transacción, inventario e IVA por parámetro`.

Respuesta HTTP:

```text
PENDIENTE: pegar código HTTP y body.
```

## 4. Comprobación posterior a orden válida

```sql
USE SalesPro;

SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto
WHERE product_id IN (1, 2)
ORDER BY product_id;

SELECT TOP 10 numero_orden, fk_cliente, total_orden, impuesto
FROM Pos_Orden
ORDER BY numero_orden DESC;

SELECT TOP 10 fk_pos_orden, fk_producto, cantidad, precio_unitario, precio_subtotal
FROM Pos_Orden_Detalle
ORDER BY fk_pos_orden DESC;
```

Resultados después:

```text
PENDIENTE: pegar salida SQL.
```

## 5. Orden inválida por inventario insuficiente

Ejecutar el request `Orden: error de inventario insuficiente; debe hacer rollback`.

Respuesta HTTP:

```text
PENDIENTE: pegar código HTTP y body.
```

## 6. Comprobación de rollback

```sql
USE SalesPro;

SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto
WHERE product_id = 1;

SELECT TOP 10 numero_orden, fk_cliente, total_orden, impuesto
FROM Pos_Orden
ORDER BY numero_orden DESC;

SELECT TOP 10 fk_pos_orden, fk_producto, cantidad, precio_unitario, precio_subtotal
FROM Pos_Orden_Detalle
ORDER BY fk_pos_orden DESC;
```

Resultado esperado: el intento inválido no debe crear una orden parcial ni modificar inventario.

Resultados reales:

```text
PENDIENTE: pegar salida SQL y explicar si coincide con el estado anterior.
```

## Conclusión

PENDIENTE: completar cuando existan evidencias reales.
