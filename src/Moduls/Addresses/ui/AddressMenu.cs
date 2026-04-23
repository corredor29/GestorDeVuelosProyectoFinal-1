using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.UI;

// Menú del CRUD de direcciones.
// Aquí el flujo se apoya bastante en listas de ciudades y tipos de vía para no capturar IDs a ciegas.
public sealed class AddressMenu : IModuleUI
{
    private readonly IAddressService _service;
    private readonly IStreetTypeService _streetTypes;
    private readonly ICityService _cities;

    public string Key => "addresses";
    public string Title => "🏠  Gestión de Direcciones";

    public AddressMenu(IAddressService service, IStreetTypeService streetTypes, ICityService cities)
    {
        _service = service;
        _streetTypes = streetTypes;
        _cities = cities;
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
                    .AddChoices(new[] {
                        "1. Listar todas las direcciones",
                        "2. Buscar por Calle y Número",
                        "3. Registrar nueva Dirección",
                        "4. Actualizar Dirección",
                        "5. Eliminar Dirección",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0")) break;

            switch (option[0])
            {
                case '1': await ListAllAsync(); break;
                case '2': await SearchByStreetAsync(); break;
                case '3': await CreateAsync(); break;
                case '4': await UpdateAsync(); break;
                case '5': await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var addresses = await _service.GetAllAsync();
        ShowTable(addresses, "Listado Global de Direcciones");
        Pause();
    }

    private async Task SearchByStreetAsync()
    {
        // Esta búsqueda es útil cuando aún no se conoce el ID exacto de la dirección.
        var street = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre de la calle:");
        if (street is null)
            return;

        var number = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Número [dim](opcional, vacío = omitir)[/]:", string.Empty, allowEmpty: true);
        if (number is null)
            return;
        
        var address = await _service.GetByStreetAndNumberAsync(street, string.IsNullOrWhiteSpace(number) ? null : number);

        if (address == null)
        {
            AnsiConsole.MarkupLine("\n[red]❌ No se encontró ninguna dirección con esos datos.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("\n[green][bold]✓ Dirección Localizada[/][/]");
            ShowTable(new[] { address }, "Resultado de la búsqueda");
        }
        Pause();
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("\n[bold blue]--- REGISTRAR NUEVA DIRECCIÓN ---[/]");

        if (!ConsoleMenuHelpers.TryBeginFormOrBack("¿Desea continuar con el registro?"))
        {
            Pause();
            return;
        }

        // Primero resolvemos el tipo de vía porque el aggregate de dirección lo necesita desde el inicio.
        var streetTypeId = await ResolveStreetTypeIdAsync();
        if (streetTypeId is null)
        {
            Pause();
            return;
        }

        var streetName = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre de la [yellow]vía[/]:");
        if (streetName is null)
        {
            Pause();
            return;
        }

        var number = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Número [dim](opcional, vacío = omitir)[/]:", string.Empty, allowEmpty: true);
        if (number is null) return;
        var complement = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Complemento/Apto [dim](opcional)[/]:", string.Empty, allowEmpty: true);
        if (complement is null) return;

        var cityId = await PromptCityIdOrCancelAsync();
        if (cityId is null)
        {
            Pause();
            return;
        }

        var postalCode = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Código postal [dim](opcional)[/]:", string.Empty, allowEmpty: true);
        if (postalCode is null) return;

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("¿Desea guardar esta dirección?");
        if (confirm == ConsoleMenuHelpers.SaveChoice.VolverAlMenu)
            return;
        if (confirm != ConsoleMenuHelpers.SaveChoice.Guardar)
        {
            Pause();
            return;
        }

        var beforeIds = (await _service.GetAllAsync()).Select(x => x.Id.Value).ToHashSet();
        await _service.CreateAsync(streetTypeId.Value, streetName, number, complement, postalCode, cityId.Value);
        AnsiConsole.MarkupLine("\n[green]✅ Dirección guardada correctamente.[/]");

        var created = (await _service.GetAllAsync())
            .Where(x => !beforeIds.Contains(x.Id.Value))
            .OrderByDescending(x => x.Id.Value)
            .FirstOrDefault();

        if (created is null)
        {
            created = await _service.GetByStreetAndNumberAsync(streetName, string.IsNullOrWhiteSpace(number) ? null : number);
        }

        if (created is not null)
        {
            AnsiConsole.MarkupLine("[grey]Dirección recién registrada:[/]");
            ShowTable(new[] { created }, "Nueva dirección");
        }
        else
        {
            // Este mensaje ayuda a detectar enseguida cuando el problema no es del menú sino de persistencia.
            AnsiConsole.MarkupLine("[red]La dirección no reapareció al consultar el listado después de guardar. Esto confirma un problema de persistencia o consulta en la base.[/]");
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        var address = await PromptAddressSelectionAsync("\n[bold]Seleccione la dirección a modificar[/]");

        if (address == null)
        {
            Pause();
            return;
        }

        var id = address.Id.Value;

        AnsiConsole.MarkupLine($"\nModificando: [bold cyan]{address.StreetName.Value} {address.Number.Value}[/]");

        if (!ConsoleMenuHelpers.TryBeginFormOrBack("¿Desea continuar con la actualización?"))
        {
            Pause();
            return;
        }

        // Repetimos el mismo orden lógico del alta para que editar no se sienta como otro formulario distinto.
        var stId = await ResolveStreetTypeIdAsync(address.RoadTypeId.Value);
        if (stId is null)
        {
            Pause();
            return;
        }

        var name = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo nombre de vía:", address.StreetName.Value, allowEmpty: false);
        if (name is null) return;

        var num = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            "Nuevo número [dim](vacío = sin número)[/]:",
            address.Number.Value ?? string.Empty,
            allowEmpty: true);
        if (num is null) return;
        var comp = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            "Nuevo complemento [dim](opcional)[/]:",
            address.Complement.Value ?? string.Empty,
            allowEmpty: true);
        if (comp is null) return;

        var city = await PromptCityIdOrCancelAsync(address.CityId.Value);
        if (city is null)
        {
            Pause();
            return;
        }

        var pc = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            "Nuevo código postal [dim](opcional)[/]:",
            address.PostalCode.Value ?? string.Empty,
            allowEmpty: true);
        if (pc is null) return;

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("¿Guardar los cambios?");
        if (confirm == ConsoleMenuHelpers.SaveChoice.VolverAlMenu)
            return;
        if (confirm != ConsoleMenuHelpers.SaveChoice.Guardar)
        {
            Pause();
            return;
        }

        await _service.UpdateAsync(id, stId.Value, name, num, comp, pc, city.Value);
        AnsiConsole.MarkupLine("\n[green]✅ Dirección actualizada correctamente.[/]");
        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var address = await PromptAddressSelectionAsync("\n[red]Seleccione la dirección a eliminar[/]");
        if (address is null)
        {
            Pause();
            return;
        }
        
        if (AnsiConsole.Confirm($"[red]¿Está seguro de eliminar {Markup.Escape(FormatAddressChoice(address))}? Esta acción es irreversible.[/]"))
        {
            var success = await _service.DeleteAsync(address.Id.Value);
            if (success) AnsiConsole.MarkupLine("\n[green]✅ Dirección eliminada.[/]");
            else AnsiConsole.MarkupLine("\n[red]❌ No se pudo eliminar (verifique si el ID existe).[/]");
        }
        Pause();
    }

    private async Task<int?> PromptStreetTypeIdOrCancelAsync(int? currentId = null)
    {
        var types = (await _streetTypes.GetAllAsync()).OrderBy(t => t.Id.Value).ToList();
        if (types.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay tipos de vía registrados. Cree uno antes de continuar.[/]");
            return null;
        }

        var labels = types.Select(FormatStreetTypeChoice).ToList();
        labels.Add(ConsoleMenuHelpers.VolverSinGuardar);

        var title = currentId is null
            ? "Seleccione el [yellow]tipo de vía[/]:"
            : $"Seleccione el [yellow]tipo de vía[/] [dim](actual: {currentId})[/]:";

        var sel = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .PageSize(15)
                .AddChoices(labels));

        if (sel == ConsoleMenuHelpers.VolverSinGuardar)
            return null;

        var picked = types.First(t => FormatStreetTypeChoice(t) == sel);
        return picked.Id.Value;
    }

    private async Task<int?> PromptCityIdOrCancelAsync(int? currentId = null)
    {
        var cities = (await _cities.GetAllAsync()).OrderBy(c => c.Name.Value).ToList();
        if (cities.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay ciudades registradas.[/]");
            return null;
        }

        var labels = cities.Select(FormatCityChoice).ToList();
        labels.Add(ConsoleMenuHelpers.VolverSinGuardar);

        var title = currentId is null
            ? "Seleccione la [yellow]ciudad[/]:"
            : $"Seleccione la [yellow]ciudad[/] [dim](actual id: {currentId})[/]:";

        var sel = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .PageSize(20)
                .AddChoices(labels));

        if (sel == ConsoleMenuHelpers.VolverSinGuardar)
            return null;

        var picked = cities.First(c => FormatCityChoice(c) == sel);
        return picked.Id.Value;
    }

    private static string FormatStreetTypeChoice(StreetType t) =>
        $"{t.Id.Value} · {Markup.Escape(t.Name.Value)}";

    private static string FormatCityChoice(City c) =>
        $"{c.Id.Value} · {Markup.Escape(c.Name.Value)}";

    private static string FormatAddressChoice(Address address)
    {
        var number = string.IsNullOrWhiteSpace(address.Number.Value) ? "s/n" : address.Number.Value;
        return $"{address.Id.Value} · {Markup.Escape(address.StreetName.Value)} {Markup.Escape(number!)} · Ciudad {address.CityId.Value}";
    }

    private async Task<Address?> PromptAddressSelectionAsync(string title)
    {
        var addresses = (await _service.GetAllAsync()).OrderBy(a => a.Id.Value).ToList();
        if (addresses.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay direcciones registradas.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(addresses.Select(FormatAddressChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return addresses.First(a => FormatAddressChoice(a) == selected);
    }

    private void ShowTable(IEnumerable<Address> addresses, string title)
    {
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Calle/Vía[/]")
            .AddColumn("[green]Número[/]")
            .AddColumn("[green]Complemento[/]")
            .AddColumn("[green]Ciudad (ID)[/]")
            .AddColumn("[blue]C.P.[/]");

        foreach (var a in addresses)
        {
            table.AddRow(
                a.Id.Value.ToString(),
                a.StreetName.Value,
                a.Number.Value ?? "-",
                a.Complement.Value ?? "-",
                a.CityId.Value.ToString(),
                a.PostalCode.Value ?? "-"
            );
        }

        AnsiConsole.Write(table);
    }

    private void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }

    private async Task<int?> ResolveStreetTypeIdAsync(int? currentId = null)
    {
        var types = (await _streetTypes.GetAllAsync()).OrderBy(t => t.Id.Value).ToList();
        if (types.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay tipos de vía registrados. Cree uno antes de continuar.[/]");
            return null;
        }

        if (types.Count == 1)
            return types[0].Id.Value;

        return await PromptStreetTypeIdOrCancelAsync(currentId);
    }
}
