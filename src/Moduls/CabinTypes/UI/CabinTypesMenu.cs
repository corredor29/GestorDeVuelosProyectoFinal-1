using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.UI;

public sealed class CabinTypesMenu : IModuleUI
{
    private readonly ICabinTypeService _service;

    public CabinTypesMenu(ICabinTypeService service)
    {
        _service = service;
    }

    public string Key => "cabin-types";
    public string Title => "Tipos de Cabina";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Tipos de Cabina [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices("Listar todos", "Crear tipo", "Actualizar tipo", "Eliminar por ID", "Eliminar por nombre", "Volver"));

            switch (option)
            {
                case "Listar todos": await ListAsync(); break;
                case "Crear tipo": await CreateAsync(); break;
                case "Actualizar tipo": await UpdateAsync(); break;
                case "Eliminar por ID": await DeleteByIdAsync(); break;
                case "Eliminar por nombre": await DeleteByNameAsync(); break;
                case "Volver": return;
            }
        }
    }

    private async Task ListAsync()
    {
        RenderTable(await _service.GetAllAsync(), "Tipos de cabina");
        Pause();
    }

    private async Task CreateAsync()
    {
        var name = PromptRequiredTextOrBack("Nombre del tipo:");
        if (name is null)
        {
            Pause();
            return;
        }
        try
        {
            await _service.CreateAsync(name);
            AnsiConsole.MarkupLine("\n[green]Tipo de cabina creado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{ex.Message}[/]");
        }
        Pause();
    }

    private async Task UpdateAsync()
    {
        var types = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (types.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay tipos de cabina registrados.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el tipo de cabina a modificar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(types.Select(t => $"{t.Id.Value} · {Markup.Escape(t.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var current = types.First(x => $"{x.Id.Value} · {Markup.Escape(x.Name.Value)}" == selected);
        var id = current.Id.Value;

        var name = PromptRequiredTextOrBack("Nuevo nombre:", current.Name.Value);
        if (name is null)
        {
            Pause();
            return;
        }
        try
        {
            await _service.UpdateAsync(id, name);
            AnsiConsole.MarkupLine("\n[green]Tipo de cabina actualizado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{ex.Message}[/]");
        }
        Pause();
    }

    private async Task DeleteByIdAsync()
    {
        var types = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (types.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay tipos de cabina registrados.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Seleccione el tipo de cabina a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(types.Select(t => $"{t.Id.Value} · {Markup.Escape(t.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var type = types.First(x => $"{x.Id.Value} · {Markup.Escape(x.Name.Value)}" == selected);
        if (!AnsiConsole.Confirm("¿Confirmas la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByIdAsync(type.Id.Value);
            AnsiConsole.MarkupLine("\n[green]Tipo de cabina eliminado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{ex.Message}[/]");
        }
        Pause();
    }

    private async Task DeleteByNameAsync()
    {
        await DeleteByIdAsync();
    }

    private static void RenderTable(IEnumerable<CabinType> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay registros para mostrar.[/]");
            return;
        }

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]ID[/]")
            .AddColumn("[bold grey]Nombre[/]");

        foreach (var item in list)
            table.AddRow(item.Id.Value.ToString(), item.Name.Value);

        AnsiConsole.Write(table);
    }

    private static int PromptPositiveInt(string label)
        => AnsiConsole.Prompt(new TextPrompt<int>($"[deepskyblue1]{label}[/]")
            .Validate(v => v > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Debe ser mayor que cero.[/]")));

    private static string? PromptRequiredTextOrBack(string label, string? current = null)
        => ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            $"[deepskyblue1]{label}[/]",
            current ?? string.Empty,
            allowEmpty: false,
            validate: v => string.IsNullOrWhiteSpace(v) ? "Campo obligatorio." : null);

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
