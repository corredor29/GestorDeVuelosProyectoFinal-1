using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;
using System.Linq;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.ui;

// Menú de códigos telefónicos por país.
// Es un catálogo pequeño, pero importante porque lo usan teléfonos y formularios de contacto.
public sealed class PhoneCodesMenu : IModuleUI
{
    private readonly IPhoneCodesService _service;

    public string Key => "phone_codes";
    public string Title => "📞  Gestión de Códigos Telefónicos";

    public PhoneCodesMenu(IPhoneCodesService service)
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
                        "1. Listar todos los códigos telefónicos",
                        "2. Buscar código por ID",
                        "3. Buscar código por código de país",
                        "4. Buscar código por nombre del país",
                        "5. Registrar nuevo Código",
                        "6. Actualizar Código",
                        "7. Eliminar Código",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0")) break;

            switch (option[0])
            {
                case '1': await ListAllAsync(); break;
                case '2': await SearchByIdAsync(); break;
                case '3': await SearchByCountryCodeAsync(); break;
                case '4': await SearchByCountryNameAsync(); break;
                case '5': await CreateAsync(); break;
                case '6': await UpdateAsync(); break;
                case '7': await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        ShowTable(items, "Todos los Códigos Telefónicos");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var item = await PromptPhoneCodeSelectionAsync("[yellow]Seleccione el código telefónico a consultar:[/]");
        if (item is null)
            return;

        ShowTable(new[] { item }, $"Resultado para ID: {item.Id.Value}");

        Pause();
    }

    private async Task SearchByCountryCodeAsync()
    {
        // Aquí permitimos ingresar el prefijo como número para que el flujo sea rápido al buscar.
        var code = PromptCountryCodeOrBack("Ingrese el [green]código del país[/] (ej: 57):");
        if (code is null)
            return;

        var item = await _service.GetByCountryCodeAsync(code);

        if (item == null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún código telefónico con ese código de país.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para código de país: {code}");

        Pause();
    }

    private async Task SearchByCountryNameAsync()
    {
        var countryName = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del país:");
        if (countryName is null)
            return;

        var item = await _service.GetByCountryNameAsync(countryName);

        if (item == null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún código telefónico con ese nombre de país.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para nombre de país: {countryName}");

        Pause();
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar Nuevo Código Telefónico[/]");
        var countryCode = PromptCountryCodeOrBack("Código del país:");
        if (countryCode is null)
            return;

        var countryName = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del país:");
        if (countryName is null)
            return;

        if (AnsiConsole.Confirm("¿Desea guardar los cambios?"))
        {
            try
            {
                await _service.CreateAsync(countryCode, countryName);
                AnsiConsole.MarkupLine("[green]✅ Código telefónico registrado correctamente.[/]");
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
        var items = (await _service.GetAllAsync())
            .OrderBy(x => x.CountryName.Value)
            .ThenBy(x => x.Code.Value)
            .ToList();

        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay códigos telefónicos registrados.[/]");
            Pause();
            return;
        }

        // En este módulo casi todo gira alrededor de escoger un país y ajustar código/nombre.
        var item = await PromptPhoneCodeSelectionAsync("[yellow]Seleccione el código telefónico a modificar:[/]", items);
        if (item is null)
            return;
        var id = item.Id.Value;

        AnsiConsole.MarkupLine($"Modificando: [bold]{item.Code.Value}[/] - {item.CountryName.Value}");
        var newCountryCode = PromptCountryCodeOrBack("Nuevo código del país:", item.Code.Value);
        if (newCountryCode is null)
            return;

        var newCountryName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo nombre del país:", item.CountryName.Value);
        if (newCountryName is null)
            return;

        try
        {
            await _service.UpdateAsync(id, newCountryCode, newCountryName);
            AnsiConsole.MarkupLine("[green]✅ Código telefónico actualizado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var items = (await _service.GetAllAsync())
            .OrderBy(x => x.CountryName.Value)
            .ThenBy(x => x.Code.Value)
            .ToList();

        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay códigos telefónicos para eliminar.[/]");
            Pause();
            return;
        }

        var item = await PromptPhoneCodeSelectionAsync("[red]Seleccione el código telefónico a eliminar:[/]", items);
        if (item is null)
            return;

        try
        {
            if (!AnsiConsole.Confirm($"[red]¿Está seguro de eliminar {Markup.Escape(item.CountryName.Value)} (+{item.Code.Value.TrimStart('+')})? Esta acción es irreversible.[/]"))
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

    private static void ShowTable(IEnumerable<PhoneCode> items, string title)
    {
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Código del país[/]")
            .AddColumn("[blue]Nombre del país[/]");

        foreach (var item in items)
            table.AddRow(item.Id.Value.ToString(), item.Code.Value, item.CountryName.Value);

        AnsiConsole.Write(table);
    }

    private static string? PromptCountryCodeOrBack(string label, string? defaultValue = null)
    {
        // Dejamos la validación aquí para reutilizarla tanto en crear como en actualizar.
        var normalizedDefault = string.IsNullOrWhiteSpace(defaultValue)
            ? null
            : defaultValue.Trim().TrimStart('+');

        return ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            label,
            normalizedDefault ?? string.Empty,
            allowEmpty: false,
            validate: value =>
            {
                var text = value?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return "El código del país es obligatorio.";

                var digits = text.TrimStart('+');
                if (!digits.All(char.IsDigit))
                    return "El código del país solo puede contener números.";
                if (digits.Length is < 1 or > 4)
                    return "El código del país debe tener entre 1 y 4 dígitos.";
                return null;
            })?.Trim();
    }

    private async Task<PhoneCode?> PromptPhoneCodeSelectionAsync(string title, IReadOnlyCollection<PhoneCode>? source = null)
    {
        var items = source?.ToList() ?? (await _service.GetAllAsync())
            .OrderBy(x => x.CountryName.Value)
            .ThenBy(x => x.Code.Value)
            .ToList();

        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay códigos telefónicos registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatPhoneCodeChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatPhoneCodeChoice(x) == selected);
    }

    private static string FormatPhoneCodeChoice(PhoneCode item) =>
        $"{item.Id.Value} · +{item.Code.Value.TrimStart('+')} · {Markup.Escape(item.CountryName.Value)}";

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }
}
