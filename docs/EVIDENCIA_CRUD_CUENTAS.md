# Evidencia: CRUD de cuentas bancarias

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

## Conclusión

El CRUD fue validado a nivel de API contra SQL Server real. Falta anexar evidencia visual de WPF para completar el respaldo de la interfaz.
