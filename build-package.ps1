# PowerShell script to build and pack NuGet package locally

param(
    [string]$Configuration = "Release",
    [string]$OutputPath = ".\nupkg"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "WinInputSimulator - NuGet Package Build" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean -c $Configuration
if (Test-Path $OutputPath) {
    Remove-Item -Path $OutputPath -Recurse -Force
}

# Restore dependencies
Write-Host "`nRestoring dependencies..." -ForegroundColor Yellow
dotnet restore .\src\WinInputSimulator\WinInputSimulator\WinInputSimulator.csproj

# Build
Write-Host "`nBuilding project in $Configuration mode..." -ForegroundColor Yellow
dotnet build .\src\WinInputSimulator\WinInputSimulator\WinInputSimulator.csproj -c $Configuration --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nBuild failed!" -ForegroundColor Red
    exit 1
}

# Pack
Write-Host "`nCreating NuGet package..." -ForegroundColor Yellow
dotnet pack .\src\WinInputSimulator\WinInputSimulator\WinInputSimulator.csproj -c $Configuration --no-build -o $OutputPath

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nPackage created successfully!" -ForegroundColor Green
    Write-Host "Location: $OutputPath" -ForegroundColor Green
    Write-Host ""
    Get-ChildItem $OutputPath -Filter *.nupkg | ForEach-Object {
        Write-Host "  - $($_.Name)" -ForegroundColor Cyan
    }
    Write-Host ""
    Write-Host "To test locally, run:" -ForegroundColor Yellow
    Write-Host "  dotnet nuget add source $OutputPath --name LocalWinInputSimulator" -ForegroundColor White
    Write-Host ""
    Write-Host "To publish to NuGet.org, run:" -ForegroundColor Yellow
    Write-Host "  dotnet nuget push $OutputPath\*.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json" -ForegroundColor White
} else {
    Write-Host "`nPackage creation failed!" -ForegroundColor Red
    exit 1
}
