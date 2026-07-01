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
- SQL Server como motor de base de datos.
- Arquitectura por capas: controlador REST, negocio y servicios de datos.
- Manejo transaccional en capa de datos cuando corresponda.
- WPF con dos funcionalidades: gestión de cuentas bancarias y registro de orden.
- Orden con patrón maestro-detalle.
- Pruebas de API en archivo `.http`.
- Documentación con API, ER, dominio, conclusiones y bitácora.

## División técnica

### Base de datos, Data y transacciones

Archivos principales:

- `Proyecto_backend/database/00_create_salespro.sql`
- `Proyecto_backend/SalesPro.Data/Repositories/OrdenRepository.cs`
- `Proyecto_backend/SalesPro.Data/Repositories/CuentaBancariaRepository.cs`

Responsabilidades:

- Mantener tablas y datos semilla.
- Validar llaves foráneas, checks y datos base.
- Proteger la transacción de orden.
- Verificar rollback cuando hay stock insuficiente.
- Mantener el cálculo de IVA desde `ParametroSistema`.

### API y negocio

Archivos principales:

- `Proyecto_backend/SalesPro.Api/Controllers/`
- `Proyecto_backend/SalesPro.Business/Services/`
- `Proyecto_backend/SalesPro.Contracts/`

Responsabilidades:

- Mantener endpoints REST.
- Validar respuestas 400/404/409.
- Mantener DTOs limpios.
- Actualizar `.http` cuando cambie la API.

### WPF

Archivos principales:

- `Proyecto_WPF/SalesPro.Wpf/Views/`
- `Proyecto_WPF/SalesPro.Wpf/ViewModels/`
- `Proyecto_WPF/SalesPro.Wpf/Services/`

Responsabilidades:

- Pantalla CRUD de cuentas bancarias.
- Pantalla de nueva orden.
- Búsqueda y selección de cliente.
- Búsqueda y selección de productos.
- Consumo real de API.
- Accesibilidad básica en controles principales.

### Documentación y pruebas

Archivos principales:

- `docs/`
- `README.md`
- `Proyecto_backend/SalesPro.Api/SalesPro.Api.http`

Responsabilidades:

- Documentar API REST.
- Documentar modelo de dominio.
- Documentar modelo entidad-relación.
- Completar bitácora.
- Completar evidencias de pruebas.
- Preparar informe final en PDF.

## Estado de cierre

- [x] Estructura física backend/WPF.
- [x] CRUD de cuentas bancarias.
- [x] Registro de orden maestro-detalle.
- [x] Búsqueda/selección de cliente en WPF.
- [x] Búsqueda/selección de productos en WPF.
- [x] Transacción para procesar orden.
- [x] Descuento de inventario dentro de transacción.
- [x] IVA desde parámetro de base de datos.
- [x] Archivo `.http` ordenado.
- [x] Script para generar ZIPs de entrega.
- [x] Compilación de solución completa.
- [x] Compilación de ZIPs por separado.
- [x] Ejecutar pruebas API contra SQL Server del curso.
- [x] Pegar evidencia real de transacciones.
- [x] Pegar evidencia real del CRUD a nivel API.
- [x] Pegar evidencia real de orden a nivel API/transacción.
- [ ] Anexar capturas WPF de CRUD.
- [ ] Anexar capturas WPF de orden.
- [ ] Exportar informe final a PDF.
