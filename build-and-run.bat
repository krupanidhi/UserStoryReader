@echo off
echo Building User Story Reader...
cd UserStoryReader
dotnet restore
dotnet build

if %errorlevel% equ 0 (
    echo.
    echo Build successful! Running application...
    echo.
    dotnet run
) else (
    echo.
    echo Build failed! Please check the errors above.
    pause
)
