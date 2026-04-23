using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Application.Interfaces;
using PassengerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate.Passengers;
using GestorDeVuelosProyectoFinal.Moduls.People.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.ui;

public class PassengersMenu
{
    private readonly IPassengerService _service;
    private readonly IPersonService _people;
    private readonly IPassengerTypesService _types;

    public PassengersMenu(IPassengerService service, IPersonService people, IPassengerTypesService types)
    {
        _service = service;
        _people = people;
        _types = types;
    }

    public async Task Show()
    {
        var exit = false;
        while (!exit)
        {
            AnsiConsole.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]GESTIÓN DE PASAJEROS[/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Listar Pasajeros",
                        "Registrar Pasajero",
                        "Eliminar Pasajero",
                        "Buscar por ID",
                        "Volver al Menú Principal"
                    }));

            switch (option)
            {
                case "Listar Pasajeros":
                    await ListPassengers();
                    break;
                case "Registrar Pasajero":
                    await CreatePassenger();
                    break;
                case "Eliminar Pasajero":
                    await DeletePassenger();
                    break;
                case "Buscar por ID":
                    await SearchById();
                    break;
                case "Volver al Menú Principal":
                    exit = true;
                    break;
            }
        }
    }

    private async Task ListPassengers()
    {
        var passengers = await _service.GetAllPassengers();
        var list = passengers.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[grey]No hay pasajeros registrados.[/]");
            AnsiConsole.MarkupLine("\n[grey]Presione cualquier tecla para continuar...[/]");
            Console.ReadKey();
            return;
        }

        var peopleNames = (await _people.GetAllAsync())
            .Where(p => p.Id is not null)
            .ToDictionary(
                p => p.Id!.Value,
                p => $"{p.FirstName.Value} {p.LastNames.Value}".Trim());

        var typeNames = (await _types.GetAllAsync())
            .Where(t => t.Id is not null)
            .ToDictionary(t => t.Id!.Value, t => t.Name.Value);

        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[blue]ID Pasajero[/]");
        table.AddColumn("[blue]Persona[/]");
        table.AddColumn("[blue]Tipo de pasajero[/]");

        foreach (var p in list)
        {
            var personLabel = peopleNames.TryGetValue(p.PersonId.Value, out var pn)
                ? $"{p.PersonId.Value} · {Markup.Escape(pn)}"
                : p.PersonId.Value.ToString();

            var typeLabel = typeNames.TryGetValue(p.PassengerTypeId.Value, out var tn)
                ? $"{p.PassengerTypeId.Value} · {Markup.Escape(tn)}"
                : p.PassengerTypeId.Value.ToString();

            table.AddRow(p.Id.Value.ToString(), personLabel, typeLabel);
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[grey]Presione cualquier tecla para continuar...[/]");
        Console.ReadKey();
    }

    private async Task CreatePassenger()
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar pasajero"))
            return;

        var people = (await _people.GetAllAsync()).Where(p => p.Id is not null).OrderBy(p => p.Id!.Value).ToList();
        if (people.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay personas para asignar. Registre una persona primero.[/]");
            Thread.Sleep(1500);
            return;
        }

        var types = (await _types.GetAllAsync()).Where(t => t.Id is not null).OrderBy(t => t.Id!.Value).ToList();
        if (types.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay tipos de pasajero en catálogo.[/]");
            Thread.Sleep(1500);
            return;
        }

        var personChoices = people
            .Select(p => $"{p.Id!.Value} · {Markup.Escape($"{p.FirstName.Value} {p.LastNames.Value}".Trim())}")
            .ToList();
        personChoices.Add(ConsoleMenuHelpers.VolverSinGuardar);

        var pickedPerson = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione la persona")
                .PageSize(15)
                .AddChoices(personChoices));
        if (pickedPerson == ConsoleMenuHelpers.VolverSinGuardar)
            return;

        var personIdPart = pickedPerson.Split('·')[0].Trim();
        if (!int.TryParse(personIdPart, out var personId))
        {
            AnsiConsole.MarkupLine("[red]Selección inválida.[/]");
            Thread.Sleep(1200);
            return;
        }

        var typeChoices = types
            .Select(t => $"{t.Id!.Value} · {Markup.Escape(t.Name.Value)}")
            .ToList();
        typeChoices.Add(ConsoleMenuHelpers.VolverSinGuardar);

        var pickedType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione el tipo de pasajero")
                .PageSize(15)
                .AddChoices(typeChoices));
        if (pickedType == ConsoleMenuHelpers.VolverSinGuardar)
            return;

        var typeIdPart = pickedType.Split('·')[0].Trim();
        if (!int.TryParse(typeIdPart, out var typeId))
        {
            AnsiConsole.MarkupLine("[red]Selección inválida.[/]");
            Thread.Sleep(1200);
            return;
        }

        try
        {
            await _service.CreatePassenger(personId, typeId);
            AnsiConsole.MarkupLine("[bold green]¡Pasajero registrado correctamente![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {ex.Message}");
        }
        Thread.Sleep(2000);
    }

    private async Task DeletePassenger()
    {
        var passengers = (await _service.GetAllPassengers()).ToList();
        if (passengers.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay pasajeros registrados.[/]");
            Thread.Sleep(1200);
            return;
        }

        var peopleNames = (await _people.GetAllAsync())
            .Where(p => p.Id is not null)
            .ToDictionary(
                p => p.Id!.Value,
                p => $"{p.FirstName.Value} {p.LastNames.Value}".Trim());

        var typeNames = (await _types.GetAllAsync())
            .Where(t => t.Id is not null)
            .ToDictionary(t => t.Id!.Value, t => t.Name.Value);

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione el pasajero a eliminar:")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(passengers.Select(p => FormatPassengerChoice(p, peopleNames, typeNames)).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var passenger = passengers.First(p => FormatPassengerChoice(p, peopleNames, typeNames) == selected);

        await _service.DeletePassenger(passenger.Id.Value);
        AnsiConsole.MarkupLine("[bold yellow]Proceso de eliminación completado.[/]");
        Thread.Sleep(1500);
    }

    private static string FormatPassengerChoice(PassengerAggregate passenger, IDictionary<int, string> peopleNames, IDictionary<int, string> typeNames)
    {
        var personLabel = peopleNames.TryGetValue(passenger.PersonId.Value, out string? personName)
            ? personName
            : $"Persona {passenger.PersonId.Value}";
        var typeLabel = typeNames.TryGetValue(passenger.PassengerTypeId.Value, out string? typeName)
            ? typeName
            : $"Tipo {passenger.PassengerTypeId.Value}";

        return $"{passenger.Id.Value} · {Markup.Escape(personLabel)} · {Markup.Escape(typeLabel)}";
    }

    private async Task SearchById()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [blue]ID[/] a buscar:");
        if (id is null)
            return;

        var p = await _service.GetPassengerById(id.Value);

        if (p == null)
        {
            AnsiConsole.MarkupLine("[red]Pasajero no encontrado.[/]");
        }
        else
        {
            var person = await _people.GetByIdAsync(p.PersonId.Value);
            var personLabel = person is null
                ? p.PersonId.Value.ToString()
                : $"{p.PersonId.Value} · {person.FirstName.Value} {person.LastNames.Value}".Trim();

            var type = await _types.GetByIdAsync(p.PassengerTypeId.Value);
            var typeLabel = type is null
                ? p.PassengerTypeId.Value.ToString()
                : $"{p.PassengerTypeId.Value} · {type.Name.Value}";

            AnsiConsole.MarkupLine($"[green]ID:[/] {p.Id.Value}");
            AnsiConsole.MarkupLine($"[green]Persona:[/] {Markup.Escape(personLabel)}");
            AnsiConsole.MarkupLine($"[green]Tipo de pasajero:[/] {Markup.Escape(typeLabel)}");
        }
        AnsiConsole.MarkupLine("\n[grey]Presione cualquier tecla para continuar...[/]");
        Console.ReadKey();
    }
}
