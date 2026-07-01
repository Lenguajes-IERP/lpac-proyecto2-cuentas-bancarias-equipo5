IF DB_ID('SalesPro') IS NULL
BEGIN
    CREATE DATABASE SalesPro;
END
GO

USE SalesPro;
GO

IF OBJECT_ID('ParametroSistema', 'U') IS NULL
BEGIN
    CREATE TABLE ParametroSistema
    (
        id INT IDENTITY(1,1) CONSTRAINT PK_ParametroSistema PRIMARY KEY,
        nombre NVARCHAR(80) NOT NULL CONSTRAINT UQ_ParametroSistema_nombre UNIQUE,
        valor_decimal DECIMAL(18,4) NULL,
        valor_texto NVARCHAR(250) NULL
    );
END
GO

IF OBJECT_ID('Banco', 'U') IS NULL
BEGIN
    CREATE TABLE Banco
    (
        id INT IDENTITY(1,1) CONSTRAINT PK_Banco PRIMARY KEY,
        codigo_identificador_banco NVARCHAR(30) NOT NULL CONSTRAINT UQ_Banco_codigo UNIQUE,
        nombre NVARCHAR(120) NOT NULL,
        email NVARCHAR(120) NULL,
        ciudad NVARCHAR(80) NULL,
        fax NVARCHAR(40) NULL,
        zip NVARCHAR(20) NULL,
        pais NVARCHAR(80) NULL,
        direccion NVARCHAR(250) NULL,
        telefono NVARCHAR(40) NULL,
        estado_pais NVARCHAR(80) NULL,
        active BIT NOT NULL CONSTRAINT DF_Banco_active DEFAULT (1)
    );
END
GO

IF OBJECT_ID('Compania', 'U') IS NULL
BEGIN
    CREATE TABLE Compania
    (
        id INT IDENTITY(1,1) CONSTRAINT PK_Compania PRIMARY KEY,
        nombre NVARCHAR(120) NOT NULL,
        logo VARBINARY(MAX) NULL,
        email NVARCHAR(120) NULL,
        telefono NVARCHAR(40) NULL,
        cedula_juridica NVARCHAR(40) NULL,
        pais NVARCHAR(80) NULL,
        direccion NVARCHAR(250) NULL
    );
END
GO

IF OBJECT_ID('Cliente', 'U') IS NULL
BEGIN
    CREATE TABLE Cliente
    (
        id INT IDENTITY(1,1) CONSTRAINT PK_Cliente PRIMARY KEY,
        apellidos NVARCHAR(100) NOT NULL,
        nombre NVARCHAR(100) NOT NULL,
        numero_identificacion NVARCHAR(40) NOT NULL CONSTRAINT UQ_Cliente_identificacion UNIQUE,
        login_name NVARCHAR(80) NULL,
        password NVARCHAR(250) NULL,
        password_encriptada NVARCHAR(250) NULL,
        direccion NVARCHAR(250) NULL,
        ciudad NVARCHAR(80) NULL,
        nacionalidad NVARCHAR(80) NULL,
        telefono_fijo NVARCHAR(40) NULL,
        telefono_mobil NVARCHAR(40) NULL,
        email NVARCHAR(120) NULL,
        photo VARBINARY(MAX) NULL,
        activo BIT NOT NULL CONSTRAINT DF_Cliente_activo DEFAULT (1)
    );
END
GO

IF OBJECT_ID('Empleado', 'U') IS NULL
BEGIN
    CREATE TABLE Empleado
    (
        id INT IDENTITY(1,1) CONSTRAINT PK_Empleado PRIMARY KEY,
        apellidos NVARCHAR(100) NOT NULL,
        nombre NVARCHAR(100) NOT NULL,
        numero_identificacion NVARCHAR(40) NOT NULL CONSTRAINT UQ_Empleado_identificacion UNIQUE,
        login_name NVARCHAR(80) NULL,
        password NVARCHAR(250) NULL,
        password_encriptada NVARCHAR(250) NULL,
        password_temporal BIT NULL,
        estado_civil NVARCHAR(40) NULL,
        direccion NVARCHAR(250) NULL,
        ciudad NVARCHAR(80) NULL,
        nacionalidad NVARCHAR(80) NULL,
        telefono_oficina NVARCHAR(40) NULL,
        telefono_mobil NVARCHAR(40) NULL,
        email NVARCHAR(120) NULL,
        fk_compania INT NULL,
        fk_departamento INT NULL,
        fk_puesto_trabajo INT NULL,
        photo VARBINARY(MAX) NULL,
        activo BIT NOT NULL CONSTRAINT DF_Empleado_activo DEFAULT (1),
        numero_cuenta_bancaria NVARCHAR(80) NULL,
        nombre_banco_cuenta NVARCHAR(120) NULL,
        CONSTRAINT FK_Empleado_Compania FOREIGN KEY (fk_compania) REFERENCES Compania(id)
    );
END
GO

IF OBJECT_ID('Producto', 'U') IS NULL
BEGIN
    CREATE TABLE Producto
    (
        product_id INT IDENTITY(1,1) CONSTRAINT PK_Producto PRIMARY KEY,
        nombre_etiqueta NVARCHAR(120) NOT NULL,
        description NVARCHAR(250) NULL,
        existencia_en_stock INT NOT NULL CONSTRAINT CK_Producto_stock CHECK (existencia_en_stock >= 0),
        existencia_limite_para_alerta INT NULL,
        precio_neto DECIMAL(18,2) NOT NULL CONSTRAINT CK_Producto_precio CHECK (precio_neto >= 0),
        precio_minimo DECIMAL(18,2) NULL,
        precio_minimo_con_impuesto DECIMAL(18,2) NULL,
        fecha_creacion DATETIME2 NOT NULL CONSTRAINT DF_Producto_fecha DEFAULT (SYSDATETIME()),
        puede_venderse BIT NOT NULL CONSTRAINT DF_Producto_vender DEFAULT (1),
        puede_comprarse BIT NOT NULL CONSTRAINT DF_Producto_comprar DEFAULT (1),
        impuesto_valor_agregado DECIMAL(18,2) NULL,
        impuesto_local1 DECIMAL(18,2) NULL,
        impuesto_local2 DECIMAL(18,2) NULL,
        tiene_impuesto BIT NOT NULL CONSTRAINT DF_Producto_tiene_impuesto DEFAULT (1),
        codigo_barra NVARCHAR(80) NULL,
        imagen VARBINARY(MAX) NULL,
        codigo_contable_ventas NVARCHAR(80) NULL,
        codigo_contable_compras NVARCHAR(80) NULL,
        peso DECIMAL(18,2) NULL,
        unidad_peso NVARCHAR(40) NULL,
        longitud DECIMAL(18,2) NULL,
        unidad_longitud NVARCHAR(40) NULL,
        superficie DECIMAL(18,2) NULL,
        unidad_superficie NVARCHAR(40) NULL,
        volumen DECIMAL(18,2) NULL
    );
END
GO

IF OBJECT_ID('Compania_Cuenta_Bancaria', 'U') IS NULL
BEGIN
    CREATE TABLE Compania_Cuenta_Bancaria
    (
        id INT IDENTITY(1,1) CONSTRAINT PK_CompaniaCuentaBancaria PRIMARY KEY,
        numero_cuenta NVARCHAR(80) NOT NULL,
        tipo_cuenta NVARCHAR(40) NOT NULL,
        tipo_divisa NVARCHAR(10) NOT NULL,
        estado BIT NOT NULL CONSTRAINT DF_CompaniaCuentaBancaria_estado DEFAULT (1),
        pais NVARCHAR(80) NOT NULL,
        provincia NVARCHAR(80) NOT NULL,
        fk_banco INT NOT NULL,
        fk_compania INT NOT NULL,
        nombre_dueno NVARCHAR(100) NOT NULL,
        apellidos_dueno NVARCHAR(100) NOT NULL,
        CONSTRAINT FK_CompaniaCuentaBancaria_Banco FOREIGN KEY (fk_banco) REFERENCES Banco(id),
        CONSTRAINT FK_CompaniaCuentaBancaria_Compania FOREIGN KEY (fk_compania) REFERENCES Compania(id),
        CONSTRAINT UQ_CompaniaCuentaBancaria_banco_numero UNIQUE (fk_banco, numero_cuenta),
        CONSTRAINT CK_CompaniaCuentaBancaria_tipo CHECK (tipo_cuenta IN ('Corriente', 'Ahorro', 'Planilla')),
        CONSTRAINT CK_CompaniaCuentaBancaria_divisa CHECK (tipo_divisa IN ('CRC', 'USD', 'EUR'))
    );
END
GO

IF OBJECT_ID('Pos_Orden', 'U') IS NULL
BEGIN
    CREATE TABLE Pos_Orden
    (
        numero_orden INT IDENTITY(1,1) CONSTRAINT PK_PosOrden PRIMARY KEY,
        fk_cliente INT NOT NULL,
        fecha_orden DATETIME2 NOT NULL CONSTRAINT DF_PosOrden_fecha DEFAULT (SYSDATETIME()),
        fk_empleado INT NULL,
        total_orden DECIMAL(18,2) NOT NULL,
        impuesto DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_PosOrden_Cliente FOREIGN KEY (fk_cliente) REFERENCES Cliente(id),
        CONSTRAINT FK_PosOrden_Empleado FOREIGN KEY (fk_empleado) REFERENCES Empleado(id)
    );
END
GO

IF OBJECT_ID('Pos_Orden_Detalle', 'U') IS NULL
BEGIN
    CREATE TABLE Pos_Orden_Detalle
    (
        fk_pos_orden INT NOT NULL,
        fk_producto INT NOT NULL,
        impuesto DECIMAL(18,2) NOT NULL,
        cantidad INT NOT NULL CONSTRAINT CK_PosOrdenDetalle_cantidad CHECK (cantidad > 0),
        descuento DECIMAL(18,2) NOT NULL CONSTRAINT DF_PosOrdenDetalle_descuento DEFAULT (0),
        precio_unitario DECIMAL(18,2) NOT NULL,
        precio_subtotal DECIMAL(18,2) NOT NULL,
        CONSTRAINT PK_PosOrdenDetalle PRIMARY KEY (fk_pos_orden, fk_producto),
        CONSTRAINT FK_PosOrdenDetalle_Orden FOREIGN KEY (fk_pos_orden) REFERENCES Pos_Orden(numero_orden),
        CONSTRAINT FK_PosOrdenDetalle_Producto FOREIGN KEY (fk_producto) REFERENCES Producto(product_id)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM ParametroSistema WHERE nombre = 'IVA')
BEGIN
    INSERT INTO ParametroSistema (nombre, valor_decimal)
    VALUES ('IVA', 13.0000);
END
GO

IF NOT EXISTS (SELECT 1 FROM Banco WHERE codigo_identificador_banco = 'BAC')
BEGIN
    INSERT INTO Banco (codigo_identificador_banco, nombre, pais, telefono, active)
    VALUES
        ('BAC', 'BAC Credomatic', 'Costa Rica', '2295-9000', 1),
        ('BCR', 'Banco de Costa Rica', 'Costa Rica', '2211-1111', 1),
        ('BNCR', 'Banco Nacional de Costa Rica', 'Costa Rica', '2212-2000', 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM Compania WHERE nombre = 'SalesPro Demo S.A.')
BEGIN
    INSERT INTO Compania (nombre, email, telefono, cedula_juridica, pais, direccion)
    VALUES ('SalesPro Demo S.A.', 'info@salespro.demo', '2222-0000', '3-101-000000', 'Costa Rica', 'San José');
END
GO

IF NOT EXISTS (SELECT 1 FROM Cliente WHERE numero_identificacion = '1-1111-1111')
BEGIN
    INSERT INTO Cliente (apellidos, nombre, numero_identificacion, ciudad, nacionalidad, telefono_mobil, email, activo)
    VALUES
        ('Mora', 'Valeria', '1-1111-1111', 'San José', 'Costarricense', '8888-1111', 'valeria@example.com', 1),
        ('Solano', 'Andrés', '1-2222-2222', 'Heredia', 'Costarricense', '8888-2222', 'andres@example.com', 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM Empleado WHERE numero_identificacion = '1-3333-3333')
BEGIN
    INSERT INTO Empleado (apellidos, nombre, numero_identificacion, email, fk_compania, activo)
    VALUES ('Cordero', 'Sebastián', '1-3333-3333', 'sebas@salespro.demo', 1, 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM Producto WHERE codigo_barra = 'PROD001')
BEGIN
    INSERT INTO Producto (codigo_barra, nombre_etiqueta, description, existencia_en_stock, precio_neto, puede_venderse, puede_comprarse, tiene_impuesto)
    VALUES
        ('PROD001', 'Laptop', 'Laptop para oficina', 10, 600000.00, 1, 1, 1),
        ('PROD002', 'Mouse', 'Mouse inalámbrico', 50, 12500.00, 1, 1, 1),
        ('PROD003', 'Keyboard', 'Teclado mecánico', 30, 37500.00, 1, 1, 1),
        ('PROD004', 'Servicio instalación', 'Servicio no gravado de ejemplo', 99, 25000.00, 1, 0, 0);
END
GO

IF NOT EXISTS (SELECT 1 FROM Compania_Cuenta_Bancaria WHERE numero_cuenta = 'CR000000000001')
BEGIN
    INSERT INTO Compania_Cuenta_Bancaria
        (numero_cuenta, tipo_cuenta, tipo_divisa, estado, pais, provincia, fk_banco, fk_compania, nombre_dueno, apellidos_dueno)
    VALUES
        ('CR000000000001', 'Corriente', 'CRC', 1, 'Costa Rica', 'San José', 1, 1, 'SalesPro', 'Demo');
END
GO
