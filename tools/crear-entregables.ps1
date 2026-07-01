Param()

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

$solutionRoot = Resolve-Path ".." | Select-Object -ExpandProperty Path
$dist = Join-Path $solutionRoot "dist"
if (-Not (Test-Path $dist)) { New-Item -ItemType Directory -Path $dist | Out-Null }

Write-Host "Limpiando bin/obj en proyectos (solo en copias temporales, no en origen)" -ForegroundColor Cyan

$tmp = Join-Path $env:TEMP "salespro_entregable_$(Get-Date -Format yyyyMMddHHmmss)"
Remove-Item -Recurse -Force -ErrorAction SilentlyContinue $tmp
New-Item -ItemType Directory -Path $tmp | Out-Null

function Copy-ProjectForZip($paths, $destFolderName) {
	$dest = Join-Path $tmp $destFolderName
	New-Item -ItemType Directory -Path $dest | Out-Null
	foreach ($p in $paths) {
		$src = Join-Path $solutionRoot $p
		if (Test-Path $src) {
			Write-Host "Copiando $src -> $dest" -ForegroundColor Green
			Copy-Item -Path $src -Destination $dest -Recurse -Force -ErrorAction Stop
			# eliminar bin/obj en copia
			Get-ChildItem -Path $dest -Recurse -Directory -Filter bin -ErrorAction SilentlyContinue | ForEach-Object { Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue }
			Get-ChildItem -Path $dest -Recurse -Directory -Filter obj -ErrorAction SilentlyContinue | ForEach-Object { Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue }
		} else {
			Write-Host "Advertencia: ruta no encontrada $src" -ForegroundColor Yellow
		}
	}
	return $dest
}

function Write-BackendSolution($dest) {
	@"
<Solution>
  <Folder Name="/Proyecto_backend/">
    <Project Path="Proyecto_backend/SalesPro.Api/SalesPro.Api.csproj" />
    <Project Path="Proyecto_backend/SalesPro.Business/SalesPro.Business.csproj" />
    <Project Path="Proyecto_backend/SalesPro.Data/SalesPro.Data.csproj" />
    <Project Path="Proyecto_backend/SalesPro.Contracts/SalesPro.Contracts.csproj" />
    <Project Path="Proyecto_backend/SalesPro.Domain/SalesPro.Domain.csproj" />
  </Folder>
</Solution>
"@ | Set-Content -Path (Join-Path $dest "Proyecto_backend.slnx") -Encoding UTF8
}

function Write-WpfSolution($dest) {
	@"
<Solution>
  <Folder Name="/Proyecto_WPF/">
    <Project Path="Proyecto_WPF/SalesPro.Wpf/SalesPro.Wpf.csproj" />
  </Folder>
  <Folder Name="/Proyecto_backend/">
    <Project Path="Proyecto_backend/SalesPro.Contracts/SalesPro.Contracts.csproj" />
  </Folder>
</Solution>
"@ | Set-Content -Path (Join-Path $dest "Proyecto_WPF.slnx") -Encoding UTF8
}

try {
	# Backend zip
	$backendPaths = @(
		"Proyecto_backend",
		"README.md"
	)
	$backendCopy = Copy-ProjectForZip $backendPaths "Proyecto_backend_Equipo5"
	Write-BackendSolution $backendCopy
	$backendZip = Join-Path $dist "Proyecto_backend_Equipo5.zip"
	if (Test-Path $backendZip) { Remove-Item $backendZip -Force }
	Compress-Archive -Path (Join-Path $backendCopy '*') -DestinationPath $backendZip -Force
	Write-Host "Generado: $backendZip" -ForegroundColor Cyan

	# WPF zip
	$wpfPaths = @(
		"Proyecto_WPF",
		"Proyecto_backend",
		"README.md"
	)
	$wpfCopy = Copy-ProjectForZip $wpfPaths "Proyecto_WPF_Equipo5"
	Write-WpfSolution $wpfCopy
	$wpfZip = Join-Path $dist "Proyecto_WPF_Equipo5.zip"
	if (Test-Path $wpfZip) { Remove-Item $wpfZip -Force }
	Compress-Archive -Path (Join-Path $wpfCopy '*') -DestinationPath $wpfZip -Force
	Write-Host "Generado: $wpfZip" -ForegroundColor Cyan

	Write-Host "Limpiando temporales..." -ForegroundColor Gray
	Remove-Item -Recurse -Force $tmp
	Write-Host "Entregables creados en: $dist" -ForegroundColor Green
} catch {
	Write-Host "Error al crear entregables: $_" -ForegroundColor Red
	if (Test-Path $tmp) { Remove-Item -Recurse -Force $tmp -ErrorAction SilentlyContinue }
	exit 1
}
