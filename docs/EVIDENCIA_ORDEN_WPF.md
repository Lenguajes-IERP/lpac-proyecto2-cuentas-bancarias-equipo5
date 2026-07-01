# Evidencia: flujo de nueva orden

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
