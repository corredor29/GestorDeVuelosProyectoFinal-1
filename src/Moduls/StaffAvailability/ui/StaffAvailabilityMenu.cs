using System.Globalization;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.ui;

public sealed class StaffAvailabilityMenu : IModuleUI
{
    private readonly IStaffAvailabilityService _service;

    public string Key => "staff_availability";
    public string Title => "📆  Gestión de disponibilidad del personal";

    public StaffAvailabilityMenu(IStaffAvailabilityService service)
    {
        _service = service;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule($"[yellow]{Title}[/]").RuleStyle("grey").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Seleccione una opción:")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "1. Listar todas las disponibilidades",
                        "2. Buscar por ID",
                        "3. Buscar por ID de personal (staff)",
                        "4. Buscar por rango de fechas",
                        "5. Registrar disponibilidad",
                        "6. Actualizar disponibilidad",
                        "7. Eliminar disponibilidad",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0"))
                break;

            switch (option.Split('.')[0])
            {
                case "1": await ListAllAsync(); break;
                case "2": await SearchByIdAsync(); break;
                case "3": await SearchByStaffIdAsync(); break;
                case "4": await SearchByDateRangeAsync(); break;
                case "5": await CreateAsync(); break;
                case "6": await UpdateAsync(); break;
                case "7": await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        ShowTable(items, "Disponibilidad del personal");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] de la disponibilidad:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value);

        if (item is null)
            AnsiConsole.MarkupLine("[red]No se encontró ninguna disponibilidad con ese ID.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para ID: {id.Value}");

        Pause();
    }

    private async Task SearchByStaffIdAsync()
    {
        var staffId = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID del empleado / staff[/]:");
        if (staffId is null)
            return;

        var items = await _service.GetByStaffIdAsync(staffId.Value);
        ShowSearchResults(items, $"Disponibilidades del personal #{staffId.Value}");
    }

    private async Task SearchByDateRangeAsync()
    {
        AnsiConsole.MarkupLine("[dim]Rango de búsqueda (varios formatos de fecha y hora admitidos).[/]");
        var startsAt = PromptDateTime("Fecha y hora [bold]inicial[/]");
        var endsAt = PromptDateTime("Fecha y hora [bold]final[/]");
        var items = await _service.GetByDateRangeAsync(startsAt, endsAt);
        ShowSearchResults(items, $"Disponibilidades entre {startsAt:yyyy-MM-dd HH:mm} y {endsAt:yyyy-MM-dd HH:mm}");
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar disponibilidad[/]");
        var staffId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del empleado (staff):");
        if (staffId is null)
            return;

        var availabilityStatusId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del estado de disponibilidad:");
        if (availabilityStatusId is null)
            return;

        var startsAt = PromptDateTime("Inicio [dim](fecha y hora)[/]");
        var endsAt = PromptDateTime("Fin [dim](fecha y hora)[/]");
        var notes = AskOptionalText("Notas (opcional):");

        if (AnsiConsole.Confirm("¿Desea guardar?"))
        {
            try
            {
                await _service.CreateAsync(staffId.Value, availabilityStatusId.Value, startsAt, endsAt, notes);
                AnsiConsole.MarkupLine("[green]Disponibilidad registrada correctamente.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
            }
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        var item = await PromptAvailabilitySelectionAsync("Seleccione la disponibilidad a modificar:");
        if (item is null)
            return;

        var staffId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Nuevo ID del empleado (staff):", item.StaffId.Value);
        if (staffId is null)
            return;

        var availabilityStatusId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Nuevo ID del estado de disponibilidad:", item.StateId.Value);
        if (availabilityStatusId is null)
            return;

        var startsAt = PromptDateTime("Nuevo inicio [dim](fecha y hora)[/]", item.Dates.StartDate);
        var endsAt = PromptDateTime("Nuevo fin [dim](fecha y hora)[/]", item.Dates.EndDate);
        var notes = AskOptionalText("Nuevas notas:", item.Observation.Value);

        try
        {
            await _service.UpdateAsync(item.Id!.Value, staffId.Value, availabilityStatusId.Value, startsAt, endsAt, notes);
            AnsiConsole.MarkupLine("[green]Disponibilidad actualizada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var subOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Eliminar disponibilidad[/]")
                .AddChoices("Eliminar por ID", "Eliminar por ID de personal (staff)", "Cancelar"));

        if (subOption == "Cancelar")
        {
            Pause();
            return;
        }

        try
        {
            switch (subOption)
            {
                case "Eliminar por ID":
                    var availability = await PromptAvailabilitySelectionAsync("Seleccione la disponibilidad a eliminar:");
                    if (availability is null)
                        break;
                    if (AnsiConsole.Confirm("¿Confirma la eliminación?"))
                        await _service.DeleteByIdAsync(availability.Id!.Value);
                    break;
                case "Eliminar por ID de personal (staff)":
                    var staffId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del empleado:");
                    if (staffId is null)
                        break;
                    if (AnsiConsole.Confirm("¿Eliminar todas las disponibilidades de ese empleado?"))
                    {
                        var affected = await _service.DeleteByStaffIdAsync(staffId.Value);
                        AnsiConsole.MarkupLine($"[green]Registros eliminados: {affected}[/]");
                    }
                    break;
            }

            AnsiConsole.MarkupLine("[green]Operación procesada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private static DateTime PromptDateTime(string label, DateTime? defaultValue = null)
    {
        while (true)
        {
            var prompt = new TextPrompt<string>(
                    $"{label} [dim](p. ej. 2008-07-23 14:30, 23-07-2008 14:30, o solo fecha)[/]:")
                .Validate(s => string.IsNullOrWhiteSpace(s)
                    ? ValidationResult.Error("[red]Este campo es obligatorio.[/]")
                    : ValidationResult.Success());

            if (defaultValue is not null)
                prompt.DefaultValue(defaultValue.Value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));

            var raw = AnsiConsole.Prompt(prompt).Trim().TrimEnd(',').Trim();
            if (TryParseDateTimeFlexible(raw, out var dt))
                return dt;

            AnsiConsole.MarkupLine(
                "[red]No se pudo interpretar la fecha. Use el año con 4 dígitos y, si hay hora, inclúyala (HH:mm).[/]");
        }
    }

    private static bool TryParseDateTimeFlexible(string s, out DateTime dt)
    {
        var formats = new[]
        {
            "yyyy-MM-dd HH:mm",
            "dd-MM-yyyy HH:mm",
            "yyyy-MM-dd H:mm",
            "dd-MM-yyyy H:mm",
            "yyyy-MM-dd",
            "dd-MM-yyyy",
            "dd/MM/yyyy HH:mm",
            "dd/MM/yyyy"
        };

        foreach (var fmt in formats)
        {
            if (DateTime.TryParseExact(s, fmt, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return true;
        }

        if (DateTime.TryParse(s, new CultureInfo("es-CO"), DateTimeStyles.None, out dt))
            return true;

        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            return true;

        dt = default;
        return false;
    }

    private static void ShowSearchResults(IEnumerable<StaffAvailabilityRecord> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
            AnsiConsole.MarkupLine("[red]No se encontraron registros.[/]");
        else
            ShowTable(list, title);

        Pause();
    }

    private static void ShowTable(IEnumerable<StaffAvailabilityRecord> items, string title)
    {
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Staff[/]")
            .AddColumn("[blue]Estado[/]")
            .AddColumn("[green]Inicio[/]")
            .AddColumn("[blue]Fin[/]")
            .AddColumn("[green]Notas[/]");

        foreach (var item in items)
        {
            table.AddRow(
                (item.Id?.Value ?? 0).ToString(),
                item.StaffId.Value.ToString(),
                item.StateId.Value.ToString(),
                item.Dates.StartDate.ToString("yyyy-MM-dd HH:mm"),
                item.Dates.EndDate.ToString("yyyy-MM-dd HH:mm"),
                item.Observation.Value ?? "-");
        }

        AnsiConsole.Write(table);
    }

    private static string? AskOptionalText(string prompt, string? currentValue = null)
    {
        var p = new TextPrompt<string>(prompt).AllowEmpty();
        if (!string.IsNullOrEmpty(currentValue))
            p.DefaultValue(currentValue);
        var text = AnsiConsole.Prompt(p);
        return string.IsNullOrWhiteSpace(text) ? null : text.Trim();
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }

    private async Task<StaffAvailabilityRecord?> PromptAvailabilitySelectionAsync(string title)
    {
        var items = (await _service.GetAllAsync())
            .OrderByDescending(x => x.Dates.StartDate)
            .ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay disponibilidades registradas.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatAvailabilityChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatAvailabilityChoice(x) == selected);
    }

    private static string FormatAvailabilityChoice(StaffAvailabilityRecord item) =>
        $"{item.Id?.Value ?? 0} · Staff {item.StaffId.Value} · {item.Dates.StartDate:yyyy-MM-dd HH:mm}";
}
