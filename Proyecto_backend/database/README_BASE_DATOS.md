# Base de datos - SalesPro

Motor requerido:

```text
SQL Server
```

## Script principal

Ejecutar en SQL Server Management Studio:

```text
Proyecto_backend/database/00_create_salespro.sql
```

El script crea la base `SalesPro`, las tablas principales y datos semilla para pruebas.

## Tablas principales

- `ParametroSistema`
- `Banco`
- `Compania`
- `Cliente`
- `Empleado`
- `Producto`
- `Compania_Cuenta_Bancaria`
- `Pos_Orden`
- `Pos_Orden_Detalle`

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

## Configuración de conexión

Para no versionar credenciales reales, usar:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.json
```

Basarse en:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.example.json
```

## Preparación por script

```powershell
powershell -ExecutionPolicy Bypass -File .\Proyecto_backend\scripts\setup-sqlserver.ps1 -Server "SERVIDOR_SQL" -User "USUARIO"
```

El script pide la clave en pantalla.

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
