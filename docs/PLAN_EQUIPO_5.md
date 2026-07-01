# Plan de trabajo - Proyecto 2 Equipo 5


## Integrantes

| Integrante | Usuario GitHub | Rol sugerido |
|---|---|---|
| Caleb Hernández Vega | `CalebHv21` | Documentación, pruebas `.http`, apoyo WPF |
| Sebastian Cordero | `cbastiancq-lab` | Base de datos, transacciones, integración |
| Josue Delgado Corrales | `JosueDelgadoCorrales` | API REST y capa de negocio |
| Alejandro Porras | `axpew` | WPF/MVVM e interfaz |

## Requisitos principales del enunciado

- Equipo 5: CRUD de cuentas bancarias.
- WPF como frontend.
- ASP.NET Core Web API como backend.
- SQL Server.
- Arquitectura de tres capas: controlador REST, negocio y servicios de datos.
- Accesibilidad digital en WPF mediante nombres accesibles, ayudas de controles y navegación por teclado.
- WPF debe tener únicamente dos funcionalidades:
  1. Gestión de cuentas bancarias.
  2. Registro de orden de venta.
- La orden debe usar patrón maestro-detalle.
- La orden debe permitir:
  - seleccionar cliente;
  - buscar producto;
  - indicar cantidad;
  - mostrar productos en matriz;
  - remover productos;
  - incrementar/decrementar cantidades;
  - mostrar subtotal, impuesto y total;
  - actualizar inventario al procesar.
- La capa de datos debe manejar transacciones cuando corresponda.

## División recomendada

### Sebastián - Base de datos y transacción

Archivos principales:

- `Proyecto_backend/database/00_create_salespro.sql`
- `Proyecto_backend/SalesPro.Data/Repositories/OrdenRepository.cs`
- `Proyecto_backend/SalesPro.Data/Repositories/CuentaBancariaRepository.cs`

Tareas:

- Mantener tablas y datos semilla.
- Validar constraints y llaves foráneas.
- Proteger la transacción de orden.
- Verificar rollback cuando hay stock insuficiente.
- Apoyar integración final.

### Josue - API y negocio

Archivos principales:

- `Proyecto_backend/SalesPro.Api/Controllers/`
- `Proyecto_backend/SalesPro.Business/Services/`
- `Proyecto_compartido/SalesPro.Contracts/`

Tareas:

- Revisar endpoints REST.
- Validar respuestas 400/404/409.
- Mantener DTOs limpios.
- Actualizar `.http` cuando cambie la API.

### Alejandro - WPF

Archivos principales:

- `Proyecto_WPF/SalesPro.Wpf/Views/`
- `Proyecto_WPF/SalesPro.Wpf/ViewModels/`
- `Proyecto_WPF/SalesPro.Wpf/Services/`

Tareas:

- Pulir pantalla de cuentas bancarias.
- Pulir pantalla de nueva orden.
- Agregar búsqueda/selección de cliente.
- Mejorar mensajes de error.
- Validar UI contra la rúbrica.

### Caleb - Documentación y pruebas

Archivos principales:

- `docs/`
- `README.md`
- `Proyecto_backend/SalesPro.Api/SalesPro.Api.http`

Tareas:

- Documentar API REST.
- Documentar modelo de dominio.
- Documentar modelo entidad-relación.
- Crear bitácora.
- Completar pruebas `.http`.

## Pendientes críticos antes de entrega

- [x] Montar base SQL Server local o confirmar base remota final.
- [x] Agregar búsqueda/selección de cliente en WPF mediante ventana o diálogo.
- [x] Revisar que el `.http` cubra todos los recursos expuestos.
- [x] Corregir la transacción para evitar rollback después de commit.
- [x] Probar CRUD completo de cuentas bancarias.
- [x] Probar orden completa y descuento de inventario.
- [x] Probar rollback por stock insuficiente.
- [x] Agregar atributos básicos de accesibilidad digital en WPF.
- [x] Crear documentación final.
