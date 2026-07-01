# Plantilla de evidencia: CRUD de cuentas bancarias en WPF

Objetivo: validar búsqueda, creación, edición y eliminación de cuentas bancarias desde la aplicación WPF consumiendo la API.

Estado actual: pendiente de completar con capturas y resultados reales.

## Preparación

- Levantar la API:

```powershell
dotnet run --project .\Proyecto_backend\SalesPro.Api\SalesPro.Api.csproj
```

- Ejecutar el proyecto `SalesPro.Wpf`.
- Confirmar que la URL base del cliente WPF coincide con el puerto real de la API.

## Datos de prueba sugeridos

- Número: `CR000000000555`
- Tipo: `Corriente`
- Divisa: `CRC`
- Banco: `BAC Credomatic` o id `1`
- Compañía: `SalesPro Demo S.A.` o id `1`
- País: `Costa Rica`
- Provincia: `Cartago`
- Dueño: `Prueba Equipo5`
- Estado: activa

## Checklist manual

### 1. Buscar cuenta

- Acción: usar el buscador por número o dueño.
- Resultado esperado: la tabla muestra coincidencias.
- Evidencia:

```text
PENDIENTE: pegar captura o descripción del resultado.
```

### 2. Crear cuenta

- Acción: completar formulario y guardar.
- Resultado esperado: la cuenta aparece en la lista.
- Evidencia:

```text
PENDIENTE: pegar captura o respuesta.
```

### 3. Editar cuenta

- Acción: seleccionar cuenta, cambiar campos y guardar.
- Resultado esperado: los cambios se reflejan en la lista y en la API.
- Evidencia:

```text
PENDIENTE: pegar captura o respuesta.
```

### 4. Cancelar edición

- Acción: iniciar edición y cancelar.
- Resultado esperado: no se aplican cambios.
- Evidencia:

```text
PENDIENTE: pegar captura o descripción.
```

### 5. Eliminar cuenta

- Acción: seleccionar cuenta de prueba, eliminar y confirmar.
- Resultado esperado: la cuenta desaparece y la API responde correctamente.
- Evidencia:

```text
PENDIENTE: pegar captura o respuesta.
```

### 6. Validaciones

- Moneda inválida.
- Número de cuenta duplicado.
- Campos obligatorios vacíos.

Evidencia:

```text
PENDIENTE: pegar resultados.
```

## Observaciones

```text
PENDIENTE: anotar errores encontrados o confirmar que no hubo.
```

## Conclusión

PENDIENTE: completar después de las pruebas.
