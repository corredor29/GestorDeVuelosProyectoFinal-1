using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Shared.ui;

/// <summary>
/// Patrones comunes para poder regresar sin guardar durante formularios en consola.
/// </summary>
public static class ConsoleMenuHelpers
{
    // Esta excepción solo se usa internamente para cortar el flujo cuando alguien
    // decide devolverse con Esc desde un campo del formulario.
    private sealed class BackNavigationException : Exception
    {
    }

    public const string VolverAlMenu = "↩ Volver al menú";
    public const string VolverSinGuardar = "↩ Volver (sin guardar)";

    public enum SaveChoice
    {
        Guardar,
        Cancelar,
        VolverAlMenu
    }

    /// <summary>Pregunta de confirmación con tres salidas: guardar, cancelar o volver al menú del módulo.</summary>
    public static SaveChoice PromptSaveCancelOrBack(string titulo = "¿Confirmar acción?")
    {
        var op = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(titulo)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices("Sí, guardar", "No, cancelar", VolverAlMenu));

        return op switch
        {
            "Sí, guardar" => SaveChoice.Guardar,
            "No, cancelar" => SaveChoice.Cancelar,
            _ => SaveChoice.VolverAlMenu
        };
    }

    /// <summary>Texto obligatorio; Esc = volver al menú del módulo (devuelve null).</summary>
    /// <param name="validate">Opcional: devuelve null si es válido, o el mensaje de error en texto plano.</param>
    public static string? PromptRequiredStringOrBack(
        string markupLabel,
        Func<string, string?>? validate = null)
    {
        return PromptStringWithInitialOrBack(markupLabel, string.Empty, allowEmpty: false, validate: validate);
    }

    /// <summary>Texto editable con valor inicial; Esc = volver al menú del módulo (devuelve null).</summary>
    public static string? PromptStringWithInitialOrBack(
        string markupLabel,
        string initialValue,
        bool allowEmpty = false,
        Func<string, string?>? validate = null)
    {
        while (true)
        {
            string input;
            try
            {
                // Toda la edición de campos pasa por el mismo punto para que los formularios
                // mantengan el mismo comportamiento visual.
                input = PromptFieldValue(markupLabel, allowEmpty, initialValue);
            }
            catch (BackNavigationException)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(input) && allowEmpty)
                return string.Empty;

            var trimmed = input.Trim();
            if (validate != null)
            {
                var err = validate(trimmed);
                if (err is not null)
                {
                    AnsiConsole.MarkupLine($"[red]{Markup.Escape(err)}[/]");
                    continue;
                }
            }

            return trimmed;
        }
    }

    /// <summary>Entero; línea vacía = volver (null).</summary>
    public static int? PromptIntOrBack(string markupLabel)
    {
        while (true)
        {
            string input;
            try
            {
                input = PromptFieldValue(markupLabel, allowEmpty: true);
            }
            catch (BackNavigationException)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (int.TryParse(input.Trim(), out var v))
                return v;

            AnsiConsole.MarkupLine("[red]Debe ser un número entero válido.[/]");
        }
    }

    /// <summary>Entero positivo (&gt; 0); vacío = volver (null).</summary>
    public static int? PromptPositiveIntOrBack(string markupLabel)
    {
        while (true)
        {
            var v = PromptIntOrBack(markupLabel);
            if (v is null)
                return null;

            if (v.Value > 0)
                return v.Value;

            AnsiConsole.MarkupLine("[red]Debe ser mayor que cero.[/]");
        }
    }

    /// <summary>Entero positivo editable con valor inicial; Esc = volver (null).</summary>
    public static int? PromptPositiveIntWithInitialOrBack(string markupLabel, int initialValue)
    {
        while (true)
        {
            string input;
            try
            {
                input = PromptFieldValue(markupLabel, allowEmpty: false, initialValue.ToString());
            }
            catch (BackNavigationException)
            {
                return null;
            }

            if (!int.TryParse(input.Trim(), out var value))
            {
                AnsiConsole.MarkupLine("[red]Debe ser un número entero válido.[/]");
                continue;
            }

            if (value <= 0)
            {
                AnsiConsole.MarkupLine("[red]Debe ser mayor que cero.[/]");
                continue;
            }

            return value;
        }
    }

    /// <summary>Decimal; línea vacía = volver (null).</summary>
    public static decimal? PromptDecimalOrBack(string markupLabel)
    {
        while (true)
        {
            string input;
            try
            {
                input = PromptFieldValue(markupLabel, allowEmpty: true);
            }
            catch (BackNavigationException)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(input))
                return null;

            // Aceptamos tanto formato invariante como el del sistema para no volver
            // la captura decimal demasiado estricta.
            if (decimal.TryParse(input.Trim(), System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var v))
                return v;

            if (decimal.TryParse(input.Trim(), out v))
                return v;

            AnsiConsole.MarkupLine("[red]Número decimal no válido.[/]");
        }
    }

    /// <summary>Fecha/hora; vacío = volver (null). Acepta varios formatos comunes.</summary>
    public static DateTime? PromptDateTimeOrBack(string markupLabel, params string[] formats)
    {
        var fmts = (formats is { Length: > 0 }
                ? formats
                : new[]
                {
                    "yyyy-MM-dd HH:mm",
                    "yyyy-MM-dd",
                    "dd-MM-yyyy HH:mm",
                    "dd-MM-yyyy",
                    "dd/MM/yyyy HH:mm",
                    "dd/MM/yyyy"
                })
            .Distinct()
            .ToArray();

        while (true)
        {
            string input;
            try
            {
                input = PromptFieldValue(markupLabel, allowEmpty: true);
            }
            catch (BackNavigationException)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(input))
                return null;

            var text = input.Trim();

            if (DateTime.TryParseExact(
                    text,
                    fmts,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AllowWhiteSpaces,
                    out var dt))
                return dt;

            if (DateTime.TryParse(text, out dt))
                return dt;

            AnsiConsole.MarkupLine("[red]Fecha/hora no válida.[/]");
            AnsiConsole.MarkupLine($"[grey]Formatos sugeridos: {Markup.Escape(string.Join(", ", fmts))}[/]");
        }
    }

    /// <summary>Antes de un formulario de varios pasos: continuar o salir.</summary>
    public static bool TryBeginFormOrBack(string tituloSeccion)
    {
        AnsiConsole.WriteLine();
        var op = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[grey]{tituloSeccion}[/]")
                .HighlightStyle(new Style(foreground: Color.Grey))
                .AddChoices("▶ Continuar con el formulario", VolverSinGuardar));

        return op != VolverSinGuardar;
    }

    private static string PromptFieldValue(string markupLabel, bool allowEmpty, string initialValue = "")
    {
        var currentValue = initialValue;

        while (true)
        {
            var edited = ReadFieldInput(markupLabel, currentValue, allowEmpty);
            if (edited is null)
                throw new BackNavigationException();

            // Después de escribir el valor dejamos decidir si se acepta, se edita
            // otra vez o se regresa al menú anterior.
            var decision = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[grey]¿Qué deseas hacer con este dato?[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices("Aceptar", "Editar", "Volver"));

            if (decision == "Aceptar")
                return edited;

            if (decision == "Volver")
                throw new BackNavigationException();

            currentValue = edited;
        }
    }

    private static string? ReadFieldInput(string markupLabel, string currentValue, bool allowEmpty)
    {
        AnsiConsole.MarkupLine($"\n{markupLabel} [grey](Esc para volver)[/]");
        AnsiConsole.MarkupLine("[grey]Da Esc para salir y Enter para continuar.[/]");

        Console.Write("> ");
        Console.Write(currentValue);

        var buffer = new System.Text.StringBuilder(currentValue);

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine();
                return null;
            }

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                var value = buffer.ToString();
                if (!allowEmpty && string.IsNullOrWhiteSpace(value))
                    continue;
                return value;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (buffer.Length == 0)
                    continue;

                buffer.Length--;
                Console.Write("\b \b");
                continue;
            }

            if (char.IsControl(key.KeyChar))
                continue;

            buffer.Append(key.KeyChar);
            Console.Write(key.KeyChar);
        }
    }
}
