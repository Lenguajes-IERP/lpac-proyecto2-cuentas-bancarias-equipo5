# GitHub organización - Equipo 5

La captura muestra que existe el equipo:

```text
Lenguajes-IERP/equipo-5
```

con 4 miembros y rol `admin`.

## ¿Se puede subir el proyecto a esa organización?

Sí, si la organización permite crear repositorios o si alguien con permisos lo crea.

Repositorio sugerido:

```text
Lenguajes-IERP/proyecto2-salespro-equipo-5
```

## Comandos sugeridos

Ejecutar desde:

```text
C:\Users\sebas\Documents\Lenguajes\Proyecto2_SalesPro
```

```powershell
git init
git add .
git commit -m "Base inicial Proyecto 2 SalesPro Equipo 5"
git branch -M main
git remote add origin https://github.com/Lenguajes-IERP/proyecto2-salespro-equipo-5.git
git push -u origin main
```

## Control de cambios por persona

GitHub no permite, en un mismo repositorio normal, decir:

```text
Alejandro solo puede escribir en /src/SalesPro.Wpf
Josue solo puede escribir en /src/SalesPro.Api
```

al nivel de permisos de escritura directos.

Lo que sí se puede hacer:

1. Proteger la rama `main`.
2. Obligar Pull Requests.
3. Activar `CODEOWNERS`.
4. Requerir aprobación de code owners.

Así, todos pueden leer y compilar, pero nadie mete cambios a `main` sin revisión.

## Configuración recomendada en GitHub

En el repo:

```text
Settings → Branches → Add branch protection rule
```

Branch name pattern:

```text
main
```

Activar:

- Require a pull request before merging.
- Require approvals: 1.
- Require review from Code Owners.
- Dismiss stale pull request approvals when new commits are pushed.
- Require status checks to pass before merging, si luego agregan CI.
- Do not allow bypassing the above settings.

## Alternativa más estricta

Si quieren control real por permisos, hay que separar el proyecto en varios repositorios:

- `salespro-api`
- `salespro-wpf`
- `salespro-database`
- `salespro-docs`

Pero para este curso no lo recomiendo porque complica la entrega. Mejor un solo repo con PRs y CODEOWNERS.
