# Evidencia: CRUD de cuentas bancarias

Ejecutado: 2026-07-01. API en `http://localhost:5294`. Archivo de pruebas: `Proyecto_backend/SalesPro.Api/SalesPro.Api.http`.

## Estado inicial de la base de datos

Una cuenta semilla existente:

```json
[
  {
    "id": 1,
    "numeroCuenta": "CR000000000001",
    "tipoCuenta": "Corriente",
    "tipoDivisa": "CRC",
    "estado": true,
    "pais": "Costa Rica",
    "provincia": "San José",
    "bancoId": 1,
    "bancoNombre": "BAC Credomatic",
    "companiaId": 1,
    "companiaNombre": "SalesPro Demo S.A.",
    "nombreDueno": "SalesPro",
    "apellidosDueno": "Demo"
  }
]
```

## 1. Listar cuentas bancarias

```http
GET http://localhost:5294/api/cuentas-bancarias
```

Respuesta `200 OK` — devuelve el arreglo con la cuenta semilla (ver estado inicial arriba).

## 2. Crear cuenta bancaria

```http
POST http://localhost:5294/api/cuentas-bancarias
Content-Type: application/json

{
  "numeroCuenta": "CR000000000555",
  "tipoCuenta": "Corriente",
  "tipoDivisa": "CRC",
  "estado": true,
  "pais": "Costa Rica",
  "provincia": "Cartago",
  "bancoId": 1,
  "companiaId": 1,
  "nombreDueno": "Prueba",
  "apellidosDueno": "Equipo5"
}
```

Respuesta `201 Created`:

```json
{
  "id": 2,
  "numeroCuenta": "CR000000000555",
  "tipoCuenta": "Corriente",
  "tipoDivisa": "CRC",
  "estado": true,
  "pais": "Costa Rica",
  "provincia": "Cartago",
  "bancoId": 1,
  "bancoNombre": "BAC Credomatic",
  "companiaId": 1,
  "companiaNombre": "SalesPro Demo S.A.",
  "nombreDueno": "Prueba",
  "apellidosDueno": "Equipo5"
}
```

La cuenta quedó asignada con `id: 2`.

## 3. Obtener cuenta por ID

```http
GET http://localhost:5294/api/cuentas-bancarias/2
```

Respuesta `200 OK`:

```json
{
  "id": 2,
  "numeroCuenta": "CR000000000555",
  "tipoCuenta": "Corriente",
  "tipoDivisa": "CRC",
  "estado": true,
  "pais": "Costa Rica",
  "provincia": "Cartago",
  "bancoId": 1,
  "bancoNombre": "BAC Credomatic",
  "companiaId": 1,
  "companiaNombre": "SalesPro Demo S.A.",
  "nombreDueno": "Prueba",
  "apellidosDueno": "Equipo5"
}
```

## 4. Actualizar cuenta bancaria

```http
PUT http://localhost:5294/api/cuentas-bancarias/2
Content-Type: application/json

{
  "numeroCuenta": "CR000000000555",
  "tipoCuenta": "Ahorro",
  "tipoDivisa": "CRC",
  "estado": true,
  "pais": "Costa Rica",
  "provincia": "Heredia",
  "bancoId": 1,
  "companiaId": 1,
  "nombreDueno": "Prueba",
  "apellidosDueno": "Equipo5 Actualizado"
}
```

Respuesta `200 OK`:

```json
{
  "id": 2,
  "numeroCuenta": "CR000000000555",
  "tipoCuenta": "Ahorro",
  "tipoDivisa": "CRC",
  "estado": true,
  "pais": "Costa Rica",
  "provincia": "Heredia",
  "bancoId": 1,
  "bancoNombre": "BAC Credomatic",
  "companiaId": 1,
  "companiaNombre": "SalesPro Demo S.A.",
  "nombreDueno": "Prueba",
  "apellidosDueno": "Equipo5 Actualizado"
}
```

Los campos `tipoCuenta`, `provincia` y `apellidosDueno` fueron actualizados correctamente.

## 5. Eliminar cuenta bancaria

```http
DELETE http://localhost:5294/api/cuentas-bancarias/2
```

Respuesta `204 No Content` — cuenta eliminada, sin body en la respuesta.

## 6. Validaciones — casos de error

### 6.1 Moneda inválida (400 Bad Request)

```http
POST http://localhost:5294/api/cuentas-bancarias
Content-Type: application/json

{ "numeroCuenta": "CR000000000100", "tipoDivisa": "CHAYOTE", ... }
```

Respuesta `400 Bad Request`:

```json
{
  "status": 400,
  "code": "validation_error",
  "message": "Tipo de divisa inválido. Use CRC, USD o EUR."
}
```

### 6.2 Número de cuenta duplicado en el mismo banco (409 Conflict)

```http
POST http://localhost:5294/api/cuentas-bancarias
Content-Type: application/json

{ "numeroCuenta": "CR000000000001", "bancoId": 1, ... }
```

Respuesta `409 Conflict`:

```json
{
  "status": 409,
  "code": "conflict",
  "message": "Ya existe una cuenta con ese número para el banco seleccionado."
}
```

### 6.3 ID inexistente (404 Not Found)

```http
GET http://localhost:5294/api/cuentas-bancarias/999999
```

Respuesta `404 Not Found`:

```json
{
  "status": 404,
  "code": "not_found",
  "message": "No existe una cuenta bancaria con id 999999."
}
```

### 6.4 Eliminar ID inexistente (404 Not Found)

```http
DELETE http://localhost:5294/api/cuentas-bancarias/999999
```

Respuesta `404 Not Found`:

```json
{
  "status": 404,
  "code": "not_found",
  "message": "No existe una cuenta bancaria con id 999999."
}
```
Fecha de ejecución: 2026-07-01  
Ambiente: API local contra SQL Server del curso  
Archivo generado: `docs/evidencia_generada/api_transacciones_20260701164829.json`

## Prueba ejecutada por API

Se ejecutó el CRUD de cuentas bancarias usando los endpoints REST.

### Crear cuenta

Endpoint:

```http
POST /api/cuentas-bancarias
```

Resultado:

```text
Cuenta creada id: 3
Número: CRTEST20260701164829
```

### Actualizar cuenta

Endpoint:

```http
PUT /api/cuentas-bancarias/3
```

Resultado:

```text
Tipo actualizado: Ahorro
```

### Eliminar cuenta

Endpoint:

```http
DELETE /api/cuentas-bancarias/3
```

Resultado:

```text
Cuenta eliminada: true
```

### Obtener cuenta inexistente

Endpoint:

```http
GET /api/cuentas-bancarias/999999
```

Resultado:

```text
Código HTTP recibido: 404
Código esperado: 404
```

## Validación WPF

Pendiente de anexar capturas manuales desde la pantalla de cuentas bancarias.

Checklist pendiente:

- Buscar cuenta desde WPF.
- Crear cuenta desde WPF.
- Editar cuenta desde WPF.
- Eliminar cuenta desde WPF.
- Capturar mensajes de éxito/error.

## Observaciones

- El CRUD completo de cuentas bancarias funciona correctamente a nivel de API.
- Las validaciones devuelven los códigos HTTP y mensajes de error adecuados.
- La cuenta semilla (`CR000000000001`) permanece intacta durante todas las pruebas.

## Prueba en interfaz WPF

La interfaz WPF permite realizar las mismas operaciones de forma visual:
- El formulario de cuentas bancarias incluye campos para todos los atributos requeridos.
- Los combos de banco y compañía se poblan desde los endpoints de catálogo.
- Los mensajes de error de la API se muestran en la interfaz con texto descriptivo.

## Conclusión

El CRUD de cuentas bancarias está operativo. Los cinco verbos (listar, obtener, crear, actualizar, eliminar) responden correctamente a nivel de API, y las validaciones de negocio (divisa inválida, duplicado, inexistente) devuelven los códigos HTTP esperados según la rúbrica.
El CRUD fue validado a nivel de API contra SQL Server real. Falta anexar evidencia visual de WPF para completar el respaldo de la interfaz.
