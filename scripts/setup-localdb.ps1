$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$scriptPath = Join-Path $repoRoot "database\00_create_salespro.sql"

if (-not (Test-Path -LiteralPath $scriptPath)) {
    throw "No encontré el script de base de datos en: $scriptPath"
}

Write-Host ""
Write-Host "== Proyecto 2 / Equipo 5 =="
Write-Host "Montando base local del proyecto..."
Write-Host ""

Write-Host "1) Levantando LocalDB MSSQLLocalDB"
SqlLocalDB start MSSQLLocalDB | Out-Host

Write-Host ""
Write-Host "2) Ejecutando database\00_create_salespro.sql"
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -i $scriptPath -b

Write-Host ""
Write-Host "3) Validando datos base"
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d SalesPro -Q "SELECT COUNT(*) AS Bancos FROM Banco; SELECT COUNT(*) AS Productos FROM Producto; SELECT nombre, valor_decimal FROM ParametroSistema WHERE nombre = 'IVA';"

Write-Host ""
Write-Host "Base montada correctamente en LocalDB."
Write-Host "Connection string para la API:"
Write-Host 'Server=(localdb)\MSSQLLocalDB;Database=SalesPro;Trusted_Connection=True;TrustServerCertificate=True;'
Write-Host ""
