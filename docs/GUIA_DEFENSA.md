# Guia de defensa

Esta guia resume los puntos que cada integrante debe poder explicar durante la defensa del proyecto.

Regla del equipo:

```text
Si no puede explicar el codigo en defensa, no entra al main.
```

## Uso aceptable de asistencia

- Pedir ayuda para entender errores.
- Generar una primera idea y luego adaptarla a la estructura del proyecto.
- Pedir ejemplos parecidos a lo visto en clase.
- Usarla para documentacion, siempre revisando que calce con el codigo real.

## Aspectos no aceptables

- Pegar codigo que no se entiende.
- Meter frameworks, paquetes o patrones raros que no se hablaron.
- Cambiar la arquitectura por capricho.
- Saltarse API, Business o Data.
- Quemar datos en WPF para que “se vea bonito” pero no consuma API.
- Romper transacciones, inventario o calculo de IVA.

## Si alguien usa IA

Debe poder responder:

- que problema resolvio;
- que archivos toco;
- que endpoint o pantalla afecta;
- que validaciones tiene;
- como se prueba en `.http` o en WPF;
- que pasa si falla.

## Frase para defensa

> El proyecto sigue una arquitectura por capas parecida al Laboratorio 3: dominio, contratos, datos, negocio, API y WPF. La WPF consume la API REST y la capa de datos usa ADO.NET. La orden se procesa con transaccion para asegurar que el inventario y el encabezado/detalle queden consistentes o no se guarde nada.

