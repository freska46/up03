@echo off
cd /d "%~dp0PakDzal_Games\PakDzal_Games_API"
echo Building API...
dotnet build
if errorlevel 1 (
    echo Build failed!
    pause
    exit /b 1
)
echo Starting API...
start http://localhost:5222/swagger
dotnet run
pause