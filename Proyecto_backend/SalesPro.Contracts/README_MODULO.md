# Módulo Contracts / DTOs

Responsables:

```text
YO (@cbastiancq-lab)
Josue Delgado Corrales (@JosueDelgadoCorrales)
Caleb Hernández Vega (@CalebHv21)
Alejandro Porras (@axpew)
```

## Qué se puede tocar aquí

- DTOs.
- Requests.
- Responses.
- Objetos compartidos entre API y WPF.

## Regla

Este módulo es compartido. Si alguien cambia un DTO, debe avisar en el Pull Request porque puede romper API y WPF.

Ejemplo:

```text
Si cambia CrearOrdenRequest, hay que revisar OrdenesController y NuevaOrdenViewModel.
```
