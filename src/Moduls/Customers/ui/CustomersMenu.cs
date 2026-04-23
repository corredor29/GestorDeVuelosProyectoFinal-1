using GestorDeVuelosProyectoFinal.Moduls.People.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.ui;

// Este menú administra clientes a partir de personas ya existentes.
// Un cliente aquí es básicamente una persona marcada para participar en reservas.
public sealed class CustomersMenu : IModuleUI
{
    private readonly ICustomersService _service;
    private readonly IPersonService _people;

    public string Key => "customers";
    public string Title => "🧾  Gestión de clientes";

    public CustomersMenu(ICustomersService service, IPersonService people)
    {
        _service = service;
        _people = people;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule($"[yellow]{Title}[/]").RuleStyle("grey").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Seleccione una opción:")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "1. Listar todos los clientes",
                        "2. Buscar cliente por ID",
                        "3. Buscar cliente por ID de persona",
                        "4. Buscar cliente por número de documento",
                        "5. Registrar cliente",
                        "6. Eliminar cliente",
                        ConsoleMenuHelpers.VolverAlMenu));

            switch (option)
            {
                case "1. Listar todos los clientes": await ListAsync(); break;
                case "2. Buscar cliente por ID": await FindByIdAsync(); break;
                case "3. Buscar cliente por ID de persona": await FindByPersonIdAsync(); break;
                case "4. Buscar cliente por número de documento": await FindByDocumentAsync(); break;
                case "5. Registrar cliente": await CreateAsync(); break;
                case "6. Eliminar cliente": await DeleteAsync(); break;
                default: return;
            }
        }
    }

    private async Task ListAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Clientes registrados[/]").LeftJustified());
        var customers = await GetValidCustomersAsync();
        await RenderAsync(customers);
        Pause();
    }

    private async Task FindByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del cliente:");
        if (id is null)
            return;

        var customer = await _service.GetByIdAsync(id.Value);
        await RenderSingleAsync(customer, $"No existe un cliente con ID {id.Value}.");
    }

    private async Task FindByPersonIdAsync()
    {
        var personId = await PromptRegisteredPersonIdAsync();
        if (personId is null)
            return;

        var customer = await _service.GetByPersonIdAsync(personId.Value);
        await RenderSingleAsync(customer, $"No hay cliente asociado a la persona {personId.Value}.");
    }

    private async Task FindByDocumentAsync()
    {
        var documentNumber = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            "Número de documento:",
            string.Empty,
            allowEmpty: false,
            validate: value =>
            {
                var text = value?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return "El documento es obligatorio.";
                if (!text.All(char.IsDigit))
                    return "El documento solo puede contener dígitos.";
                if (text.Length > 30)
                    return "El documento no puede superar los 30 dígitos.";
                return null;
            });
        if (documentNumber is null)
            return;

        var customer = await _service.GetByDocumentNumberAsync(documentNumber.Trim());
        await RenderSingleAsync(customer, $"No hay cliente con documento «{Markup.Escape(documentNumber.Trim())}».");
    }

    private async Task CreateAsync()
    {
        // Solo dejamos registrar personas que todavía no estén asociadas como cliente.
        var personId = await PromptAvailablePersonIdAsync();
        if (personId is null)
        {
            Pause();
            return;
        }

        try
        {
            await _service.CreateAsync(personId.Value);
            AnsiConsole.MarkupLine("\n[green]✅ Cliente registrado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteAsync()
    {
        var customers = await GetValidCustomersAsync();
        if (customers.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay clientes registrados.[/]");
            Pause();
            return;
        }

        var selected = await PromptCustomerSelectionAsync(customers, "[red]Seleccione el cliente a eliminar:[/]");
        if (selected is null)
            return;

        if (!AnsiConsole.Confirm($"[red]¿Confirma la eliminación del cliente {Markup.Escape(await GetCustomerDisplayNameAsync(selected))}?[/]"))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteAsync(selected.Id.Value);
            AnsiConsole.MarkupLine("\n[green]✅ Cliente eliminado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task RenderAsync(IReadOnlyCollection<Customer> customers)
    {
        if (customers.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay clientes registrados.[/]");
            return;
        }

        var names = (await _people.GetAllAsync())
            .ToDictionary(p => p.Id.Value, p => $"{p.FirstName.Value} {p.LastNames.Value}");

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold]ID[/]")
            .AddColumn("[bold]Persona[/]")
            .AddColumn("[bold]Registrado[/]");

        // En el listado mostramos el nombre de la persona si existe; así evitamos una tabla fría solo con IDs.
        foreach (var item in customers.OrderBy(x => x.Id.Value))
        {
            var personLabel = names.TryGetValue(item.PersonId, out var n)
                ? $"{item.PersonId} · {Markup.Escape(n)}"
                : item.PersonId.ToString();

            table.AddRow(
                item.Id.Value.ToString(),
                personLabel,
                item.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
    }

    private async Task RenderSingleAsync(Customer? customer, string notFoundMessage)
    {
        if (customer is null)
        {
            AnsiConsole.MarkupLine($"\n[yellow]{notFoundMessage}[/]");
            Pause();
            return;
        }

        var person = await _people.GetByIdAsync(customer.PersonId);
        var personLine = person is null
            ? customer.PersonId.ToString()
            : $"{customer.PersonId} · {Markup.Escape($"{person.FirstName.Value} {person.LastNames.Value}")}";

        var panel = new Panel(
            $"ID cliente: [bold]{customer.Id.Value}[/]\n" +
            $"Persona: [bold]{personLine}[/]\n" +
            $"Registrado: [bold]{customer.CreatedAt.Value:yyyy-MM-dd HH:mm:ss}[/]")
            .Header("[grey]Cliente[/]")
            .BorderColor(Color.Grey);

        AnsiConsole.WriteLine();
        AnsiConsole.Write(panel);
        Pause();
    }

    private async Task<List<Customer>> GetValidCustomersAsync()
    {
        var customers = (await _service.GetAllAsync()).OrderBy(x => x.Id.Value).ToList();
        var people = (await _people.GetAllAsync())
            .Where(p => p.Id is not null)
            .ToDictionary(p => p.Id.Value, p => p);

        // Si hay clientes huérfanos por datos viejos o inconsistentes, los limpiamos
        // para que el panel no muestre registros rotos.
        var orphanCustomers = customers
            .Where(c => !people.ContainsKey(c.PersonId))
            .ToList();

        foreach (var orphan in orphanCustomers)
            await _service.DeleteAsync(orphan.Id.Value);

        return customers
            .Where(c => people.ContainsKey(c.PersonId))
            .ToList();
    }

    private async Task<int?> PromptAvailablePersonIdAsync()
    {
        var people = (await _people.GetAllAsync())
            .Where(p => p.Id is not null)
            .OrderBy(p => p.LastNames.Value)
            .ThenBy(p => p.FirstName.Value)
            .ToList();

        var existingPersonIds = (await GetValidCustomersAsync())
            .Select(c => c.PersonId)
            .ToHashSet();

        var available = people
            .Where(p => !existingPersonIds.Contains(p.Id.Value))
            .ToList();

        if (available.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay personas disponibles para registrar como clientes.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione la persona a registrar como cliente:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(available.Select(FormatPersonChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return available.First(p => FormatPersonChoice(p) == selected).Id.Value;
    }

    private async Task<int?> PromptRegisteredPersonIdAsync()
    {
        var people = (await _people.GetAllAsync())
            .Where(p => p.Id is not null)
            .OrderBy(p => p.LastNames.Value)
            .ThenBy(p => p.FirstName.Value)
            .ToList();

        if (people.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay personas registradas.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione la persona a consultar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(people.Select(FormatPersonChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return people.First(p => FormatPersonChoice(p) == selected).Id.Value;
    }

    private async Task<Customer?> PromptCustomerSelectionAsync(IReadOnlyCollection<Customer> customers, string title)
    {
        var peopleMap = (await _people.GetAllAsync())
            .Where(p => p.Id is not null)
            .ToDictionary(p => p.Id.Value, p => $"{p.FirstName.Value} {p.LastNames.Value}");

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(customers.Select(c => FormatCustomerChoice(c, peopleMap)).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return customers.First(c => FormatCustomerChoice(c, peopleMap) == selected);
    }

    private async Task<string> GetCustomerDisplayNameAsync(Customer customer)
    {
        var person = await _people.GetByIdAsync(customer.PersonId);
        return person is null
            ? $"ID {customer.Id.Value}"
            : $"{person.FirstName.Value} {person.LastNames.Value}";
    }

    private static string FormatPersonChoice(Person person) =>
        $"{person.Id.Value} · {Markup.Escape(person.FirstName.Value)} {Markup.Escape(person.LastNames.Value)} · Doc {Markup.Escape(person.DocumentNumber.Value)}";

    private static string FormatCustomerChoice(Customer customer, IReadOnlyDictionary<int, string> peopleMap)
    {
        var personName = peopleMap.TryGetValue(customer.PersonId, out var name)
            ? name
            : $"Persona {customer.PersonId}";

        return $"{customer.Id.Value} · {Markup.Escape(personName)}";
    }

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Pulse Enter para continuar...[/]").AllowEmpty());
    }
}
