using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.UI;

public sealed class PassengerTypesMenu : IModuleUI
{
    private readonly IPassengerTypesService _service;

    public PassengerTypesMenu(IPassengerTypesService service)
    {
        _service = service;
    }

    public string Key => "passenger-types";
    public string Title => "Tipos de pasajero";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Tipos de pasajero [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .PageSize(12)
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todos",
                        "Buscar por ID",
                        "Buscar por nombre",
                        "Buscar por edad (años)",
                        "Crear tipo",
                        "Actualizar tipo",
                        "Eliminar por ID",
                        "Eliminar por nombre",
                        "Volver"));

            switch (option)
            {
                case "Listar todos":
                    await ListAllAsync(cancellationToken);
                    break;
                case "Buscar por ID":
                    await SearchByIdAsync(cancellationToken);
                    break;
                case "Buscar por nombre":
                    await SearchByNameAsync(cancellationToken);
                    break;
                case "Buscar por edad (años)":
                    await SearchByAgeAsync(cancellationToken);
                    break;
                case "Crear tipo":
                    await CreateAsync(cancellationToken);
                    break;
                case "Actualizar tipo":
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

    private async Task ListAllAsync(CancellationToken cancellationToken)
    {
        RenderTable(await _service.GetAllAsync(cancellationToken), "Tipos de pasajero");
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        RenderTable(item is null ? Array.Empty<PassengerType>() : new[] { item }, $"ID {id.Value}");
        Pause();
    }

    private async Task SearchByNameAsync(CancellationToken cancellationToken)
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre:");
        if (name is null)
            return;

        var item = await _service.GetByNameAsync(name, cancellationToken);
        RenderTable(item is null ? Array.Empty<PassengerType>() : new[] { item }, name);
        Pause();
    }

    private async Task SearchByAgeAsync(CancellationToken cancellationToken)
    {
        var age = PromptNonNegativeIntOrBack("Edad en años:");
        if (age is null)
            return;

        var item = await _service.GetByAgeAsync(age.Value, cancellationToken);
        RenderTable(item is null ? Array.Empty<PassengerType>() : new[] { item }, $"Edad {age.Value}");
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar tipo de pasajero"))
            return;

        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre:");
        if (name is null)
            return;

        var minAge = PromptOptionalIntOrBack("Edad mínima (vacío = omitir):");
        if (minAge.WentBack)
            return;

        var maxAge = PromptOptionalIntOrBack("Edad máxima (vacío = omitir):");
        if (maxAge.WentBack)
            return;

        try
        {
            await _service.CreateAsync(name, minAge.Value, maxAge.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Tipo creado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var current = await PromptPassengerTypeSelectionAsync("Seleccione el tipo a actualizar:", cancellationToken);
        if (current is null)
            return;

        var name = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nombre:", current.Name.Value);
        if (name is null)
            return;

        var minAge = PromptOptionalIntOrBack("Edad mínima (vacío = omitir):", current.MinAge.Value);
        if (minAge.WentBack)
            return;

        var maxAge = PromptOptionalIntOrBack("Edad máxima (vacío = omitir):", current.MaxAge.Value);
        if (maxAge.WentBack)
            return;

        try
        {
            await _service.UpdateAsync(current.Id!.Value, name, minAge.Value, maxAge.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Tipo actualizado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync(CancellationToken cancellationToken)
    {
        var current = await PromptPassengerTypeSelectionAsync("Seleccione el tipo a eliminar:", cancellationToken);
        if (current is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmar eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByIdAsync(current.Id!.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Eliminado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByNameAsync(CancellationToken cancellationToken)
    {
        var current = await PromptPassengerTypeSelectionAsync("Seleccione el tipo a eliminar:", cancellationToken);
        if (current is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmar eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByNameAsync(current.Name.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Eliminado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    

    private static void RenderTable(IEnumerable<PassengerType> items, string title)
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
            .AddColumn("[bold grey]name[/]")
            .AddColumn("[bold grey]min_age[/]")
            .AddColumn("[bold grey]max_age[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id?.Value.ToString() ?? "-",
                item.Name.Value,
                item.MinAge.Value?.ToString() ?? "-",
                item.MaxAge.Value?.ToString() ?? "-");
        }

        AnsiConsole.Write(table);
    }

    private static int? PromptNonNegativeIntOrBack(string label, int? current = null)
    {
        while (true)
        {
            var value = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString() ?? string.Empty, allowEmpty: false);
            if (value is null)
                return null;

            if (!int.TryParse(value.Trim(), out var parsed))
            {
                AnsiConsole.MarkupLine("[red]Valor entero inválido.[/]");
                continue;
            }

            if (parsed < 0)
            {
                AnsiConsole.MarkupLine("[red]No puede ser negativo.[/]");
                continue;
            }

            return parsed;
        }
    }

    private static (bool WentBack, int? Value) PromptOptionalIntOrBack(string label, int? current = null)
    {
        while (true)
        {
            var value = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString() ?? string.Empty, allowEmpty: true);
            if (value is null)
                return (true, null);

            if (string.IsNullOrWhiteSpace(value))
                return (false, null);

            if (int.TryParse(value.Trim(), out var parsed))
                return (false, parsed);

            AnsiConsole.MarkupLine("[red]Valor entero inválido.[/]");
        }
    }

    private async Task<PassengerType?> PromptPassengerTypeSelectionAsync(string title, CancellationToken cancellationToken)
    {
        var types = (await _service.GetAllAsync(cancellationToken))
            .Where(t => t.Id != null)
            .OrderBy(t => t.Name.Value)
            .ToList();
        if (types.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay tipos de pasajero registrados.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(types.Select(FormatPassengerTypeChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return types.First(t => FormatPassengerTypeChoice(t) == selected);
    }

    private static string FormatPassengerTypeChoice(PassengerType type) =>
        $"{type.Id?.Value ?? 0} · {Markup.Escape(type.Name.Value)}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
