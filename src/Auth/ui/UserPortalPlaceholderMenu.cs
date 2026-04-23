using GestorDeVuelosProyectoFinal.Auth.Application;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Auth.ui;

// Este menú quedó como respaldo para roles que todavía no tienen portal definitivo.
// Mientras tanto solo muestra el estado actual y deja salir de la sesión.
public sealed class UserPortalPlaceholderMenu
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold yellow]Área de usuario[/]").LeftJustified());
            AnsiConsole.MarkupLine($"[grey]Sesión: {Markup.Escape(ApplicationSession.Username ?? "")}[/]");
            AnsiConsole.MarkupLine("\n[italic]La funcionalidad para el rol usuario se implementará más adelante.[/]\n");

            var opt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Opciones disponibles")
                    .AddChoices("Cerrar sesión"));

            if (opt == "Cerrar sesión")
                return;

            await Task.CompletedTask;
        }
    }
}
