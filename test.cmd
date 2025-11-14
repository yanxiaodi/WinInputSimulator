@echo off
setlocal

echo ?? WinInputSimulator Test Runner
echo.

REM Check if solution file exists
if not exist "WinInputSimulator.sln" (
    echo ? Solution file not found: WinInputSimulator.sln
    exit /b 1
)

echo ?? Restoring packages...
dotnet restore WinInputSimulator.sln
if %errorlevel% neq 0 (
    echo ? Package restore failed
    exit /b %errorlevel%
)

echo ?? Building solution...
dotnet build WinInputSimulator.sln --configuration Release --no-restore
if %errorlevel% neq 0 (
    echo ? Build failed
    exit /b %errorlevel%
)

echo ?? Running tests...
dotnet test tests\WinInputSimulator.Tests\WinInputSimulator.Tests.csproj --configuration Release --no-build --verbosity normal

if %errorlevel% equ 0 (
    echo.
    echo ? All tests passed!
) else (
    echo.
    echo ? Some tests failed!
)

exit /b %errorlevel%