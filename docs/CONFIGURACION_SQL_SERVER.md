# Configuración de SQL Server para el proyecto

El proyecto puede trabajar contra el SQL Server del curso usando un archivo local que no se sube al repositorio.

## 1. Crear configuración local de la API

Copiar este archivo:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.example.json
```

con este nombre:

```text
Proyecto_backend/SalesPro.Api/appsettings.Local.json
```

Luego completar los datos del servidor, usuario y clave.

`appsettings.Local.json` está ignorado por Git, por lo que no debe subirse al repositorio.

## 2. Montar la base de datos

Desde la raíz del repositorio:

```powershell
powershell -ExecutionPolicy Bypass -File .\Proyecto_backend\scripts\setup-sqlserver.ps1 -Server "SERVIDOR_SQL" -User "USUARIO"
```

El script pedirá la clave en pantalla. Esto evita dejar la contraseña escrita en el historial de comandos.

## 3. Levantar la API

```powershell
dotnet run --project .\Proyecto_backend\SalesPro.Api\SalesPro.Api.csproj
```

## 4. Probar endpoints

Abrir:

```text
Proyecto_backend/SalesPro.Api/SalesPro.Api.http
```

Confirmar que `@baseUrl` coincide con el puerto real de la API.
