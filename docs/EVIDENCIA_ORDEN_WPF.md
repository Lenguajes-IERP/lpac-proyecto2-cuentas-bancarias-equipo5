# Plantilla de evidencia: flujo de nueva orden en WPF

Objetivo: validar el flujo completo de creación de una orden desde WPF hasta la base de datos.

Estado actual: pendiente de completar con capturas, respuestas HTTP y salidas SQL reales.

## Preparación

- Levantar la API:

```powershell
dotnet run --project .\Proyecto_backend\SalesPro.Api\SalesPro.Api.csproj
```

- Ejecutar `SalesPro.Wpf`.
- Verificar que existan clientes y productos con stock.

## Checklist de pruebas

### 1. Abrir vista de nueva orden

- Acción: navegar a la vista de nueva orden.
- Evidencia:

```text
PENDIENTE.
```

### 2. Seleccionar cliente

- Acción: buscar un cliente, por ejemplo `Valeria`, y seleccionarlo.
- Evidencia:

```text
PENDIENTE.
```

### 3. Buscar producto

- Acción: buscar un producto, por ejemplo `Laptop`.
- Evidencia:

```text
PENDIENTE.
```

### 4. Agregar productos

- Acción: agregar un producto con cantidad 1 y otro con cantidad 2.
- Resultado esperado: la matriz muestra código, nombre, precio, cantidad y subtotal.
- Evidencia:

```text
PENDIENTE.
```

### 5. Incrementar y decrementar cantidades

- Acción: usar los controles de cantidad.
- Resultado esperado: subtotales, impuesto y total se recalculan.
- Evidencia:

```text
PENDIENTE.
```

### 6. Remover producto

- Acción: eliminar una línea de la orden.
- Resultado esperado: la matriz y los totales se actualizan.
- Evidencia:

```text
PENDIENTE.
```

### 7. Confirmar IVA

```sql
USE SalesPro;

SELECT nombre, valor_decimal
FROM ParametroSistema
WHERE nombre = 'IVA';
```

Evidencia:

```text
PENDIENTE.
```

### 8. Procesar orden

- Acción: confirmar la orden.
- Resultado esperado: se muestra el número de orden creado.
- Evidencia:

```text
PENDIENTE.
```

### 9. Comprobar inventario

```sql
USE SalesPro;

SELECT product_id, nombre_etiqueta, existencia_en_stock
FROM Producto
WHERE product_id IN (1, 2)
ORDER BY product_id;
```

Resultado esperado: las existencias bajan según las cantidades vendidas.

Evidencia:

```text
PENDIENTE.
```

## Observaciones

```text
PENDIENTE: anotar errores encontrados o confirmar que no hubo.
```

## Conclusión

PENDIENTE: completar después de las pruebas.
