# Informe Final del Proyecto 2 - Equipo 5 (APA 7) - Borrador

Autor: Equipo 5
Fecha: (completar)

Resumen
-------
Breve resumen del proyecto, objetivos y alcance. (Completar con resultados verificados)

Tabla de contenidos
-------------------
- Portada
- Resumen
- Introducción y objetivo
- Especificación de la necesidad
- Arquitectura y diseño
- API REST (tabla de endpoints)
- Modelo entidad-relación
- Modelo de dominio
- Funcionalidades implementadas
- Manejo transaccional y evidencia
- Pruebas y resultados
- Accesibilidad
- Conclusiones
- Bitácora
- Referencias

1. Introducción y objetivos
---------------------------
Describir el problema real, objetivos específicos del sistema y límites del proyecto.

2. Especificación de la necesidad
---------------------------------
Requisitos funcionales y no funcionales verificados. No incluir funciones no implementadas.

3. Arquitectura y diseño
------------------------
Describir arquitectura por capas (API, Business, Data, Domain, WPF cliente). Incluir diagrama de componentes y flujo de datos.

4. API REST (tabla de endpoints)
--------------------------------
Incluir tabla con ruta, verbo, descripción, parámetros, código de respuesta esperado. Ejemplo:

| Ruta | Verbo | Descripción | Parámetros | Respuestas |
|---|---:|---|---|---|
| /api/catalogos/bancos | GET | Listar bancos | - | 200 OK |

5. Modelo ER
------------
Incluir el diagrama Mermaid generado en docs/diagramas/ER.md y una explicación de las relaciones principales.

6. Modelo de dominio
--------------------
Incluir docs/diagramas/DOMINIO.md y explicar las responsabilidades de las clases principales.

7. Funcionalidades implementadas
--------------------------------
- CRUD cuentas bancarias
- Gestión de catálogos (bancos, compañías, clientes, productos)
- Orden maestro-detalle con manejo de inventario e impuesto
- Interfaz WPF para CRUD y órdenes
- Swagger y documentación básica

8. Manejo transaccional y evidencia
----------------------------------
Incluir el contenido de docs/EVIDENCIA_TRANSACCIONES.md (resumen) y anexar las capturas/resultados SQL obtenidos durante pruebas.

9. Pruebas
----------
Lista de pruebas realizadas (unitarias, integración, manuales). Incluir resultados del .http y de la WPF (docs/EVIDENCIA_CRUD_CUENTAS.md y docs/EVIDENCIA_ORDEN_WPF.md).

10. Accesibilidad
-----------------
Describir medidas básicas de accesibilidad implementadas en la interfaz WPF.

11. Conclusiones
----------------
Conclusiones reales basadas en pruebas y limitaciones conocidas.

12. Bitácora
-----------
Registrar tareas, responsables y fechas. No atribuir trabajo no verificado.

13. Referencias
--------------
Formato APA 7 para referencias técnicas y recursos utilizados.

Anexos
------
- SQL de creación de base de datos: Proyecto_backend/database/00_create_salespro.sql
- Archivo de pruebas HTTP: Proyecto_backend/SalesPro.Api/SalesPro.Api.http
- Evidencias: docs/EVIDENCIA_TRANSACCIONES.md, docs/EVIDENCIA_CRUD_CUENTAS.md, docs/EVIDENCIA_ORDEN_WPF.md

Conversión a DOCX/PDF:
- Sugerencia: usar pandoc para convertir este markdown a docx y pdf una vez completado:

```powershell
pandoc docs/informe/Informe_Final_Proyecto2_Equipo5.md -o docs/informe/Informe_Final_Proyecto2_Equipo5.docx
pandoc docs/informe/Informe_Final_Proyecto2_Equipo5.md -o docs/informe/Informe_Final_Proyecto2_Equipo5.pdf
```

Nota: completar secciones y anexar evidencia real antes de generar el PDF final.
