#!/usr/bin/env sh

set -eu

SCRIPT_DIR="$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)"
PROJECT_PATH="$SCRIPT_DIR/GestorDeVuelosProyectoFinal.csproj"

if [ ! -f "$PROJECT_PATH" ]; then
  echo "No se encontro el proyecto en: $PROJECT_PATH" >&2
  exit 1
fi

echo "Iniciando GestorDeVuelosProyectoFinal..."
echo "Se aplicaran migraciones y luego se abrira el menu."

exec dotnet run --project "$PROJECT_PATH"
