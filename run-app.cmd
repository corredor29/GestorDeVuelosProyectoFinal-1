@echo off
setlocal

set "PROJECT_PATH=%~dp0GestorDeVuelosProyectoFinal.csproj"

if not exist "%PROJECT_PATH%" (
    echo No se encontro el proyecto en: %PROJECT_PATH%
    exit /b 1
)

echo Iniciando GestorDeVuelosProyectoFinal...
echo Se aplicaran migraciones y luego se abrira el menu.

dotnet run --project "%PROJECT_PATH%"
