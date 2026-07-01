# Guía de defensa — Caleb Hernández

Esta guía está basada en el código real del proyecto. Cada pregunta incluye
la respuesta que podés dar y la línea de código que la respalda.

---

## 1. ¿Qué hiciste tú en el proyecto?

> "Me encargué de la documentación y las pruebas de la API. Específicamente:
> - Completé el informe final con la tabla de todos los endpoints, los diagramas
>   de dominio y entidad-relación, la tabla de pruebas y las conclusiones.
> - Actualicé la bitácora con el historial real de commits del equipo.
> - Llené los tres archivos de evidencia ejecutando la API contra la base de datos
>   real y capturando las respuestas HTTP y las salidas SQL.
> - Limpié el archivo `.http` eliminando bloques duplicados y agregué los casos de
>   error que faltaban para el endpoint `PUT` de cuentas bancarias."

---

## 2. ¿Qué es el archivo `.http` y para qué sirve?

> "Es un archivo de pruebas que el IDE (Visual Studio / VS Code con la extensión
> REST Client) puede ejecutar directamente. Cada bloque con `###` es una petición
> HTTP independiente. Lo usamos para probar todos los endpoints de la API sin
> necesidad de herramientas externas como Postman.
>
> El archivo cubre tres recursos: catálogos, cuentas bancarias y órdenes.
> Para cada recurso incluye el caso feliz y los casos de error esperados
> (400, 404, 409)."

---

## 3. ¿Cuál es la arquitectura del sistema?

```
WPF (MVVM) → HTTP → SalesPro.Api → SalesPro.Business → SalesPro.Data → SQL Server
```

> "El sistema tiene cuatro capas:
>
> - **Api**: recibe la petición HTTP, valida el formato y delega al Business.
> - **Business**: aplica las reglas del negocio (¿la divisa es válida?, ¿el número
>   de cuenta ya existe?, ¿hay stock?). No sabe nada de SQL.
> - **Data**: ejecuta el SQL con ADO.NET. Es la única capa que toca la base de datos.
> - **Domain/Contracts**: clases compartidas — las excepciones (`NotFoundException`,
>   `ConflictException`, etc.) y los DTOs (objetos de transferencia de datos).
>
> Esta separación sirve para que si mañana cambian la base de datos o la interfaz,
> las reglas de negocio no cambian."

---

## 4. ¿Por qué algunos endpoints devuelven 400, otros 404 y otros 409?

> "Eso lo define el tipo de excepción que lanza el código de negocio."

El archivo `SalesPro.Domain/Exceptions/SalesProException.cs` tiene tres clases:

```csharp
// 400 - dato inválido en la solicitud
public sealed class ValidationFailureException : SalesProException
{
    public override int StatusCode => 400;
    public override string ErrorCode => "validation_error";
}

// 404 - el recurso no existe
public sealed class NotFoundException : SalesProException
{
    public override int StatusCode => 404;
    public override string ErrorCode => "not_found";
}

// 409 - existe un conflicto con el estado actual de la BD
public sealed class ConflictException : SalesProException
{
    public override int StatusCode => 409;
    public override string ErrorCode => "conflict";
}
```

> "La API tiene un middleware que atrapa estas excepciones y convierte el
> `StatusCode` de la excepción en el código HTTP de la respuesta.
> Así el controller nunca tiene código de manejo de errores: solo delega."

---

## 5. ¿Qué validaciones hace el sistema al crear una cuenta bancaria?

En `CuentaBancariaService.cs`, el método `ValidarAsync` (línea 72) hace esto en orden:

1. **Campos obligatorios** — número, tipo, divisa, país, provincia, nombre, apellidos.
   Si alguno está vacío lanza `ValidationFailureException` (400).

2. **Tipo de cuenta** — solo acepta `Corriente`, `Ahorro` o `Planilla`.
   Si viene otro valor → 400.

3. **Tipo de divisa** — solo acepta `CRC`, `USD` o `EUR`.
   Si viene otro valor (como "CHAYOTE") → 400.

4. **Banco existe** — consulta la base de datos para verificar que el `bancoId` sea real.
   Si no existe → 400.

5. **Compañía existe** — mismo proceso para `companiaId`.
   Si no existe → 400.

6. **Número de cuenta duplicado** — verifica que no exista ya esa combinación de
   número de cuenta + banco. Si ya existe → `ConflictException` (409).

> "El 409 es Conflict porque el problema no es que el dato esté mal formado,
> sino que choca con algo que ya existe en la base de datos."

---

## 6. ¿Cómo funciona la transacción de la orden de venta? (Pregunta clave)

En `OrdenRepository.cs`, el método `CrearOrdenAsync` (línea 18) hace lo siguiente:

```csharp
using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
var committed = false;

try
{
    // 1. Validar que el cliente exista y esté activo
    await ValidarClienteAsync(...);

    // 2. Para cada producto en el detalle:
    foreach (var detalle in request.Detalles)
    {
        // 2a. Leer el producto CON BLOQUEO (WITH UPDLOCK, ROWLOCK)
        var producto = await ObtenerProductoBloqueadoAsync(...);

        // 2b. Verificar que puede venderse
        if (!producto.PuedeVenderse) throw new ConflictException(...);

        // 2c. Verificar que hay stock suficiente
        if (producto.ExistenciaEnStock < detalle.Cantidad)
            throw new ConflictException("Stock insuficiente...");

        // 2d. Calcular subtotal e impuesto
    }

    // 3. Insertar el encabezado de la orden en Pos_Orden
    var numeroOrden = await InsertarEncabezadoAsync(...);

    // 4. Para cada detalle: descontar inventario e insertar la línea
    foreach (var detalle in detallesCalculados)
    {
        await DescontarInventarioAsync(...);
        await InsertarDetalleAsync(...);
    }

    // 5. Si TODO salió bien → commit
    transaction.Commit();
    committed = true;
}
catch
{
    // Si algo falló antes del commit → rollback
    if (!committed)
        transaction.Rollback();
    throw; // propagar el error al controller
}
```

> "La variable `committed` es el truco clave: si el `Commit()` nunca se ejecuta
> (porque una excepción interrumpió el flujo antes), el `catch` llama a `Rollback()`.
> Así la base de datos vuelve exactamente al estado anterior, sin registros a medias."

---

## 7. ¿Por qué el `SELECT` del producto usa `WITH (UPDLOCK, ROWLOCK)`?

```sql
SELECT ... FROM Producto WITH (UPDLOCK, ROWLOCK) WHERE product_id = @productoId;
```

> "Son hints de SQL Server. `UPDLOCK` pone un bloqueo de actualización en la fila
> desde que se lee, no solo cuando se escribe. `ROWLOCK` limita el bloqueo a esa
> fila específica, no a toda la tabla.
>
> Sirve para evitar una condición de carrera: si dos usuarios hacen una orden del
> mismo producto al mismo tiempo, el segundo espera a que el primero termine.
> Sin ese bloqueo, ambos podrían leer el stock como `10` y ambos creer que tienen
> stock, pero solo había para uno."

---

## 8. ¿Cómo se calcula el IVA? ¿Está fijo en el código?

> "No. El IVA se lee dinámicamente de la tabla `ParametroSistema` cada vez que
> se crea una orden."

En `OrdenService.cs` (línea 24):

```csharp
var porcentajeImpuestoVenta = await _parametroSistemaRepository
    .ObtenerValorDecimalAsync("IVA", cancellationToken);
```

> "La base de datos tiene un registro con `nombre = 'IVA'` y `valor_decimal = 13`.
> Si el día de mañana el IVA cambia, solo hay que actualizar ese registro en la base
> de datos. No hay que cambiar ni recompilar el código."

**Fórmula:**
```
impuesto_por_linea = precio_unitario × cantidad × (IVA / 100)
total = suma_subtotales + suma_impuestos
```

En el código (línea 54-56 de `OrdenRepository.cs`):
```csharp
var impuesto = producto.TieneImpuesto
    ? Math.Round(subtotal * porcentajeImpuestoVenta / 100m, 2, MidpointRounding.AwayFromZero)
    : 0m;
```

> "Si el producto no tiene impuesto (`tiene_impuesto = 0`), su línea no suma IVA."

---

## 9. ¿Qué es un DTO y por qué lo usan?

> "DTO significa Data Transfer Object. Es una clase simple que solo tiene datos,
> sin lógica. Lo usamos para no exponer directamente las entidades del dominio
> en la API.
>
> Por ejemplo, `CuentaBancariaDto` devuelve el nombre del banco (`bancoNombre`)
> en vez de solo el `bancoId`. Eso es conveniente para la interfaz, pero la entidad
> de dominio solo tiene el ID. El DTO hace el puente entre lo que la base de datos
> guarda y lo que el cliente necesita ver."

---

## 10. ¿Qué diferencia hay entre `SalesPro.Domain` y `SalesPro.Contracts`?

> "- `SalesPro.Domain` tiene las entidades del negocio y las excepciones. Es el
>   corazón del sistema — no depende de nada externo.
> - `SalesPro.Contracts` tiene los DTOs, los requests y los responses. Es la
>   'interfaz pública' de la API — lo que el cliente manda y lo que recibe.
>
> Los separamos para que si cambia la forma en que la API expone los datos
> (por ejemplo, agregar un campo nuevo a la respuesta), no tengamos que tocar
> las entidades de dominio."

---

## 11. ¿Qué es el diagrama ER y cuáles son las relaciones principales?

> "El diagrama ER muestra las tablas de la base de datos y cómo se relacionan.
> Las relaciones principales son:
>
> - Una **Compania** puede tener muchas **Compania_Cuenta_Bancaria** (cuentas bancarias).
>   Cada cuenta pertenece a un solo banco y a una sola compañía.
> - Un **Cliente** puede tener muchas **Pos_Orden** (órdenes), pero una orden
>   pertenece a un solo cliente.
> - Una **Pos_Orden** tiene uno o más **Pos_Orden_Detalle** (líneas de detalle).
>   Cada línea referencia un producto."

---

## 12. ¿Qué es el diagrama de dominio y en qué se diferencia del ER?

> "El diagrama de dominio muestra las clases del código, no las tablas de la base
> de datos. La diferencia es que el dominio puede tener lógica, métodos y relaciones
> que no existen como tablas (por ejemplo, una colección `Orden.Detalles` que en SQL
> se representa con un JOIN).
>
> En este proyecto el dominio y el ER son bastante similares porque usamos ADO.NET
> puro (sin ORM como Entity Framework), entonces las clases se mapean manualmente
> a las tablas."

---

## 13. ¿Por qué la evidencia de transacciones muestra el stock antes y después?

> "Para demostrar que la transacción es atómica. El protocolo de prueba fue:
>
> 1. Consulté el stock antes (Laptop: 10, Mouse: 50).
> 2. Creé una orden válida con 1 Laptop y 2 Mouse.
> 3. Consulté el stock después → Laptop bajó a 9, Mouse a 48. La transacción
>    modificó el inventario correctamente.
> 4. Intenté crear una orden con 999,999 unidades de Laptop (stock insuficiente).
> 5. La API respondió 409 Conflict.
> 6. Consulté el stock nuevamente → Laptop sigue en 9. El rollback funcionó:
>    no quedó ningún registro parcial en la base de datos."

---

## 14. Posibles preguntas trampa del profesor

**¿Qué pasa si falla el `Commit()` mismo?**
> "El `catch` lo atrapa. La variable `committed` sigue siendo `false`, así que
> el `Rollback()` se ejecuta. La base de datos queda en estado limpio."

**¿Por qué 409 para stock insuficiente y no 400?**
> "Porque el 400 significa que la solicitud tiene un formato o dato inválido.
> La solicitud de stock insuficiente está bien formada — el problema es que
> el estado actual de la base de datos no permite cumplirla. Eso es un conflicto,
> que es exactamente lo que significa el código 409."

**¿Qué pasa si se manda el mismo producto dos veces en una orden?**
> "El `OrdenService.ValidarSolicitud()` lo detecta antes de llegar a la base de
> datos. Agrupa los detalles por `productoId` y si algún grupo tiene más de una
> entrada, lanza `ValidationFailureException` (400). Esto evita inconsistencias
> porque si alguien manda el mismo producto en dos líneas, no se sabe si quería
> sumar las cantidades o duplicar la línea."

**¿Por qué usan ADO.NET y no Entity Framework?**
> "Probablemente por requisito del curso o para tener control total sobre el SQL
> que se ejecuta, especialmente en la transacción de la orden donde se usan hints
> de bloqueo (`UPDLOCK`, `ROWLOCK`) que serían difíciles de expresar con un ORM."

---

## Resumen rápido para memorizar

| Concepto | Respuesta en una línea |
|---|---|
| ¿Qué hice yo? | Documentación, bitácora, evidencias con datos reales y limpieza del `.http` |
| ¿Qué es el `.http`? | Archivo de pruebas de la API que ejecuta el IDE directamente |
| ¿Por qué 400? | El dato está mal (divisa inválida, campo vacío, cantidad negativa) |
| ¿Por qué 404? | El recurso no existe en la base de datos |
| ¿Por qué 409? | Hay un conflicto con el estado actual (duplicado, stock insuficiente) |
| ¿Cómo funciona la transacción? | `BeginTransaction` → operaciones → `Commit` si todo OK, `Rollback` si algo falla |
| ¿Dónde vive el IVA? | En la tabla `ParametroSistema`, no en el código |
| ¿Qué hace `UPDLOCK`? | Bloquea la fila del producto desde la lectura para evitar condición de carrera |
| ¿DTO vs entidad? | DTO es lo que la API expone; entidad es la clase interna del dominio |
