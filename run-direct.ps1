# PowerShell script to run UserStoryReader directly
Write-Host "=== User Story Reader - Direct Run ===" -ForegroundColor Green
Write-Host ""

# Change to project directory
Set-Location "C:\Users\kpeterson\CascadeProjects\UserStoryReader\UserStoryReader"

# Run using dotnet SDK (bypasses executable security issues)
Write-Host "Starting application..." -ForegroundColor Yellow
dotnet run --no-build --verbosity quiet

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
