using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.ui;

public sealed class SeatLocationTypesMenu : IModuleUI
{
    private readonly ISeatLocationTypesService _service;

    public SeatLocationTypesMenu(ISeatLocationTypesService service)
    {
        _service = service;
    }

    public string Key => "seat-location-types";
    public string Title => "Tipos de asiento";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Tipos de ubicación de asiento [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar y Enter para seleccionar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todos",
                        "Buscar por ID",
                        "Buscar por nombre",
                        "Crear tipo de asiento",
                        "Actualizar tipo de asiento",
                        "Eliminar por ID",
                        "Eliminar por nombre",
                        "Volver"));

            switch (option)
            {
                case "Listar todos":
                    await ListAsync(cancellationToken);
                    break;
                case "Buscar por ID":
                    await SearchByIdAsync(cancellationToken);
                    break;
                case "Buscar por nombre":
                    await SearchByNameAsync(cancellationToken);
                    break;
                case "Crear tipo de asiento":
                    await CreateAsync(cancellationToken);
                    break;
                case "Actualizar tipo de asiento":
                    await UpdateAsync(cancellationToken);
                    break;
                case "Eliminar por ID":
                    await DeleteByIdAsync(cancellationToken);
                    break;
                case "Eliminar por nombre":
                    await DeleteByNameAsync(cancellationToken);
                    break;
                case "Volver":
                    return;
            }
        }
    }

    private async Task ListAsync(CancellationToken cancellationToken)
    {
        RenderTable(await _service.GetAllAsync(cancellationToken), "Tipos de asiento");
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        RenderTable(item is null ? Array.Empty<SeatLocationType>() : new[] { item }, $"ID {id.Value}");
        Pause();
    }

    private async Task SearchByNameAsync(CancellationToken cancellationToken)
    {
        var name = PromptRequiredTextOrBack("Nombre:");
        if (name is null)
            return;

        var item = await _service.GetByNameAsync(name, cancellationToken);
        RenderTable(item is null ? Array.Empty<SeatLocationType>() : new[] { item }, name);
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        var name = PromptRequiredTextOrBack("Nombre del tipo de asiento:");
        if (name is null)
        {
            Pause();
            return;
        }

        try
        {
            await _service.CreateAsync(name, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Tipo de asiento creado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var items = (await _service.GetAllAsync(cancellationToken)).OrderBy(x => x.Name.Value).ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay tipos de asiento registrados.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el tipo de asiento a actualizar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(i => $"{i.Id?.Value ?? 0} · {Markup.Escape(i.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var current = items.First(i => $"{i.Id?.Value ?? 0} · {Markup.Escape(i.Name.Value)}" == selected);
        var id = current.Id!.Value;

        var name = PromptRequiredTextOrBack("Nuevo nombre:", current.Name.Value);
        if (name is null)
        {
            Pause();
            return;
        }

        try
        {
            await _service.UpdateAsync(id, name, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Tipo de asiento actualizado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync(CancellationToken cancellationToken)
    {
        var items = (await _service.GetAllAsync(cancellationToken)).OrderBy(x => x.Name.Value).ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay tipos de asiento registrados.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Seleccione el tipo de asiento a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(i => $"{i.Id?.Value ?? 0} · {Markup.Escape(i.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var item = items.First(i => $"{i.Id?.Value ?? 0} · {Markup.Escape(i.Name.Value)}" == selected);
        if (!AnsiConsole.Confirm("¿Confirmas la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByIdAsync(item.Id!.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Tipo de asiento eliminado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }
    private async Task DeleteByNameAsync(CancellationToken cancellationToken)
    {
        await DeleteByIdAsync(cancellationToken);
    }
    private static void RenderTable(IEnumerable<SeatLocationType> items, string title)
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
        {
            table.AddRow(
                item.Id?.Value.ToString() ?? "-",
                item.Name.Value);
        }

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
