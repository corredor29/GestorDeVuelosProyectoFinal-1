using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.People.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.ui;

// Este menú maneja los teléfonos de personas.
// Intenta apoyarse bastante en listas para evitar que la persona tenga que recordar IDs.
public sealed class PeoplePhonesMenu : IModuleUI
{
    private readonly IPeoplePhonesService _service;
    private readonly IPhoneCodesService _phoneCodes;
    private readonly IPersonService _people;

    public string Key => "people_phones";
    public string Title => "☎️  Gestión de Teléfonos de Personas";

    public PeoplePhonesMenu(IPeoplePhonesService service, IPhoneCodesService phoneCodes, IPersonService people)
    {
        _service = service;
        _phoneCodes = phoneCodes;
        _people = people;
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
                    .PageSize(12)
                    .AddChoices(new[]
                    {
                        "1. Listar todos los teléfonos",
                        "2. Buscar por ID",
                        "3. Buscar por ID de persona",
                        "4. Buscar por número de teléfono",
                        "5. Buscar por ID de código telefónico",
                        "6. Buscar por Nombre de Persona",
                        "7. Registrar nuevo Teléfono",
                        "8. Actualizar Teléfono",
                        "9. Eliminar Teléfono",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0")) break;

            switch (option[0])
            {
                case '1': await ListAllAsync(); break;
                case '2': await SearchByIdAsync(); break;
                case '3': await SearchByPersonIdAsync(); break;
                case '4': await SearchByPhoneNumberAsync(); break;
                case '5': await SearchByPhoneCodeIdAsync(); break;
                case '6': await SearchByPersonNameAsync(); break;
                case '7': await CreateAsync(); break;
                case '8': await UpdateAsync(); break;
                case '9': await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        await ShowTableAsync(items, "Todos los Teléfonos de Personas");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var phones = (await _service.GetAllAsync()).OrderBy(x => x.PersonId.Value).ThenBy(x => x.PhoneNumber.Value).ToList();
        var item = await PromptPhoneSelectionAsync(phones, "[yellow]Seleccione el teléfono a consultar:[/]");
        if (item is null)
            return;

        await ShowTableAsync(new[] { item }, $"Resultado para ID: {item.Id.Value}");

        Pause();
    }

    private async Task SearchByPersonIdAsync()
    {
        var personId = await PromptPersonIdAsync();
        if (personId is null)
            return;

        var items = await _service.GetByPersonIdAsync(personId.Value);
        await ShowSearchResultsAsync(items, $"Teléfonos de la persona #{personId.Value}");
    }

    private async Task SearchByPhoneNumberAsync()
    {
        var phoneNumber = PromptPhoneNumber("Ingrese el [green]número de teléfono[/]:");
        if (phoneNumber is null)
            return;

        var items = await _service.GetByPhoneNumberAsync(phoneNumber);
        await ShowSearchResultsAsync(items, $"Resultado para número: {phoneNumber}");
    }

    private async Task SearchByPhoneCodeIdAsync()
    {
        var phoneCodeId = await PromptPhoneCodeSelectionAsync();
        if (phoneCodeId is null)
            return;

        var items = await _service.GetByPhoneCodeIdAsync(phoneCodeId.Value);
        await ShowSearchResultsAsync(items, $"Resultado para ID de código telefónico: {phoneCodeId.Value}");
    }

    private async Task SearchByPersonNameAsync()
    {
        var personName = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre de la persona:");
        if (personName is null)
            return;

        var items = await _service.GetByPersonNameAsync(personName);
        await ShowSearchResultsAsync(items, $"Resultado para nombre: {personName}");
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar Nuevo Teléfono de Persona[/]");
        // Primero elegimos persona y código telefónico, y luego capturamos el número local.
        var personId = await PromptPersonIdAsync();
        if (personId is null)
        {
            Pause();
            return;
        }

        var phoneCodeId = await PromptPhoneCodeSelectionAsync();
        if (phoneCodeId is null)
        {
            Pause();
            return;
        }

        var phoneNumber = PromptPhoneNumber("Número de teléfono:");
        if (phoneNumber is null)
        {
            Pause();
            return;
        }
        var isPrimary = AnsiConsole.Confirm("¿Es teléfono principal?");

        if (AnsiConsole.Confirm("¿Desea guardar los cambios?"))
        {
            try
            {
                await _service.CreateAsync(personId.Value, phoneCodeId.Value, phoneNumber, isPrimary);
                AnsiConsole.MarkupLine("[green]✅ Teléfono registrado correctamente.[/]");
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
        var phones = (await _service.GetAllAsync()).OrderBy(x => x.PersonId.Value).ThenBy(x => x.PhoneNumber.Value).ToList();
        if (phones.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay teléfonos registrados.[/]");
            Pause();
            return;
        }

        var selected = await PromptPhoneSelectionAsync(phones, "[yellow]Seleccione el teléfono a modificar:[/]");
        if (selected is null)
            return;

        // En actualización permitimos cambiar persona, código y número dentro del mismo flujo.
        var item = selected;
        var id = item.Id.Value;

        var personId = await PromptPersonIdAsync(item.PersonId.Value);
        if (personId is null)
        {
            Pause();
            return;
        }

        var phoneCodeId = await PromptPhoneCodeIdForUpdateAsync(item.PhoneCodeId.Value);
        if (phoneCodeId is null)
        {
            Pause();
            return;
        }
        var phoneNumber = PromptPhoneNumber("Nuevo número de teléfono:", item.PhoneNumber.Value);
        if (phoneNumber is null)
        {
            Pause();
            return;
        }
        var isPrimary = AnsiConsole.Confirm("¿Es teléfono principal?", item.IsPrimary.Value);

        try
        {
            await _service.UpdateAsync(id, personId.Value, phoneCodeId.Value, phoneNumber, isPrimary);
            AnsiConsole.MarkupLine("[green]✅ Teléfono actualizado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var phones = (await _service.GetAllAsync()).OrderBy(x => x.PersonId.Value).ThenBy(x => x.PhoneNumber.Value).ToList();
        if (phones.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay teléfonos para eliminar.[/]");
            Pause();
            return;
        }

        var item = await PromptPhoneSelectionAsync(phones, "[red]Seleccione el teléfono a eliminar:[/]");
        if (item is null)
            return;

        try
        {
            if (!AnsiConsole.Confirm($"[red]¿Está seguro de eliminar el teléfono {Markup.Escape(item.PhoneNumber.Value)}? Esta acción es irreversible.[/]"))
            {
                Pause();
                return;
            }

            await _service.DeleteByIdAsync(item.Id.Value);

            AnsiConsole.MarkupLine("[green]✅ Operación procesada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task<int?> PromptPhoneCodeIdForUpdateAsync(int currentPhoneCodeId)
    {
        var current = await _phoneCodes.GetByIdAsync(currentPhoneCodeId);
        var currentLabel = current is null
            ? currentPhoneCodeId.ToString()
            : $"{Markup.Escape(current.CountryName.Value)} ({Markup.Escape(current.Code.Value)})";

        while (true)
        {
            // Damos varias formas de elegir el código porque a veces la persona recuerda
            // el país, otras el prefijo y otras solo quiere ver la lista completa.
            var mode = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Código telefónico [dim](actual: {currentLabel})[/]")
                    .AddChoices(
                        "Mantener el código actual",
                        "Buscar por nombre del país",
                        "Buscar por código telefónico del país",
                        "Elegir de la lista completa",
                        ConsoleMenuHelpers.VolverAlMenu));

            if (mode == "Mantener el código actual")
                return currentPhoneCodeId;
            if (mode == ConsoleMenuHelpers.VolverAlMenu)
                return null;

            if (mode == "Elegir de la lista completa")
            {
                var all = (await _phoneCodes.GetAllAsync()).OrderBy(p => p.CountryName.Value, StringComparer.OrdinalIgnoreCase).ToList();
                if (all.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]No hay códigos telefónicos registrados.[/]");
                    continue;
                }

                var labels = all.Select(FormatPhoneCodeChoice).ToList();
                var sel = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Seleccione el país:")
                        .PageSize(20)
                        .AddChoices(labels));
                return all.First(p => FormatPhoneCodeChoice(p) == sel).Id.Value;
            }

            if (mode == "Buscar por nombre del país")
            {
                var name = AnsiConsole.Prompt(
                    new TextPrompt<string>("Nombre del país [dim](coincidencia parcial)[/]:")
                        .Validate(s => string.IsNullOrWhiteSpace(s)
                            ? ValidationResult.Error("[red]Indique un nombre.[/]")
                            : ValidationResult.Success()));

                var needle = name.Trim();
                var candidates = (await _phoneCodes.GetAllAsync())
                    .Where(p => p.CountryName.Value.Contains(needle, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(p => p.CountryName.Value, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (candidates.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]No hay países que coincidan. Intente de nuevo o use la lista.[/]");
                    continue;
                }

                if (candidates.Count == 1)
                {
                    var only = candidates[0];
                    AnsiConsole.MarkupLine($"[green]Seleccionado:[/] {Markup.Escape(only.CountryName.Value)} — {Markup.Escape(only.Code.Value)}");
                    return only.Id.Value;
                }

                var labels = candidates.Select(FormatPhoneCodeChoice).ToList();
                var sel = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Varios países coinciden. Elija uno:")
                        .PageSize(15)
                        .AddChoices(labels));
                var picked = candidates.First(p => FormatPhoneCodeChoice(p) == sel);
                return picked.Id.Value;
            }

            var code = AnsiConsole.Prompt(
                new TextPrompt<string>("Código del país [dim](ej. +57, 57, CO según catálogo)[/]:")
                    .Validate(s => string.IsNullOrWhiteSpace(s)
                        ? ValidationResult.Error("[red]Indique un código.[/]")
                        : ValidationResult.Success()));

            var byCode = await _phoneCodes.GetByCountryCodeAsync(code.Trim());
            if (byCode is null)
            {
                AnsiConsole.MarkupLine("[red]No se encontró un código telefónico con ese valor.[/]");
                continue;
            }

            AnsiConsole.MarkupLine($"[green]Seleccionado:[/] {Markup.Escape(byCode.CountryName.Value)} — {Markup.Escape(byCode.Code.Value)}");
            return byCode.Id.Value;
        }
    }

    private static string FormatPhoneCodeChoice(PhoneCode p) =>
        $"{Markup.Escape(p.CountryName.Value)} — {Markup.Escape(p.Code.Value)} (id {p.Id.Value})";

    private static string? PromptPhoneNumber(string label, string? defaultValue = null)
    {
        return ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            label,
            defaultValue ?? string.Empty,
            allowEmpty: false,
            validate: value =>
            {
                var text = value?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return "El número telefónico es obligatorio.";
                if (!text.All(char.IsDigit))
                    return "El número telefónico solo puede contener números.";
                if (text.Length > 20)
                    return "El número telefónico no puede superar los 20 dígitos.";
                return null;
            })?.Trim();
    }

    private async Task ShowSearchResultsAsync(IEnumerable<PersonPhone> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
            AnsiConsole.MarkupLine("[red]❌ No se encontraron registros.[/]");
        else
            await ShowTableAsync(list, title);

        Pause();
    }

    private async Task ShowTableAsync(IEnumerable<PersonPhone> items, string title)
    {
        var peopleMap = (await _people.GetAllAsync())
            .Where(p => p.Id is not null)
            .ToDictionary(
                p => p.Id.Value,
                p => $"{p.FirstName.Value} {p.LastNames.Value}");

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Persona[/]")
            .AddColumn("[blue]Código telefónico ID[/]")
            .AddColumn("[green]Número de teléfono[/]")
            .AddColumn("[blue]Principal[/]");

        // Mostramos persona con ID y nombre para que el listado sea más útil al admin.
        foreach (var item in items)
        {
            var personLabel = peopleMap.TryGetValue(item.PersonId.Value, out var name)
                ? $"{item.PersonId.Value} · {name}"
                : item.PersonId.Value.ToString();

            table.AddRow(
                item.Id.Value.ToString(),
                Markup.Escape(personLabel),
                item.PhoneCodeId.Value.ToString(),
                item.PhoneNumber.Value,
                item.IsPrimary.Value ? "Sí" : "No");
        }

        AnsiConsole.Write(table);
    }

    private async Task<int?> PromptPersonIdAsync(int? currentPersonId = null)
    {
        var people = (await _people.GetAllAsync())
            .Where(p => p.Id is not null)
            .OrderBy(p => p.LastNames.Value)
            .ThenBy(p => p.FirstName.Value)
            .ToList();

        if (people.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay personas registradas.[/]");
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
                .AddChoices(people.Select(FormatPersonChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return people.First(p => FormatPersonChoice(p) == selected).Id.Value;
    }

    private async Task<int?> PromptPhoneCodeSelectionAsync()
    {
        var codes = (await _phoneCodes.GetAllAsync())
            .OrderBy(p => p.CountryName.Value, StringComparer.OrdinalIgnoreCase)
            .ThenBy(p => p.Code.Value)
            .ToList();

        if (codes.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay códigos telefónicos registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el código telefónico:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(codes.Select(FormatPhoneCodeChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return codes.First(p => FormatPhoneCodeChoice(p) == selected).Id.Value;
    }

    private async Task<PersonPhone?> PromptPhoneSelectionAsync(IReadOnlyCollection<PersonPhone> phones, string title)
    {
        var peopleMap = (await _people.GetAllAsync())
            .Where(p => p.Id is not null)
            .ToDictionary(
                p => p.Id.Value,
                p => $"{p.FirstName.Value} {p.LastNames.Value}");

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(phones.Select(p => FormatPhoneChoice(p, peopleMap)).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return phones.First(p => FormatPhoneChoice(p, peopleMap) == selected);
    }

    private static string FormatPersonChoice(Person person) =>
        $"{person.Id.Value} · {Markup.Escape(person.FirstName.Value)} {Markup.Escape(person.LastNames.Value)}";

    private static string FormatPhoneChoice(PersonPhone item, IReadOnlyDictionary<int, string> peopleMap)
    {
        var personName = peopleMap.TryGetValue(item.PersonId.Value, out var name)
            ? name
            : $"Persona {item.PersonId.Value}";

        return $"{item.Id.Value} · {Markup.Escape(personName)} · {Markup.Escape(item.PhoneNumber.Value)}";
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }
}
