#!/usr/bin/env pwsh
# Test script for WinInputSimulator
# Usage: .\test.ps1 [options]

param(
    [string]$Configuration = "Debug",
    [string]$Filter = "",
    [switch]$Coverage = $false,
    [switch]$Watch = $false,
    [switch]$Verbose = $false,
    [switch]$Help = $false
)

if ($Help) {
    Write-Host "WinInputSimulator Test Script"
    Write-Host "Usage: .\test.ps1 [options]"
    Write-Host ""
    Write-Host "Options:"
    Write-Host "  -Configuration <Debug|Release>  Build configuration (default: Debug)"
    Write-Host "  -Filter <pattern>               Test filter pattern"
    Write-Host "  -Coverage                       Collect code coverage"
    Write-Host "  -Watch                          Run tests in watch mode"
    Write-Host "  -Verbose                        Verbose output"
    Write-Host "  -Help                           Show this help"
    Write-Host ""
    Write-Host "Examples:"
    Write-Host "  .\test.ps1                                    # Run all tests"
    Write-Host "  .\test.ps1 -Coverage                          # Run with coverage"
    Write-Host "  .\test.ps1 -Filter 'InputSimulatorTests'     # Run specific tests"
    Write-Host "  .\test.ps1 -Watch                             # Run in watch mode"
    exit 0
}

$SolutionFile = "WinInputSimulator.sln"
$TestProject = "tests\WinInputSimulator.Tests\WinInputSimulator.Tests.csproj"

Write-Host "?? WinInputSimulator Test Runner" -ForegroundColor Cyan
Write-Host "Configuration: $Configuration" -ForegroundColor Yellow

# Check if solution file exists
if (!(Test-Path $SolutionFile)) {
    Write-Error "Solution file not found: $SolutionFile"
    exit 1
}

# Restore packages
Write-Host "?? Restoring packages..." -ForegroundColor Green
dotnet restore $SolutionFile
if ($LASTEXITCODE -ne 0) {
    Write-Error "Package restore failed"
    exit $LASTEXITCODE
}

# Build solution
Write-Host "?? Building solution..." -ForegroundColor Green
dotnet build $SolutionFile --configuration $Configuration --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed"
    exit $LASTEXITCODE
}

# Prepare test command
$TestArgs = @(
    $TestProject,
    "--configuration", $Configuration,
    "--no-build"
)

if ($Filter) {
    $TestArgs += "--filter"
    $TestArgs += $Filter
}

if ($Verbose) {
    $TestArgs += "--verbosity"
    $TestArgs += "detailed"
    $TestArgs += "--logger"
    $TestArgs += "console;verbosity=detailed"
} else {
    $TestArgs += "--verbosity"
    $TestArgs += "normal"
}

if ($Coverage) {
    $TestArgs += "--collect:XPlat Code Coverage"
    $TestArgs += "--settings"
    $TestArgs += "tests\test.runsettings"
    $TestArgs += "--results-directory"
    $TestArgs += "TestResults"
}

if ($Watch) {
    # Watch mode
    Write-Host "?? Running tests in watch mode..." -ForegroundColor Green
    Write-Host "Press Ctrl+C to stop" -ForegroundColor Yellow
    dotnet watch test @TestArgs
} else {
    # Single run
    Write-Host "?? Running tests..." -ForegroundColor Green
    dotnet test @TestArgs
}

$ExitCode = $LASTEXITCODE

if ($Coverage -and $ExitCode -eq 0) {
    Write-Host ""
    Write-Host "?? Code coverage results:" -ForegroundColor Green
    
    # Find coverage files
    $CoverageFiles = Get-ChildItem -Path "TestResults" -Filter "*.xml" -Recurse | Where-Object { $_.Name -like "*coverage*" }
    
    if ($CoverageFiles) {
        Write-Host "Coverage files generated in TestResults folder:" -ForegroundColor Yellow
        $CoverageFiles | ForEach-Object { Write-Host "  $($_.FullName)" -ForegroundColor Gray }
        
        # Try to open coverage report if reportgenerator is available
        if (Get-Command reportgenerator -ErrorAction SilentlyContinue) {
            Write-Host "Generating HTML coverage report..." -ForegroundColor Green
            reportgenerator -reports:"TestResults\**\*.xml" -targetdir:"TestResults\CoverageReport" -reporttypes:Html
            
            if ($LASTEXITCODE -eq 0) {
                $ReportPath = Join-Path (Get-Location) "TestResults\CoverageReport\index.html"
                Write-Host "Coverage report generated: $ReportPath" -ForegroundColor Green
                
                # Ask to open report
                $OpenReport = Read-Host "Open coverage report in browser? (y/n)"
                if ($OpenReport -eq "y" -or $OpenReport -eq "Y") {
                    Start-Process $ReportPath
                }
            }
        } else {
            Write-Host "?? Tip: Install reportgenerator for HTML coverage reports:" -ForegroundColor Yellow
            Write-Host "   dotnet tool install -g dotnet-reportgenerator-globaltool" -ForegroundColor Gray
        }
    }
}

if ($ExitCode -eq 0) {
    Write-Host ""
    Write-Host "? All tests passed!" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "? Some tests failed!" -ForegroundColor Red
}

exit $ExitCode