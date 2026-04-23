using GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.ui;

public sealed class AirlinesMenu : IModuleUI
{
    private readonly IAirlinesService _service;

    public string Key => "airlines";
    public string Title => "✈️  Gestión de Aerolíneas";

    public AirlinesMenu(IAirlinesService service)
    {
        _service = service;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        // Este menú combina consultas rápidas con altas/bajas lógicas del catálogo.
        while (!ct.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule($"[yellow]{Title}[/]").RuleStyle("grey").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Seleccione una opción:")
                    .PageSize(12)
                    .AddChoices(new[]
                    {
                        "1. Listar todas las aerolíneas",
                        "2. Listar solo aerolíneas activas",
                        "3. Buscar por ID",
                        "4. Buscar por Nombre",
                        "5. Buscar por IATA",
                        "6. Buscar por País de Origen (ID)",
                        "7. Registrar nueva Aerolínea",
                        "8. Actualizar Aerolínea",
                        "9. Desactivar Aerolínea",
                        "10. Reactivar Aerolínea",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0"))
                break;

            switch (option.Split('.')[0])
            {
                case "1": await ListAllAsync(); break;
                case "2": await ListActiveAsync(); break;
                case "3": await SearchByIdAsync(); break;
                case "4": await SearchByNameAsync(); break;
                case "5": await SearchByIataCodeAsync(); break;
                case "6": await SearchByOriginCountryIdAsync(); break;
                case "7": await CreateAsync(); break;
                case "8": await UpdateAsync(); break;
                case "9": await DeactivateMenuAsync(); break;
                case "10": await ReactivateAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        ShowTable(items, "Todas las Aerolíneas");
        Pause();
    }

    private async Task ListActiveAsync()
    {
        var items = await _service.GetActiveAsync();
        ShowTable(items, "Aerolíneas Activas");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] de la aerolínea:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value);

        if (item is null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ninguna aerolínea con ese ID.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para ID: {id.Value}");

        Pause();
    }

    private async Task SearchByNameAsync()
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]nombre[/] de la aerolínea:");
        if (name is null)
            return;

        var item = await _service.GetByNameAsync(name);

        if (item is null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ninguna aerolínea con ese nombre.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para: {name}");

        Pause();
    }

    private async Task SearchByIataCodeAsync()
    {
        var code = PromptIataCodeOrBack("Ingrese el [green]código IATA[/] de la aerolínea:");
        if (code is null)
            return;

        var item = await _service.GetByIataCodeAsync(code);

        if (item is null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ninguna aerolínea con ese código IATA.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para IATA: {code.ToUpperInvariant()}");

        Pause();
    }

    private async Task SearchByOriginCountryIdAsync()
    {
        // Aquí todavía se trabaja por id del país porque la relación es simple y ya existe el filtro.
        var originCountryId = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID del país de origen[/]:");
        if (originCountryId is null)
            return;

        var items = await _service.GetByOriginCountryIdAsync(originCountryId.Value);
        ShowSearchResults(items, $"Aerolíneas del país #{originCountryId.Value}");
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar Nueva Aerolínea[/]");
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre:");
        if (name is null)
        {
            Pause();
            return;
        }

        var iataCode = PromptIataCodeOrBack("Código IATA (máximo 3 letras):");
        if (iataCode is null)
        {
            Pause();
            return;
        }

        var originCountryId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del país de origen:");
        if (originCountryId is null)
        {
            Pause();
            return;
        }

        var isActive = AnsiConsole.Confirm("¿Registrar como activa?", true);

        if (AnsiConsole.Confirm("¿Desea guardar los cambios?"))
        {
            try
            {
                await _service.CreateAsync(name, iataCode, originCountryId.Value, isActive);
                AnsiConsole.MarkupLine("[green]✅ Aerolínea registrada correctamente.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
            }
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        // Primero se escoge una aerolínea real y luego se precargan los valores editables.
        var item = await PromptSelectAirlineForEditAsync();
        if (item is null)
        {
            Pause();
            return;
        }

        var id = item.Id!.Value;

        AnsiConsole.MarkupLine($"Modificando: [bold]{item.Name.Value}[/] ([blue]{item.IataCode.Value}[/])");
        var name = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo nombre:", item.Name.Value);
        if (name is null)
        {
            Pause();
            return;
        }

        var iataCode = PromptIataCodeOrBack("Nuevo código IATA:", item.IataCode.Value);
        if (iataCode is null)
        {
            Pause();
            return;
        }

        var originCountryId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Nuevo ID del país de origen:", item.OriginCountryId.Value);
        if (originCountryId is null)
        {
            Pause();
            return;
        }

        var isActive = AnsiConsole.Confirm("¿La aerolínea está activa?", item.IsActive.Value);

        try
        {
            await _service.UpdateAsync(id, name, iataCode, originCountryId.Value, isActive);
            AnsiConsole.MarkupLine("[green]✅ Aerolínea actualizada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task<Airline?> PromptSelectAirlineForEditAsync()
    {
        // La etiqueta mezcla nombre e IATA para que la selección sea mucho más clara al usuario.
        var airlines = (await _service.GetAllAsync())
            .OrderBy(a => a.Name.Value)
            .ThenBy(a => a.IataCode.Value)
            .ToList();

        if (airlines.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay aerolíneas registradas.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione la aerolínea a modificar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(airlines.Select(FormatAirlineChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return airlines.First(a => FormatAirlineChoice(a) == selected);
    }

    private async Task DeactivateMenuAsync()
    {
        var airlines = (await _service.GetAllAsync())
            .Where(a => a.IsActive.Value)
            .OrderBy(a => a.Name.Value)
            .ThenBy(a => a.IataCode.Value)
            .ToList();

        if (airlines.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay aerolíneas activas para desactivar.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Seleccione la aerolínea a desactivar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(airlines.Select(FormatAirlineChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var airline = airlines.First(a => FormatAirlineChoice(a) == selected);

        try
        {
            if (!AnsiConsole.Confirm($"[red]¿Desea desactivar la aerolínea {Markup.Escape(airline.Name.Value)} ({Markup.Escape(airline.IataCode.Value)})?[/]"))
            {
                Pause();
                return;
            }

            await _service.DeactivateByIdAsync(airline.Id!.Value);

            AnsiConsole.MarkupLine("[green]✅ Operación procesada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task ReactivateAsync()
    {
        var airlines = (await _service.GetAllAsync())
            .Where(a => !a.IsActive.Value)
            .OrderBy(a => a.Name.Value)
            .ThenBy(a => a.IataCode.Value)
            .ToList();

        if (airlines.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay aerolíneas inactivas para reactivar.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione la aerolínea a reactivar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(airlines.Select(FormatAirlineChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var airline = airlines.First(a => FormatAirlineChoice(a) == selected);

        try
        {
            await _service.ReactivateAsync(airline.Id!.Value);
            AnsiConsole.MarkupLine("[green]✅ Aerolínea reactivada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private static void ShowSearchResults(IEnumerable<Airline> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
            AnsiConsole.MarkupLine("[red]❌ No se encontraron registros.[/]");
        else
            ShowTable(list, title);

        Pause();
    }

    private static void ShowTable(IEnumerable<Airline> items, string title)
    {
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Nombre[/]")
            .AddColumn("[blue]IATA[/]")
            .AddColumn("[green]País Origen ID[/]")
            .AddColumn("[blue]Activo[/]")
            .AddColumn("[grey]Creado[/]")
            .AddColumn("[grey]Actualizado[/]");

        foreach (var item in items)
        {
            table.AddRow(
                (item.Id?.Value ?? 0).ToString(),
                item.Name.Value,
                item.IataCode.Value,
                item.OriginCountryId.Value.ToString(),
                item.IsActive.Value ? "Sí" : "No",
                item.CreatedIn.Value.ToString("yyyy-MM-dd HH:mm"),
                item.UpdatedIn.Value.ToString("yyyy-MM-dd HH:mm"));
        }

        AnsiConsole.Write(table);
    }

    private static string? PromptIataCodeOrBack(string label, string? defaultValue = null)
    {
        return ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            label,
            defaultValue ?? string.Empty,
            allowEmpty: false,
            validate: value =>
            {
                var text = value?.Trim().ToUpperInvariant() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return "El código IATA es obligatorio.";
                if (text.Length != 3)
                    return "El código IATA debe tener exactamente 3 letras.";
                if (!text.All(char.IsLetter))
                    return "El código IATA solo puede contener letras.";
                return null;
            })?.ToUpperInvariant();
    }

    private static string FormatAirlineChoice(Airline airline) =>
        $"{airline.Id?.Value ?? 0} · {Markup.Escape(airline.Name.Value)} · {Markup.Escape(airline.IataCode.Value)}";

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }
}
