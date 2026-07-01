# Evidencia: pruebas de transacciones y rollback

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

Consulta previa contra SQL Server:

```text
Bancos: 3
Compañías: 1
Clientes: 2
Productos: 4
Cuentas bancarias: 1
IVA: 13.0000
```

## Orden válida

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

Conclusión: la orden válida se creó y el inventario fue actualizado.

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
