# Guia de defensa

Esta guia resume los puntos que cada integrante debe poder explicar durante la defensa del proyecto.

Regla del equipo:

```text
Si no puede explicar el codigo en defensa, no entra al main.
```

## Aspectos no aceptables

- Pegar codigo que no se entiende.
- Meter frameworks, paquetes o patrones raros que no se hablaron.
- Cambiar la arquitectura por capricho.
- Saltarse API, Business o Data.
- Quemar datos en WPF para que “se vea bonito” pero no consuma API.
- Romper transacciones, inventario o calculo de IVA.

## Cada integrante debe poder explicar

Debe poder responder:

- que problema resolvio;
- que archivos toco;
- que endpoint o pantalla afecta;
- que validaciones tiene;
- como se prueba en `.http` o en WPF;
- que pasa si falla.

## Accesibilidad digital

Para la defensa se puede indicar que la interfaz WPF incorpora ajustes de accesibilidad relacionados con la Ley 7600:

- nombres accesibles en controles principales mediante `AutomationProperties.Name`;
- textos de ayuda mediante `AutomationProperties.HelpText` y `ToolTip`;
- orden de tabulación para navegación por teclado;
- mensajes de estado con `AutomationProperties.LiveSetting`;
- encabezados de pantalla marcados con `AutomationProperties.HeadingLevel`.

Estos ajustes no sustituyen la validación final con usuario o lector de pantalla, pero dejan la interfaz preparada para una revisión básica de accesibilidad.

## Selección de cliente en la orden

La pantalla de nueva orden no depende de escribir el identificador del cliente manualmente. La vista abre una ventana de búsqueda de cliente, consulta el catálogo por API y asigna el cliente seleccionado al ViewModel de la orden. Al procesar la orden se envía el `ClienteId` seleccionado al backend.

## Frase para defensa

> El proyecto sigue una arquitectura por capas parecida al Laboratorio 3: dominio, contratos, datos, negocio, API y WPF. La WPF consume la API REST y la capa de datos usa ADO.NET. La orden se procesa con transaccion para asegurar que el inventario y el encabezado/detalle queden consistentes o no se guarde nada.

