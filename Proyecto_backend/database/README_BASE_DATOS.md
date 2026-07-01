# Base de datos - SalesPro

Motor requerido por el enunciado:

```text
SQL Server
```

## Script principal

Ejecutar en SQL Server Management Studio:

```text
database/00_create_salespro.sql
```

Este script crea:

- Base `SalesPro`.
- Tabla `ParametroSistema`.
- Tabla `Banco`.
- Tabla `Compania`.
- Tabla `Cliente`.
- Tabla `Empleado`.
- Tabla `Producto`.
- Tabla `Compania_Cuenta_Bancaria`.
- Tabla `Pos_Orden`.
- Tabla `Pos_Orden_Detalle`.
- Datos semilla para demo.

## Datos importantes

Parámetro de IVA:

```text
ParametroSistema.nombre = IVA
ParametroSistema.valor_decimal = 13
```

Productos semilla:

- Laptop.
- Mouse.
- Keyboard.
- Servicio instalación, sin impuesto.

## Connection string local recomendada

Para desarrollo local con LocalDB:

```json
"SalesProDb": "Server=(localdb)\\MSSQLLocalDB;Database=SalesPro;Trusted_Connection=True;TrustServerCertificate=True;"
```

## Connection string remota

Si se usa la base remota del curso, mantener las credenciales solo en `appsettings.Development.json` local o en variables de entorno.

No subir credenciales reales a repos públicos.

## Prueba rápida

Después de ejecutar el script:

```sql
USE SalesPro;

SELECT * FROM Banco;
SELECT * FROM Compania;
SELECT * FROM Cliente;
SELECT * FROM Producto;
SELECT * FROM Compania_Cuenta_Bancaria;
SELECT * FROM ParametroSistema;
```
