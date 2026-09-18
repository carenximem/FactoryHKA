-- ================================================================
--  SISTEMA DE VENTAS - THE FACTORY HKA
--  Script de creación de base de datos + datos de prueba
--  Base de datos: FactoryHKA_Ventas
-- ================================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FactoryHKA_Ventas')
BEGIN
    CREATE DATABASE FactoryHKA_Ventas;
END
GO

USE FactoryHKA_Ventas;
GO

-- ==================== TABLAS ====================

-- Clientes
IF OBJECT_ID('dbo.Clientes', 'U') IS NULL
CREATE TABLE Clientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Telefono NVARCHAR(20),
    Direccion NVARCHAR(200),
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Productos
IF OBJECT_ID('dbo.Productos', 'U') IS NULL
CREATE TABLE Productos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(500),
    Precio DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- Facturas
IF OBJECT_ID('dbo.Facturas', 'U') IS NULL
CREATE TABLE Facturas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NumeroFactura NVARCHAR(30) NOT NULL UNIQUE,
    ClienteId INT NOT NULL FOREIGN KEY REFERENCES Clientes(Id),
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Subtotal DECIMAL(18,2) NOT NULL,
    Impuesto DECIMAL(18,2) NOT NULL,
    Total DECIMAL(18,2) NOT NULL,
    Estado INT NOT NULL DEFAULT 1
);
GO

-- Detalle de facturas
IF OBJECT_ID('dbo.DetalleFacturas', 'U') IS NULL
CREATE TABLE DetalleFacturas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FacturaId INT NOT NULL FOREIGN KEY REFERENCES Facturas(Id) ON DELETE CASCADE,
    ProductoId INT NOT NULL FOREIGN KEY REFERENCES Productos(Id),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL
);
GO

-- ==================== ÍNDICES ====================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Clientes_Email')
    CREATE UNIQUE INDEX IX_Clientes_Email ON Clientes(Email);
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Facturas_Numero')
    CREATE UNIQUE INDEX IX_Facturas_Numero ON Facturas(NumeroFactura);
GO

-- ==================== DATOS DE PRUEBA ====================

-- 2 clientes iniciales
IF NOT EXISTS (SELECT * FROM Clientes)
BEGIN
    INSERT INTO Clientes (Nombre, Apellido, Email, Telefono, Direccion) VALUES
    ('Juan', 'Pérez', 'juan@test.com', '3001234567', 'Cra 10 #20-30, Bogotá'),
    ('María', 'Gómez', 'maria@test.com', '3009876543', 'Calle 45 #12-15, Medellín');
END
GO

-- 50 clientes adicionales
IF (SELECT COUNT(*) FROM Clientes) < 50
BEGIN
    INSERT INTO Clientes (Nombre, Apellido, Email, Telefono, Direccion, Activo) VALUES
    ('Carlos', 'Ramírez', 'carlos.ramirez@test.com', '3001110001', 'Cra 10 #20-30, Bogotá', 1),
    ('Ana', 'Martínez', 'ana.martinez@test.com', '3001110002', 'Calle 45 #12-15, Medellín', 1),
    ('Luis', 'González', 'luis.gonzalez@test.com', '3001110003', 'Av 68 #30-40, Cali', 1),
    ('María', 'López', 'maria.lopez@test.com', '3001110004', 'Cra 7 #45-60, Barranquilla', 1),
    ('Jorge', 'Hernández', 'jorge.hernandez@test.com', '3001110005', 'Calle 80 #15-22, Bogotá', 1),
    ('Laura', 'García', 'laura.garcia@test.com', '3001110006', 'Cra 50 #20-30, Medellín', 1),
    ('Andrés', 'Rodríguez', 'andres.rodriguez@test.com', '3001110007', 'Av 6N #25-10, Cali', 1),
    ('Diana', 'Pérez', 'diana.perez@test.com', '3001110008', 'Calle 100 #20-15, Bogotá', 1),
    ('Felipe', 'Sánchez', 'felipe.sanchez@test.com', '3001110009', 'Cra 43A #1-50, Medellín', 1),
    ('Camila', 'Torres', 'camila.torres@test.com', '3001110010', 'Av 3N #35-20, Cali', 1),
    ('Sebastián', 'Flores', 'sebastian.flores@test.com', '3001110011', 'Calle 26 #50-30, Bogotá', 1),
    ('Valentina', 'Rivera', 'valentina.rivera@test.com', '3001110012', 'Cra 70 #45-10, Medellín', 1),
    ('Diego', 'Castro', 'diego.castro@test.com', '3001110013', 'Calle 5 #10-15, Cali', 1),
    ('Isabella', 'Ortiz', 'isabella.ortiz@test.com', '3001110014', 'Av 15 #25-40, Barranquilla', 1),
    ('Daniel', 'Gómez', 'daniel.gomez@test.com', '3001110015', 'Cra 11 #78-30, Bogotá', 1),
    ('Sofía', 'Morales', 'sofia.morales@test.com', '3001110016', 'Calle 30 #45-12, Medellín', 1),
    ('Mateo', 'Vargas', 'mateo.vargas@test.com', '3001110017', 'Av 4N #20-18, Cali', 1),
    ('Mariana', 'Jiménez', 'mariana.jimenez@test.com', '3001110018', 'Cra 54 #72-10, Barranquilla', 1),
    ('Juan', 'Rojas', 'juan.rojas@test.com', '3001110019', 'Calle 53 #24-30, Bogotá', 1),
    ('Gabriela', 'Suárez', 'gabriela.suarez@test.com', '3001110020', 'Cra 35 #10-25, Medellín', 1),
    ('Alejandro', 'Mendoza', 'alejandro.mendoza@test.com', '3001110021', 'Av 6 #14-40, Cali', 1),
    ('Paula', 'Reyes', 'paula.reyes@test.com', '3001110022', 'Calle 72 #10-05, Barranquilla', 1),
    ('Ricardo', 'Cruz', 'ricardo.cruz@test.com', '3001110023', 'Cra 15 #85-20, Bogotá', 1),
    ('Natalia', 'Herrera', 'natalia.herrera@test.com', '3001110024', 'Calle 50 #40-15, Medellín', 1),
    ('Óscar', 'Medina', 'oscar.medina@test.com', '3001110025', 'Av 5N #23-30, Cali', 1),
    ('Juliana', 'Aguilar', 'juliana.aguilar@test.com', '3001110026', 'Cra 46 #80-12, Barranquilla', 1),
    ('Santiago', 'Peña', 'santiago.pena@test.com', '3001110027', 'Calle 19 #4-30, Bogotá', 1),
    ('Carolina', 'Navarro', 'carolina.navarro@test.com', '3001110028', 'Cra 80 #30-25, Medellín', 1),
    ('Julián', 'Molina', 'julian.molina@test.com', '3001110029', 'Av 2N #15-08, Cali', 1),
    ('Sara', 'Delgado', 'sara.delgado@test.com', '3001110030', 'Calle 84 #15-40, Barranquilla', 1),
    ('Miguel', 'Cabrera', 'miguel.cabrera@test.com', '3001110031', 'Cra 13 #26-15, Bogotá', 1),
    ('Daniela', 'Ruiz', 'daniela.ruiz@test.com', '3001110032', 'Calle 44 #52-10, Medellín', 1),
    ('Esteban', 'Silva', 'esteban.silva@test.com', '3001110033', 'Av 8N #18-25, Cali', 1),
    ('Manuela', 'Ríos', 'manuela.rios@test.com', '3001110034', 'Cra 60 #75-30, Barranquilla', 1),
    ('Nicolás', 'Arias', 'nicolas.arias@test.com', '3001110035', 'Calle 63 #22-10, Bogotá', 1),
    ('Alejandra', 'Cortés', 'alejandra.cortes@test.com', '3001110036', 'Cra 32 #40-18, Medellín', 1),
    ('Jhon', 'Quintero', 'jhon.quintero@test.com', '3001110037', 'Av 4N #12-05, Cali', 1),
    ('Lina', 'Mejía', 'lina.mejia@test.com', '3001110038', 'Calle 90 #18-40, Barranquilla', 1),
    ('Camilo', 'Restrepo', 'camilo.restrepo@test.com', '3001110039', 'Cra 7 #32-25, Bogotá', 1),
    ('Tatiana', 'Ospina', 'tatiana.ospina@test.com', '3001110040', 'Calle 55 #48-12, Medellín', 1),
    ('Fernando', 'Cardona', 'fernando.cardona@test.com', '3001110041', 'Av 5N #20-30, Cali', 1),
    ('Verónica', 'Salazar', 'veronica.salazar@test.com', '3001110042', 'Cra 53 #79-15, Barranquilla', 1),
    ('Héctor', 'Bermúdez', 'hector.bermudez@test.com', '3001110043', 'Calle 13 #30-40, Bogotá', 1),
    ('Melissa', 'Zapata', 'melissa.zapata@test.com', '3001110044', 'Cra 37 #50-20, Medellín', 1),
    ('Iván', 'Pineda', 'ivan.pineda@test.com', '3001110045', 'Av 3N #28-10, Cali', 1),
    ('Adriana', 'Cárdenas', 'adriana.cardenas@test.com', '3001110046', 'Calle 76 #25-15, Barranquilla', 1),
    ('Gustavo', 'Beltrán', 'gustavo.beltran@test.com', '3001110047', 'Cra 9 #70-30, Bogotá', 0),
    ('Mónica', 'Lozano', 'monica.lozano@test.com', '3001110048', 'Calle 48 #38-20, Medellín', 0),
    ('Raúl', 'Parra', 'raul.parra@test.com', '3001110049', 'Av 6N #32-15, Cali', 1),
    ('Catalina', 'Espinoza', 'catalina.espinoza@test.com', '3001110050', 'Cra 58 #68-40, Barranquilla', 1);
END
GO

-- Productos de prueba
IF (SELECT COUNT(*) FROM Productos) < 20
BEGIN
    INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Activo) VALUES
    ('Laptop HP 15"', 'Laptop 15 pulgadas Intel Core i5, 8GB RAM', 2500000, 10, 1),
    ('Mouse Logitech M280', 'Mouse inalámbrico ergonómico', 85000, 50, 1),
    ('Teclado Mecánico RGB', 'Teclado mecánico retroiluminado', 250000, 20, 1),
    ('Monitor Samsung 24"', 'Monitor LED Full HD 1080p', 850000, 15, 1),
    ('Teclado Logitech K380', 'Teclado inalámbrico bluetooth', 180000, 30, 1),
    ('Mousepad Gamer XL', 'Mousepad grande antideslizante', 65000, 40, 1),
    ('Webcam Logitech C920', 'Cámara HD 1080p con micrófono', 350000, 12, 1),
    ('Audífonos Sony WH-1000', 'Audífonos con cancelación de ruido', 1200000, 8, 1),
    ('Disco SSD 500GB', 'Unidad de estado sólido SATA III', 280000, 25, 1),
    ('Memoria RAM 8GB DDR4', 'Memoria RAM 2666MHz', 220000, 18, 1),
    ('Fuente 600W', 'Fuente de poder certificada 80+ Bronze', 320000, 10, 1),
    ('Gabinete ATX', 'Gabinete con ventilación lateral', 200000, 7, 1),
    ('Silla Gamer', 'Silla ergonómica reclinable', 950000, 5, 1),
    ('Escritorio Moderno', 'Escritorio de madera 1.20m', 680000, 4, 1),
    ('Impresora HP LaserJet', 'Impresora láser monocromática', 890000, 6, 1),
    ('Router TP-Link AX1500', 'Router WiFi 6 dual band', 420000, 14, 1),
    ('Tablet Samsung A8', 'Tablet 10.5 pulgadas 64GB', 1050000, 9, 1),
    ('Smartwatch Xiaomi', 'Reloj inteligente con GPS', 450000, 11, 1),
    ('Cargador USB-C 65W', 'Cargador rápido GaN 65W', 130000, 35, 1),
    ('Cable HDMI 2m', 'Cable HDMI 4K alta velocidad', 45000, 60, 1),
    ('Hub USB 4 puertos', 'Hub USB 3.0 con alimentación', 85000, 0, 1),
    ('Base refrigerante laptop', 'Base con 5 ventiladores RGB', 115000, 3, 0),
    ('Lámpara LED escritorio', 'Lámpara con brazo ajustable', 95000, 22, 1);
END
GO

PRINT '=========================================';
PRINT '  Base de datos FactoryHKA_Ventas lista.';
PRINT '=========================================';
GO

SELECT 
    (SELECT COUNT(*) FROM Clientes) AS TotalClientes,
    (SELECT COUNT(*) FROM Productos) AS TotalProductos,
    (SELECT COUNT(*) FROM Facturas) AS TotalFacturas;
GO