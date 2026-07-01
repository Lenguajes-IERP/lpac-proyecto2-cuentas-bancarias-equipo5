# Evidencia: flujo de nueva orden

Ejecutado: 2026-07-01. API en `http://localhost:5294`.

## Datos de prueba usados

- Cliente: `Valeria Mora` (id 1)
- Empleado: id 1
- Productos agregados:
  - Laptop (id 1), cantidad 1, precio ₡600 000
  - Mouse (id 2), cantidad 2, precio ₡12 500 c/u
- IVA configurado: 13% (tabla `ParametroSistema`)

## Verificación de IVA en base de datos

```sql
USE SalesPro;
SELECT nombre, valor_decimal FROM ParametroSistema WHERE nombre = 'IVA';
```

```
nombre  valor_decimal
------- -------------
IVA            13.0000
```

## 1. Catálogos disponibles al abrir la orden

```http
GET http://localhost:5294/api/catalogos/clientes?buscar=Valeria
```

```json
[{ "id": 1, "nombre": "Valeria", "apellidos": "Mora" }]
```

```http
GET http://localhost:5294/api/catalogos/productos?buscar=lap
```

```json
[{ "id": 1, "nombreEtiqueta": "Laptop", "existenciaEnStock": 10, "precioNeto": 600000.00 }]
```

## 2. Inventario antes de procesar la orden

```sql
SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto WHERE product_id IN (1, 2) ORDER BY product_id;
```

```
product_id  nombre_etiqueta  existencia_en_stock
----------- ---------------- -------------------
          1 Laptop                            10
          2 Mouse                             50
```

## 3. Procesar orden (request HTTP)

```http
POST http://localhost:5294/api/ordenes
Content-Type: application/json

Fecha de ejecución API: 2026-07-01  
Ambiente: API local contra SQL Server del curso  
Archivo generado: `docs/evidencia_generada/api_transacciones_20260701164829.json`

## Validación por API

Se ejecutó una orden válida contra el endpoint:

```http
POST /api/ordenes
```

Body:

```json
{
  "clienteId": 1,
  "empleadoId": 1,
  "detalles": [
    { "productoId": 1, "cantidad": 1 },
    { "productoId": 2, "cantidad": 2 }
  ]
}
```

Respuesta `201 Created`:

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

**Cálculo verificado:**
- Laptop: ₡600 000 × 1 = ₡600 000 + IVA ₡78 000
- Mouse: ₡12 500 × 2 = ₡25 000 + IVA ₡3 250
- Subtotal: ₡625 000 | Impuesto: ₡81 250 | **Total: ₡706 250**

## 4. Inventario después de procesar la orden

```sql
SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto WHERE product_id IN (1, 2) ORDER BY product_id;
```

```
product_id  nombre_etiqueta  existencia_en_stock
----------- ---------------- -------------------
          1 Laptop                             9
          2 Mouse                             48
```

Laptop bajó de 10 a 9 (-1). Mouse bajó de 50 a 48 (-2). Inventario actualizado correctamente.

## 5. Consultar la orden creada

```http
GET http://localhost:5294/api/ordenes/1
```

Respuesta `200 OK` con los mismos datos de la orden creada (número, cliente, detalles, totales).

## 6. Errores de validación de orden

### Sin detalles → 400

```json
{ "status": 400, "code": "validation_error", "message": "La orden debe incluir al menos un producto." }
```

### Cliente inexistente → 404

```json
{ "status": 404, "code": "not_found", "message": "El cliente 999999 no existe o no está activo." }
```

### Stock insuficiente → 409 con rollback

```json
{ "status": 409, "code": "conflict", "message": "Stock insuficiente para 'Laptop'. Disponible: 9, solicitado: 999999." }
```

Después del intento fallido: Laptop sigue en 9, total de órdenes sigue siendo 1. No hubo inserción parcial.

## Observaciones

- Los totales calculados por la API coinciden con la fórmula: `subtotal × (1 + IVA/100)`.
- El IVA se lee dinámicamente de `ParametroSistema` — si cambia el parámetro, cambia el cálculo sin modificar código.
- El rollback ante stock insuficiente es verificable en base de datos inmediatamente después del error.

## Conclusión

El flujo completo de la orden (selección de cliente, búsqueda de productos, cálculo de totales, procesamiento e inventario) funciona correctamente. La transacción garantiza que no existen estados parciales ante errores.
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

## Validación de rollback

Se ejecutó una orden inválida por inventario insuficiente.

Resultado:

```text
Código HTTP recibido: 409
Stock antes: 9
Stock después: 9
Rollback mantiene stock: true
```

## Validación WPF pendiente de captura

La implementación WPF contiene:

- ventana de búsqueda de cliente;
- ventana de búsqueda de producto;
- matriz maestro-detalle;
- incremento y decremento de cantidades;
- remoción de productos;
- subtotal, IVA estimado y total estimado;
- botón para procesar orden contra API.

Pendiente de anexar capturas manuales:

1. Vista de nueva orden abierta.
2. Cliente seleccionado.
3. Producto buscado y agregado.
4. Matriz con productos.
5. Incremento/decremento de cantidades.
6. Orden procesada.
7. Consulta SQL o respuesta API posterior.

## Conclusión

El flujo principal de orden fue validado contra backend y base real. Falta anexar capturas de la interfaz WPF para completar la evidencia visual de la funcionalidad.
