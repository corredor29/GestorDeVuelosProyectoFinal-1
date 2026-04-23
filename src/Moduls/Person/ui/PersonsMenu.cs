using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.People.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.People.ui;

// Este menú administra el CRUD de personas desde consola.
// Además resuelve varias ayudas visuales para elegir personas y tipos de documento sin pedir IDs a ciegas.
public sealed class PersonsMenu : IModuleUI
{
    private readonly IPersonService _service;
    private readonly IDocumentTypesService _documentTypes;

    public string Key => "persons";
    public string Title => "👤  Gestión de Personas";

    public PersonsMenu(IPersonService service, IDocumentTypesService documentTypes)
    {
        _service = service;
        _documentTypes = documentTypes;
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
                        "1. Listar todas las personas",
                        "2. Buscar persona por ID",
                        "3. Buscar personas por Nombre",
                        "4. Buscar personas por Apellido",
                        "5. Buscar personas por Número de Documento",
                        "6. Registrar nueva Persona",
                        "7. Actualizar Persona",
                        "8. Eliminar Persona",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0")) break;

            // Usamos el primer carácter para mantener el menú simple y fácil de leer.
            switch (option[0])
            {
                case '1': await ListAllAsync(); break;
                case '2': await SearchByIdAsync(); break;
                case '3': await SearchByFirstNameAsync(); break;
                case '4': await SearchByLastNameAsync(); break;
                case '5': await SearchByDocumentNumberAsync(); break;
                case '6': await CreateAsync(); break;
                case '7': await UpdateAsync(); break;
                case '8': await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var people = await _service.GetAllAsync();
        await ShowTableAsync(people, "Todas las Personas");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var people = await _service.GetAllAsync();
        var person = await PromptPersonSelectionAsync(people, "[yellow]Seleccione la persona a consultar:[/]");
        if (person is null)
            return;

        await ShowTableAsync(new[] { person }, $"Resultado para ID: {person.Id.Value}");

        Pause();
    }

    private async Task SearchByFirstNameAsync()
    {
        var firstName = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre de la persona:");
        if (firstName is null)
            return;

        var people = await _service.GetByFirstNameAsync(firstName);
        await ShowSearchResultsAsync(people, $"Resultado para nombre: {firstName}");
    }

    private async Task SearchByLastNameAsync()
    {
        var lastName = ConsoleMenuHelpers.PromptRequiredStringOrBack("Apellido de la persona:");
        if (lastName is null)
            return;

        var people = await _service.GetByLastNameAsync(lastName);
        await ShowSearchResultsAsync(people, $"Resultado para apellido: {lastName}");
    }

    private async Task SearchByDocumentNumberAsync()
    {
        var documentNumber = PromptNumericDocumentOrBack("Ingrese el [green]número de documento[/]:");
        if (documentNumber is null)
            return;

        var people = await _service.GetByDocumentNumberAsync(documentNumber);
        await ShowSearchResultsAsync(people, $"Resultado para documento: {documentNumber}");
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar Nueva Persona[/]");
        // El flujo pide primero el tipo de documento porque varias validaciones dependen de eso.
        var documentTypeId = await PromptDocumentTypeIdByCodeAsync();
        if (documentTypeId is null)
        {
            Pause();
            return;
        }

        var documentNumber = PromptNumericDocumentOrBack("Número de documento:");
        if (documentNumber is null)
        {
            Pause();
            return;
        }

        var firstName = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombres:");
        if (firstName is null)
        {
            Pause();
            return;
        }

        var lastName = ConsoleMenuHelpers.PromptRequiredStringOrBack("Apellidos:");
        if (lastName is null)
        {
            Pause();
            return;
        }

        var birthDate = PromptOptionalDateOrBack("Fecha de nacimiento (opcional, ENTER para omitir):");
        if (birthDate.WentBack)
        {
            Pause();
            return;
        }

        var gender = PromptOptionalGenderOrBack();
        if (gender.WentBack)
        {
            Pause();
            return;
        }

        var addressId = PromptOptionalIntOrBack("ID de dirección (opcional, ENTER para omitir):");
        if (addressId.WentBack)
        {
            Pause();
            return;
        }

        if (AnsiConsole.Confirm("¿Desea guardar los cambios?"))
        {
            try
            {
                await _service.CreateAsync(documentTypeId.Value, documentNumber, firstName, lastName, birthDate.Value, gender.Value, addressId.Value);
                AnsiConsole.MarkupLine("[green]✅ Persona registrada correctamente.[/]");
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
        var people = (await _service.GetAllAsync()).OrderBy(p => p.LastNames.Value).ThenBy(p => p.FirstName.Value).ToList();
        if (people.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay personas registradas.[/]");
            Pause();
            return;
        }

        var person = await PromptPersonSelectionAsync(people, "[yellow]Seleccione la persona a modificar:[/]");
        if (person is null)
            return;
        var id = person.Id.Value;

        AnsiConsole.MarkupLine($"Modificando: [bold]{person.FirstName.Value} {person.LastNames.Value}[/]");
        var currentCode = await GetDocumentTypeCodeAsync(person.DocumentTypeId.Value);
        AnsiConsole.MarkupLine($"[dim]Tipo de documento actual: {Markup.Escape(currentCode ?? person.DocumentTypeId.Value.ToString())}[/]");
        var documentTypeId = await PromptDocumentTypeIdByCodeAsync(person.DocumentTypeId.Value);
        if (documentTypeId is null)
        {
            Pause();
            return;
        }

        var documentNumber = PromptNumericDocumentOrBack("Nuevo número de documento:", person.DocumentNumber.Value);
        if (documentNumber is null)
        {
            Pause();
            return;
        }

        var firstName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevos nombres:", person.FirstName.Value);
        if (firstName is null)
        {
            Pause();
            return;
        }

        var lastName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevos apellidos:", person.LastNames.Value);
        if (lastName is null)
        {
            Pause();
            return;
        }

        var birthDate = PromptOptionalDateOrBack("Nueva fecha de nacimiento (opcional, ENTER para omitir):", person.BirthDate.Value);
        if (birthDate.WentBack)
        {
            Pause();
            return;
        }

        var gender = PromptOptionalGenderOrBack(person.Gender.Value);
        if (gender.WentBack)
        {
            Pause();
            return;
        }

        var addressId = PromptOptionalIntOrBack("Nuevo ID de dirección (opcional, ENTER para omitir):", person.AddressId?.Value);
        if (addressId.WentBack)
        {
            Pause();
            return;
        }

        try
        {
            await _service.UpdateAsync(id, documentTypeId.Value, documentNumber, firstName, lastName, birthDate.Value, gender.Value, addressId.Value);
            AnsiConsole.MarkupLine("[green]✅ Persona actualizada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var people = (await _service.GetAllAsync()).OrderBy(p => p.LastNames.Value).ThenBy(p => p.FirstName.Value).ToList();
        if (people.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay personas registradas.[/]");
            Pause();
            return;
        }

        var person = await PromptPersonSelectionAsync(people, "[red]Seleccione la persona a eliminar:[/]");
        if (person is null)
            return;

        try
        {
            if (!AnsiConsole.Confirm($"[red]¿Está seguro de eliminar a {Markup.Escape(person.FirstName.Value)} {Markup.Escape(person.LastNames.Value)}? Esta acción es irreversible.[/]"))
            {
                Pause();
                return;
            }

            await _service.DeleteByIdAsync(person.Id.Value);
            AnsiConsole.MarkupLine("[green]✅ Persona eliminada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task ShowSearchResultsAsync(IEnumerable<Person> people, string title)
    {
        var list = people.ToList();

        // Centralizamos la respuesta de búsqueda para no repetir el mismo patrón en cada filtro.
        if (list.Count == 0)
            AnsiConsole.MarkupLine("[red]❌ No se encontraron registros.[/]");
        else
            await ShowTableAsync(list, title);

        Pause();
    }

    private async Task ShowTableAsync(IEnumerable<Person> people, string title)
    {
        var docTypes = await _documentTypes.GetAllAsync();
        var codeByTypeId = docTypes.ToDictionary(t => t.Id.Value, t => t.Code.Value);

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[blue]Tipo (código)[/]")
            .AddColumn("[green]Documento[/]")
            .AddColumn("[green]Nombres[/]")
            .AddColumn("[green]Apellidos[/]")
            .AddColumn("[blue]Nacimiento[/]")
            .AddColumn("[blue]Género[/]")
            .AddColumn("[blue]Dirección[/]");

        foreach (var person in people)
        {
            var docCode = codeByTypeId.TryGetValue(person.DocumentTypeId.Value, out var c)
                ? c
                : person.DocumentTypeId.Value.ToString();

            table.AddRow(
                person.Id.Value.ToString(),
                docCode,
                person.DocumentNumber.Value,
                person.FirstName.Value,
                person.LastNames.Value,
                person.BirthDate.Value?.ToString("yyyy-MM-dd") ?? "-",
                person.Gender.Value?.ToString() ?? "-",
                person.AddressId?.Value.ToString() ?? "-"
            );
        }

        AnsiConsole.Write(table);
    }

    private static (bool WentBack, DateTime? Value) PromptOptionalDateOrBack(string prompt, DateTime? currentValue = null)
    {
        // Dejamos aceptar vacío porque fecha de nacimiento en este flujo es opcional.
        var raw = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            prompt,
            currentValue?.ToString("yyyy-MM-dd") ?? string.Empty,
            allowEmpty: true,
            validate: value => string.IsNullOrWhiteSpace(value) || DateTime.TryParse(value.Trim(), out _)
                ? null
                : "Fecha inválida.");

        if (raw is null)
            return (true, null);

        if (string.IsNullOrWhiteSpace(raw))
            return (false, null);

        return (false, DateTime.Parse(raw.Trim()));
    }

    private static (bool WentBack, int? Value) PromptOptionalIntOrBack(string prompt, int? currentValue = null)
    {
        var raw = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            prompt,
            currentValue?.ToString() ?? string.Empty,
            allowEmpty: true,
            validate: value => string.IsNullOrWhiteSpace(value) || int.TryParse(value.Trim(), out _)
                ? null
                : "Debe ser un número entero válido.");

        if (raw is null)
            return (true, null);

        if (string.IsNullOrWhiteSpace(raw))
            return (false, null);

        return (false, int.Parse(raw.Trim()));
    }

    private async Task<int?> PromptDocumentTypeIdByCodeAsync(int? defaultTypeId = null)
    {
        while (true)
        {
            var hint = defaultTypeId is null
                ? " [dim](ej. CC, TI, CE)[/]"
                : $" [dim](Enter = mantener tipo actual)[/]";

            var raw = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
                $"Código del tipo de documento{hint}:",
                string.Empty,
                allowEmpty: true);

            if (raw is null)
                return null;

            if (string.IsNullOrWhiteSpace(raw))
            {
                if (defaultTypeId is not null)
                    return defaultTypeId;
                AnsiConsole.MarkupLine("[yellow]Indique un código o use Enter solo al actualizar para mantener el actual.[/]");
                continue;
            }

            // Aquí preferimos pedir el código del documento porque para la persona usuaria
            // suele ser más natural recordar "CC" o "TI" que el ID interno.
            var docType = await _documentTypes.GetByCodeAsync(raw.Trim());
            if (docType is null)
            {
                AnsiConsole.MarkupLine($"[red]No existe un tipo de documento con código «{Markup.Escape(raw.Trim())}».[/]");
                continue;
            }

            return docType.Id.Value;
        }
    }

    private async Task<string?> GetDocumentTypeCodeAsync(int typeId)
    {
        var dt = await _documentTypes.GetByIdAsync(typeId);
        return dt?.Code.Value;
    }

    private static (bool WentBack, char? Value) PromptOptionalGenderOrBack(char? currentValue = null)
    {
        var raw = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            "Género (M/F/N, opcional):",
            currentValue?.ToString() ?? string.Empty,
            allowEmpty: true,
            validate: value =>
            {
                if (string.IsNullOrWhiteSpace(value))
                    return null;

                var letter = char.ToUpperInvariant(value.Trim()[0]);
                return letter is 'M' or 'F' or 'N'
                    ? null
                    : "Solo se permite M, F o N.";
            });

        if (raw is null)
            return (true, null);

        if (string.IsNullOrWhiteSpace(raw))
            return (false, null);

        return (false, char.ToUpperInvariant(raw.Trim()[0]));
    }

    private static string? PromptNumericDocumentOrBack(string label, string? currentValue = null)
    {
        return ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            label,
            currentValue ?? string.Empty,
            allowEmpty: false,
            validate: value =>
            {
                var text = value?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return "El número de documento es obligatorio.";
                if (!text.All(char.IsDigit))
                    return "El número de documento solo puede contener dígitos.";
                if (text.Length > 30)
                    return "El número de documento no puede superar los 30 dígitos.";
                return null;
            });
    }

    private static Task<Person?> PromptPersonSelectionAsync(IEnumerable<Person> source, string title)
    {
        var people = source
            .Where(p => p.Id is not null)
            .OrderBy(p => p.LastNames.Value)
            .ThenBy(p => p.FirstName.Value)
            .ToList();

        if (people.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay personas registradas.[/]");
            return Task.FromResult<Person?>(null);
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(people.Select(FormatPersonChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return Task.FromResult<Person?>(null);

        return Task.FromResult<Person?>(people.First(p => FormatPersonChoice(p) == selected));
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }

    private static string FormatPersonChoice(Person person)
    {
        return $"{person.Id.Value} · {Markup.Escape(person.FirstName.Value)} {Markup.Escape(person.LastNames.Value)} · Doc {Markup.Escape(person.DocumentNumber.Value)}";
    }
}
