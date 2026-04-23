using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Continents.ui;

public sealed class ContinentsMenu : IModuleUI
{
    private readonly IContinentService _service;

    public string Key => "continents";
    public string Title => "Continentes";

    public ContinentsMenu(IContinentService service)
    {
        _service = service;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        // El menú principal solo decide a qué flujo entrar; cada caso se resuelve aparte.
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Gestión de continentes [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar y Enter para seleccionar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar continentes",
                        "Crear continente",
                        "Actualizar continente",
                        "Eliminar continente",
                        ConsoleMenuHelpers.VolverAlMenu
                    )
            );

            switch (option)
            {
                case "Listar continentes":   await ListAsync();   break;
                case "Crear continente":     await CreateAsync(); break;
                case "Actualizar continente": await UpdateAsync(); break;
                case "Eliminar continente":  await DeleteAsync(); break;
                default:                  return;
            }
        }
    }


    // LIST


    private async Task ListAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Todos los continentes[/]").LeftJustified());

        var continents = (await _service.GetAllAsync()).ToList();

        if (continents.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay continentes registrados.[/]");
        }
        else
        {
            // La tabla se arma con un índice visual porque aquí el usuario no trabaja por id real.
            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn("[bold grey]#[/]").Centered())
                .AddColumn(new TableColumn("[bold grey]Nombre[/]"));

            for (int i = 0; i < continents.Count; i++)
                table.AddRow(
                    $"[grey]{i + 1}[/]",
                    $"[white]{continents[i].Name.Value}[/]"
                );

            AnsiConsole.WriteLine();
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"\n[grey]Total: {continents.Count} continente(s)[/]");
        }

        Pause();
    }
 // CREATE


    private async Task CreateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Crear continente[/]").LeftJustified());
        AnsiConsole.WriteLine();

        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Crear continente"))
        {
            Pause();
            return;
        }

        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack(
            "[deepskyblue1]Nombre del continente:[/]",
            n =>
            {
                if (n.Length < 2 || n.Length > 50)
                    return "El nombre debe tener entre 2 y 50 caracteres.";
                if (n.Any(char.IsDigit))
                    return "El nombre no puede contener números.";
                return null;
            });

        if (name is null)
        {
            AnsiConsole.MarkupLine("[yellow]Operación cancelada (volver).[/]");
            Pause();
            return;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel($"[bold]{name}[/]")
            .Header("[grey]Continente a crear[/]")
            .BorderColor(Color.Grey));

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("\n¿Confirmar creación?");
        if (confirm == ConsoleMenuHelpers.SaveChoice.VolverAlMenu)
        {
            Pause();
            return;
        }

        if (confirm == ConsoleMenuHelpers.SaveChoice.Guardar)
        {
            try
            {
                await AnsiConsole.Status()
                    .StartAsync("Guardando...", async _ =>
                    {
                        await _service.CreateAsync(name);
                    });

                AnsiConsole.MarkupLine("\n[green]Continente creado correctamente.[/]");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
            }
        }
        else
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");

        Pause();
    }

    // UPDATE


    private async Task UpdateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Actualizar continente[/]").LeftJustified());

        // Se trabaja desde lista para que el usuario no tenga que memorizar ids o nombres exactos.
        var continents = (await _service.GetAllAsync()).ToList();

        if (continents.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay continentes para actualizar.[/]");
            Pause();
            return;
        }

        var choices = continents.Select(c => c.Name.Value).ToList();
        choices.Add(ConsoleMenuHelpers.VolverAlMenu);

        AnsiConsole.WriteLine();
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Selecciona el continente a actualizar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(choices)
        );

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        AnsiConsole.WriteLine();
        var newName = ConsoleMenuHelpers.PromptRequiredStringOrBack(
            $"[deepskyblue1]Nuevo nombre para[/] [bold]{selected}[/][deepskyblue1]:[/]",
            n =>
            {
                if (n.Length < 2 || n.Length > 50)
                    return "El nombre debe tener entre 2 y 50 caracteres.";
                if (n.Any(char.IsDigit))
                    return "El nombre no puede contener números.";
                return null;
            });

        if (newName is null)
        {
            AnsiConsole.MarkupLine("[yellow]Operación cancelada (volver).[/]");
            Pause();
            return;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel($"[strikethrough grey]{selected}[/]  [grey]→[/]  [bold]{newName}[/]")
            .Header("[grey]Cambio a aplicar[/]")
            .BorderColor(Color.Grey));

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("\n¿Confirmar actualización?");
        if (confirm == ConsoleMenuHelpers.SaveChoice.VolverAlMenu)
        {
            Pause();
            return;
        }

        if (confirm == ConsoleMenuHelpers.SaveChoice.Guardar)
        {
            try
            {
                await AnsiConsole.Status()
                    .StartAsync("Actualizando...", async _ =>
                    {
                        await _service.UpdateAsync(selected, newName);
                    });

                AnsiConsole.MarkupLine("\n[green]Continente actualizado correctamente.[/]");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
            }
        }
        else
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");

        Pause();
    }


    // DELETE


    private async Task DeleteAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Eliminar continente[/]").LeftJustified());

        var continents = (await _service.GetAllAsync()).ToList();

        if (continents.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay continentes para eliminar.[/]");
            Pause();
            return;
        }

        var choices = continents.Select(c => c.Name.Value).ToList();
        choices.Add(ConsoleMenuHelpers.VolverAlMenu);

        AnsiConsole.WriteLine();
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Selecciona el continente a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(choices)
        );

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel($"[bold red]{selected}[/]")
            .Header("[red]A punto de eliminar[/]")
            .BorderColor(Color.Red));

        AnsiConsole.MarkupLine("\n[red]Advertencia:[/] Esta acción no se puede deshacer.");

        var confirm = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n¿Confirma eliminación?")
                .HighlightStyle(new Style(foreground: Color.Red))
                .AddChoices("Sí, eliminar", "No, cancelar", ConsoleMenuHelpers.VolverAlMenu)
        );

        if (confirm == ConsoleMenuHelpers.VolverAlMenu || confirm == "No, cancelar")
        {
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");
            Pause();
            return;
        }

        try
        {
            await AnsiConsole.Status()
                    .StartAsync("Eliminando...", async _ =>
                {
                    await _service.DeleteAsync(selected);
                });

            AnsiConsole.MarkupLine("\n[green]Continente eliminado correctamente.[/]");
        }
        catch (InvalidOperationException ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }


    // HELPER


    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(
            new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]")
                .AllowEmpty()
        );
    }
}
