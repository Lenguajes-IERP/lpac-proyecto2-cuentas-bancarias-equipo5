# Módulo WPF / Frontend

Responsable principal:

```text
Alejandro Porras (@axpew)
```

Apoyo:

```text
Caleb Hernández Vega (@CalebHv21)
Sebastián Cordero (@cbastiancq-lab)
```

## Qué se puede tocar aquí

- Vistas XAML.
- ViewModels.
- Servicios `HttpClient`.
- Validaciones visuales simples.
- Mensajes al usuario.
- Flujo de pantallas.

## Qué NO se debe hacer aquí

- No escribir SQL.
- No tocar inventario directamente.
- No guardar órdenes sin pasar por el API.
- No calcular el total oficial de la orden como verdad final.

## Regla de defensa

WPF puede mostrar estimaciones, pero el backend calcula el total oficial.

La orden debe verse como maestro-detalle:

- Cliente y fecha arriba.
- Productos en tabla.
- Subtotal, impuesto y total visibles.
- Botones para agregar/remover/incrementar/decrementar.
