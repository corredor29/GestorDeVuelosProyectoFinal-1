using GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.People.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.ui;

public sealed class PersonalMenu : IModuleUI
{
    private readonly IPersonalService _service;
    private readonly IPersonService _people;
    private readonly IPersonalPositionsService _positions;
    private readonly IAirlinesService _airlines;
    private readonly IAirportsService _airports;

    public string Key => "staff";
    public string Title => "🧑‍💼  Gestión de Personal";

    public PersonalMenu(
        IPersonalService service,
        IPersonService people,
        IPersonalPositionsService positions,
        IAirlinesService airlines,
        IAirportsService airports)
    {
        _service = service;
        _people = people;
        _positions = positions;
        _airlines = airlines;
        _airports = airports;
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
                    .PageSize(14)
                    .AddChoices(new[]
                    {
                        "1. Listar todo el personal",
                        "2. Buscar por ID",
                        "3. Buscar por ID de persona",
                        "4. Buscar por Nombre de Persona",
                        "5. Buscar por Cargo",
                        "6. Buscar por Aerolínea",
                        "7. Buscar por Aeropuerto",
                        "8. Buscar por Estado Activo",
                        "9. Registrar nuevo Empleado",
                        "10. Actualizar Empleado",
                        "11. Desactivar Empleado",
                        "12. Reactivar Empleado",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0"))
                break;

            switch (option.Split('.')[0])
            {
                case "1": await ListAllAsync(); break;
                case "2": await SearchByIdAsync(); break;
                case "3": await SearchByPersonIdAsync(); break;
                case "4": await SearchByPersonNameAsync(); break;
                case "5": await SearchByPositionAsync(); break;
                case "6": await SearchByAirlineAsync(); break;
                case "7": await SearchByAirportAsync(); break;
                case "8": await SearchByIsActiveAsync(); break;
                case "9": await CreateAsync(); break;
                case "10": await UpdateAsync(); break;
                case "11": await DeactivateMenuAsync(); break;
                case "12": await ReactivateAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        await ShowTableAsync(items, "Todo el Personal");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] del empleado:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value);

        if (item is null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún empleado con ese ID.[/]");
        else
            await ShowTableAsync(new[] { item }, $"Resultado para ID: {id.Value}");

        Pause();
    }

    private async Task SearchByPersonIdAsync()
    {
        var personId = await PromptPersonSelectionAsync();
        if (personId is null)
            return;

        var item = await _service.GetByPersonIdAsync(personId.Value);

        if (item is null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún empleado con ese ID de persona.[/]");
        else
            await ShowTableAsync(new[] { item }, $"Resultado para ID de persona: {personId.Value}");

        Pause();
    }

    private async Task SearchByPersonNameAsync()
    {
        var personName = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]nombre[/] de la persona:");
        if (personName is null)
            return;

        var items = await _service.GetByPersonNameAsync(personName);
        await ShowSearchResultsAsync(items, $"Personal para nombre: {personName}");
    }

    private async Task SearchByPositionAsync()
    {
        var positionId = await PromptPositionSelectionAsync();
        if (positionId is null)
            return;

        var position = await _positions.GetByIdAsync(positionId.Value);
        if (position is null)
        {
            AnsiConsole.MarkupLine("[red]❌ Cargo no encontrado.[/]");
            Pause();
            return;
        }

        var items = await _service.GetByPositionNameAsync(position.Name.Value);
        await ShowSearchResultsAsync(items, $"Personal con cargo: {position.Name.Value}");
    }

    private async Task SearchByAirlineAsync()
    {
        var airlineId = await PromptAirlineSelectionAsync();
        if (airlineId is null)
            return;

        var airline = await _airlines.GetByIdAsync(airlineId.Value);
        if (airline is null)
        {
            AnsiConsole.MarkupLine("[red]❌ Aerolínea no encontrada.[/]");
            Pause();
            return;
        }

        var items = await _service.GetByAirlineNameAsync(airline.Name.Value);
        await ShowSearchResultsAsync(items, $"Personal en aerolínea: {airline.Name.Value}");
    }

    private async Task SearchByAirportAsync()
    {
        var airportId = await PromptAirportSelectionAsync();
        if (airportId is null)
            return;

        var airport = await _airports.GetByIdAsync(airportId.Value);
        if (airport is null)
        {
            AnsiConsole.MarkupLine("[red]❌ Aeropuerto no encontrado.[/]");
            Pause();
            return;
        }

        var items = await _service.GetByAirportNameAsync(airport.Name.Value);
        await ShowSearchResultsAsync(items, $"Personal en aeropuerto: {airport.Name.Value}");
    }

    private async Task SearchByIsActiveAsync()
    {
        var isActive = AnsiConsole.Confirm("¿Buscar empleados activos?", true);
        var items = await _service.GetByIsActiveAsync(isActive);
        await ShowSearchResultsAsync(items, isActive ? "Personal Activo" : "Personal Inactivo");
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar Nuevo Empleado[/]");
        var personId = await PromptPersonSelectionAsync();
        if (personId is null)
        {
            Pause();
            return;
        }

        var positionId = await PromptPositionSelectionAsync();
        if (positionId is null)
        {
            Pause();
            return;
        }

        var airlineId = await PromptOptionalAirlineSelectionAsync();
        if (airlineId.WentBack)
        {
            Pause();
            return;
        }

        var airportId = await PromptOptionalAirportSelectionAsync();
        if (airportId.WentBack)
        {
            Pause();
            return;
        }

        var hireDate = ConsoleMenuHelpers.PromptDateTimeOrBack("Hire Date (yyyy-MM-dd):", "yyyy-MM-dd");
        if (hireDate is null)
        {
            Pause();
            return;
        }

        var isActive = AnsiConsole.Confirm("¿Registrar como activo?", true);

        if (AnsiConsole.Confirm("¿Desea guardar los cambios?"))
        {
            try
            {
                await _service.CreateAsync(personId.Value, positionId.Value, airlineId.Value, airportId.Value, hireDate.Value, isActive);
                AnsiConsole.MarkupLine("[green]✅ Empleado registrado correctamente.[/]");
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
        var staff = (await _service.GetAllAsync()).OrderBy(x => x.PersonId.Value).ToList();
        if (staff.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay empleados registrados.[/]");
            Pause();
            return;
        }

        var selected = await PromptStaffSelectionAsync(staff, "[yellow]Seleccione el empleado a modificar:[/]");
        if (selected is null)
            return;

        var item = selected;
        var id = item.Id!.Value;

        var personId = await PromptPersonSelectionAsync(item.PersonId.Value);
        if (personId is null)
        {
            Pause();
            return;
        }

        var positionId = await PromptPositionSelectionAsync(item.PositionId.Value);
        if (positionId is null)
        {
            Pause();
            return;
        }

        var airlineId = await PromptOptionalAirlineSelectionAsync(item.AirlineId?.Value);
        if (airlineId.WentBack)
        {
            Pause();
            return;
        }

        var airportId = await PromptOptionalAirportSelectionAsync(item.AirportId?.Value);
        if (airportId.WentBack)
        {
            Pause();
            return;
        }

        var hireDate = ConsoleMenuHelpers.PromptDateTimeOrBack("Nueva Hire Date:", "yyyy-MM-dd");
        if (hireDate is null)
        {
            Pause();
            return;
        }

        var isActive = AnsiConsole.Confirm("¿Empleado activo?", item.IsActive.Value);

        try
        {
            await _service.UpdateAsync(id, personId.Value, positionId.Value, airlineId.Value, airportId.Value, hireDate.Value, isActive);
            AnsiConsole.MarkupLine("[green]✅ Empleado actualizado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task DeactivateMenuAsync()
    {
        var staff = (await _service.GetAllAsync()).Where(x => x.IsActive.Value).OrderBy(x => x.PersonId.Value).ToList();
        if (staff.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay empleados activos para desactivar.[/]");
            Pause();
            return;
        }

        var item = await PromptStaffSelectionAsync(staff, "[red]Seleccione el empleado a desactivar:[/]");
        if (item is null)
            return;

        try
        {
            if (AnsiConsole.Confirm("¿Desea desactivar el empleado?"))
            {
                await _service.DeactivateByIdAsync(item.Id!.Value);
                AnsiConsole.MarkupLine("[green]✅ Operación procesada.[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task ReactivateAsync()
    {
        var staff = (await _service.GetAllAsync()).Where(x => !x.IsActive.Value).OrderBy(x => x.PersonId.Value).ToList();
        if (staff.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay empleados inactivos para reactivar.[/]");
            Pause();
            return;
        }

        var item = await PromptStaffSelectionAsync(staff, "[yellow]Seleccione el empleado a reactivar:[/]");
        if (item is null)
            return;

        try
        {
            await _service.ReactivateAsync(item.Id!.Value);
            AnsiConsole.MarkupLine("[green]✅ Empleado reactivado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task ShowSearchResultsAsync(IEnumerable<Staff> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
            AnsiConsole.MarkupLine("[red]❌ No se encontraron registros.[/]");
        else
            await ShowTableAsync(list, title);

        Pause();
    }

    private async Task ShowTableAsync(IEnumerable<Staff> items, string title)
    {
        var list = items.ToList();

        var people = (await _people.GetAllAsync()).ToDictionary(p => p.Id.Value, p => $"{p.FirstName.Value} {p.LastNames.Value}");
        var positions = (await _positions.GetAllAsync())
            .Where(p => p.Id != null)
            .ToDictionary(p => p.Id!.Value, p => p.Name.Value);
        var airlines = (await _airlines.GetAllAsync())
            .Where(a => a.Id != null)
            .ToDictionary(a => a.Id!.Value, a => a.Name.Value);
        var airports = (await _airports.GetAllAsync())
            .Where(a => a.Id != null)
            .ToDictionary(a => a.Id!.Value, a => a.Name.Value);

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Persona[/]")
            .AddColumn("[blue]Cargo[/]")
            .AddColumn("[green]Aerolínea[/]")
            .AddColumn("[blue]Aeropuerto[/]")
            .AddColumn("[green]Contratación[/]")
            .AddColumn("[blue]Activo[/]");

        foreach (var item in list)
        {
            var personLabel = people.TryGetValue(item.PersonId.Value, out var pn)
                ? $"{item.PersonId.Value} · {Markup.Escape(pn)}"
                : item.PersonId.Value.ToString();
            var posLabel = positions.TryGetValue(item.PositionId.Value, out var pos)
                ? $"{item.PositionId.Value} · {Markup.Escape(pos)}"
                : item.PositionId.Value.ToString();
            var airlineLabel = item.AirlineId is null
                ? "-"
                : airlines.TryGetValue(item.AirlineId.Value, out var al)
                    ? $"{item.AirlineId.Value} · {Markup.Escape(al)}"
                    : item.AirlineId.Value.ToString();
            var airportLabel = item.AirportId is null
                ? "-"
                : airports.TryGetValue(item.AirportId.Value, out var ap)
                    ? $"{item.AirportId.Value} · {Markup.Escape(ap)}"
                    : item.AirportId.Value.ToString();

            table.AddRow(
                (item.Id?.Value ?? 0).ToString(),
                personLabel,
                posLabel,
                airlineLabel,
                airportLabel,
                item.HireDate.Value.ToString("yyyy-MM-dd"),
                item.IsActive.Value ? "Sí" : "No");
        }

        AnsiConsole.Write(table);
    }

    private async Task<int?> PromptPersonSelectionAsync(int? currentPersonId = null)
    {
        var people = (await _people.GetAllAsync()).OrderBy(p => p.LastNames.Value).ThenBy(p => p.FirstName.Value).ToList();
        if (people.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay personas registradas.[/]");
            return null;
        }

        var title = currentPersonId is null
            ? "[yellow]Seleccione la persona:[/]"
            : $"[yellow]Seleccione la persona[/] [dim](actual id: {currentPersonId})[/]:";

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(people.Select(p => $"{p.Id.Value} · {Markup.Escape(p.FirstName.Value)} {Markup.Escape(p.LastNames.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return people.First(p => $"{p.Id.Value} · {Markup.Escape(p.FirstName.Value)} {Markup.Escape(p.LastNames.Value)}" == selected).Id.Value;
    }

    private async Task<int?> PromptPositionSelectionAsync(int? currentPositionId = null)
    {
        var positions = (await _positions.GetAllAsync()).Where(p => p.Id is not null).OrderBy(p => p.Name.Value).ToList();
        if (positions.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay cargos registrados.[/]");
            return null;
        }

        var title = currentPositionId is null
            ? "[yellow]Seleccione el cargo:[/]"
            : $"[yellow]Seleccione el cargo[/] [dim](actual id: {currentPositionId})[/]:";

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(positions.Select(p => $"{p.Id!.Value} · {Markup.Escape(p.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return positions.First(p => $"{p.Id!.Value} · {Markup.Escape(p.Name.Value)}" == selected).Id!.Value;
    }

    private async Task<int?> PromptAirlineSelectionAsync(int? currentAirlineId = null)
    {
        var airlines = (await _airlines.GetAllAsync()).Where(a => a.Id is not null).OrderBy(a => a.Name.Value).ToList();
        if (airlines.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay aerolíneas registradas.[/]");
            return null;
        }

        var title = currentAirlineId is null
            ? "[yellow]Seleccione la aerolínea:[/]"
            : $"[yellow]Seleccione la aerolínea[/] [dim](actual id: {currentAirlineId})[/]:";

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(airlines.Select(a => $"{a.Id!.Value} · {Markup.Escape(a.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return airlines.First(a => $"{a.Id!.Value} · {Markup.Escape(a.Name.Value)}" == selected).Id!.Value;
    }

    private async Task<int?> PromptAirportSelectionAsync(int? currentAirportId = null)
    {
        var airports = (await _airports.GetAllAsync()).Where(a => a.Id is not null).OrderBy(a => a.Name.Value).ToList();
        if (airports.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay aeropuertos registrados.[/]");
            return null;
        }

        var title = currentAirportId is null
            ? "[yellow]Seleccione el aeropuerto:[/]"
            : $"[yellow]Seleccione el aeropuerto[/] [dim](actual id: {currentAirportId})[/]:";

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(airports.Select(a => $"{a.Id!.Value} · {Markup.Escape(a.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return airports.First(a => $"{a.Id!.Value} · {Markup.Escape(a.Name.Value)}" == selected).Id!.Value;
    }

    private async Task<(bool WentBack, int? Value)> PromptOptionalAirlineSelectionAsync(int? currentAirlineId = null)
    {
        var airlines = (await _airlines.GetAllAsync()).Where(a => a.Id is not null).OrderBy(a => a.Name.Value).ToList();
        var choices = airlines.Select(a => $"{a.Id!.Value} · {Markup.Escape(a.Name.Value)}").ToList();
        choices.Add("Sin aerolínea");
        choices.Add(ConsoleMenuHelpers.VolverAlMenu);

        var title = currentAirlineId is null
            ? "[yellow]Seleccione la aerolínea[/] [dim](opcional)[/]:"
            : $"[yellow]Seleccione la aerolínea[/] [dim](actual id: {currentAirlineId}, opcional)[/]:";

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(choices));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return (true, null);

        if (selected == "Sin aerolínea")
            return (false, null);

        return (false, airlines.First(a => $"{a.Id!.Value} · {Markup.Escape(a.Name.Value)}" == selected).Id!.Value);
    }

    private async Task<(bool WentBack, int? Value)> PromptOptionalAirportSelectionAsync(int? currentAirportId = null)
    {
        var airports = (await _airports.GetAllAsync()).Where(a => a.Id is not null).OrderBy(a => a.Name.Value).ToList();
        var choices = airports.Select(a => $"{a.Id!.Value} · {Markup.Escape(a.Name.Value)}").ToList();
        choices.Add("Sin aeropuerto");
        choices.Add(ConsoleMenuHelpers.VolverAlMenu);

        var title = currentAirportId is null
            ? "[yellow]Seleccione el aeropuerto[/] [dim](opcional)[/]:"
            : $"[yellow]Seleccione el aeropuerto[/] [dim](actual id: {currentAirportId}, opcional)[/]:";

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(choices));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return (true, null);

        if (selected == "Sin aeropuerto")
            return (false, null);

        return (false, airports.First(a => $"{a.Id!.Value} · {Markup.Escape(a.Name.Value)}" == selected).Id!.Value);
    }

    private async Task<Staff?> PromptStaffSelectionAsync(IReadOnlyCollection<Staff> staff, string title)
    {
        var people = (await _people.GetAllAsync()).ToDictionary(p => p.Id.Value, p => $"{p.FirstName.Value} {p.LastNames.Value}");
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(staff.Select(s => FormatStaffChoice(s, people)).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return staff.First(s => FormatStaffChoice(s, people) == selected);
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }

    private static string FormatStaffChoice(Staff item, IReadOnlyDictionary<int, string> people)
    {
        var personName = people.TryGetValue(item.PersonId.Value, out var name)
            ? name
            : $"Persona {item.PersonId.Value}";

        return $"{item.Id?.Value ?? 0} · {Markup.Escape(personName)}";
    }
}
