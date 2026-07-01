# Módulo Business

Responsable principal:

```text
Josué Delgado Corrales (@JosueDelgadoCorrales)
```

Revisor obligatorio:

```text
Sebastián Cordero (@cbastiancq-lab)
```

## Qué se puede tocar aquí

- Validaciones.
- Reglas de negocio.
- Coordinación entre API y Data.
- Decisiones como: cuenta duplicada, producto repetido, cantidad inválida.

## Qué NO se debe hacer aquí

- No abrir conexiones SQL.
- No usar `SqlCommand`.
- No escribir XAML.
- No acceder directamente a controles WPF.

## Regla de defensa

Esta capa explica “por qué” una operación es válida o inválida.

Ejemplo:

```text
No se puede registrar una orden sin productos.
No se puede registrar una cuenta con banco inexistente.
```
