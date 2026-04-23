using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Shared.ui;

/// <summary>Pantalla inicial de bienvenida antes del menú de acceso.</summary>
public static class ConsoleWelcome
{
    public static void ShowSplash()
    {
        AnsiConsole.Clear();

        // Mostramos una bienvenida simple antes de mandar a login para que el arranque
        // se sienta más intencional y menos seco.
        AnsiConsole.Write(
            new FigletText("VUELOS")
                .Color(Color.DeepSkyBlue1));

        AnsiConsole.WriteLine();
        AnsiConsole.Write(
            new Panel(
                    new Rows(
                        new Markup("[bold white]Bienvenido al Gestor de Vuelos[/]"),
                        new Markup("[grey]Administración de rutas, reservas, tripulación y operaciones aéreas.[/]"),
                        new Markup(" "),
                        new Markup("[dim]Proyecto académico · consola interactiva[/]")))
                .Header("[bold aqua]✈  Bienvenida[/]")
                .Border(BoxBorder.Double)
                .BorderColor(Color.Cyan1)
                .Padding(1, 2));

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Pulsa [bold]Enter[/] para ir al inicio de sesión…[/]");
        AnsiConsole.Prompt(new TextPrompt<string>("").AllowEmpty());
    }
}
