# Pruebas manuales `.http`

Responsable principal:

```text
Caleb Hernández Vega (@CalebHv21)
```

Apoyo:

```text
Josue Delgado Corrales (@JosueDelgadoCorrales)
Sebastián Cordero (@cbastiancq-lab)
```

Archivo principal:

```text
SalesPro.Api.http
```

## Qué debe probar

- Health check.
- Bancos.
- Compañías.
- Clientes.
- Productos.
- CRUD de cuentas bancarias.
- Crear orden.
- Consultar orden.
- Error por stock insuficiente.
- Error por datos inválidos.

## Cuidado

Algunas pruebas crean datos o descuentan inventario. No correrlas a lo loco contra una base compartida.
