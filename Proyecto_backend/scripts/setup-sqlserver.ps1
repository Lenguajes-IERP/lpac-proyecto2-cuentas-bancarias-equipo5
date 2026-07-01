param(
    [Parameter(Mandatory = $true)]
    [string]$Server,

    [Parameter(Mandatory = $true)]
    [string]$User,

    [Parameter(Mandatory = $false)]
    [string]$Password
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$scriptPath = Join-Path $repoRoot "database\00_create_salespro.sql"

if (-not (Test-Path -LiteralPath $scriptPath)) {
    throw "No se encontró el script de base de datos en: $scriptPath"
}

if ([string]::IsNullOrWhiteSpace($Password)) {
    $securePassword = Read-Host "Clave SQL para $User" -AsSecureString
    $bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
    try {
        $Password = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr)
    } finally {
        [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr)
    }
}

Write-Host ""
Write-Host "== Proyecto 2 / Equipo 5 =="
Write-Host "Montando base SalesPro en SQL Server..."
Write-Host ""

Write-Host "Servidor: $Server"
Write-Host "Usuario: $User"
Write-Host ""

Write-Host "1) Ejecutando database\00_create_salespro.sql"
sqlcmd -S $Server -U $User -P $Password -i $scriptPath -b -C

Write-Host ""
Write-Host "2) Validando datos base"
sqlcmd -S $Server -U $User -P $Password -d SalesPro -Q "SELECT COUNT(*) AS Bancos FROM Banco; SELECT COUNT(*) AS Productos FROM Producto; SELECT nombre, valor_decimal FROM ParametroSistema WHERE nombre = 'IVA';" -b -C

Write-Host ""
Write-Host "Base montada correctamente."
Write-Host "Configure Proyecto_backend/SalesPro.Api/appsettings.Local.json con este servidor."
Write-Host "No suba appsettings.Local.json al repositorio."
Write-Host ""
