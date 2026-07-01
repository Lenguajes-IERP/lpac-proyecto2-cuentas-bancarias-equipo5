# Módulo Data / ADO.NET / Transacciones

Responsable principal:

```text
YO (@cbastiancq-lab)
```

Apoyo:

```text
Caleb Hernández Vega (@CalebHv21)
```

## Qué se puede tocar aquí

- Repositorios.
- Consultas SQL.
- `SqlConnection`.
- `SqlCommand`.
- `SqlTransaction`.
- Manejo de inventario dentro de la transacción.

## Qué NO se debe hacer aquí

- No poner lógica visual.
- No mostrar `MessageBox`.
- No depender de WPF.
- No aceptar totales calculados por el frontend como verdad.

## Regla crítica

La orden de venta debe ser atómica:

```text
Si falla cliente/producto/stock/detalle, se hace rollback.
Si todo sale bien, se hace commit.
```

No mover el descuento de inventario fuera de la transacción.
