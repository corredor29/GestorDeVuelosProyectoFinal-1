using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.UI;

// Menú para el catálogo de tipos de vía.
// Este catálogo se usa bastante en direcciones para evitar textos libres inconsistentes.
public sealed class StreetTypeMenu : IModuleUI
{
    private readonly IStreetTypeService _service;
    
    // Dejamos estas propiedades para que el módulo se integre igual que los demás menús.
    public string Key => "street_types";
    public string Title => "🛣️  Gestión de Tipos de Vía";

    public StreetTypeMenu(IStreetTypeService service)
    {
        _service = service;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            // Mantenemos el mismo estilo visual del resto de catálogos base.
            AnsiConsole.Write(new Rule($"[yellow]{Title}[/]").RuleStyle("grey").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Seleccione una opción:")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Listar todos los tipos de vía",
                        "2. Buscar por Nombre",
                        "3. Registrar nuevo Tipo",
                        "4. Actualizar Tipo",
                        "5. Eliminar Tipo",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0")) break;

            switch (option[0])
            {
                case '1': await ListAllAsync(); break;
                case '2': await SearchByNameAsync(); break;
                case '3': await CreateAsync(); break;
                case '4': await UpdateAsync(); break;
                case '5': await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var types = await _service.GetAllAsync();
        ShowTable(types, "Todos los Tipos de Vía");
        Pause();
    }

    private async Task SearchByNameAsync()
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del tipo de vía:");
        if (name is null)
            return;

        var type = await _service.GetByNameAsync(name);

        if (type == null)
        {
            AnsiConsole.MarkupLine("\n[red]❌ No se encontró ningún tipo de vía con ese nombre.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("\n[green][bold]✓ Registro Encontrado[/][/]");
            ShowTable(new[] { type }, $"Resultado para: {name}");
        }
        Pause();
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("\n[bold blue]--- REGISTRAR NUEVO TIPO DE VÍA ---[/]");
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del tipo de vía:");
        if (name is null)
            return;

        if (AnsiConsole.Confirm("¿Desea guardar los cambios?"))
        {
            await _service.CreateAsync(name);
            AnsiConsole.MarkupLine("\n[green]✅ Tipo de vía registrado correctamente.[/]");
        }
        Pause();
    }

    private async Task UpdateAsync()
    {
        var types = (await _service.GetAllAsync()).OrderBy(t => t.Name.Value).ToList();
        if (types.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay tipos de vía disponibles.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n[bold]Seleccione el tipo de vía a actualizar[/]")
                .PageSize(12)
                .AddChoices(types.Select(FormatStreetTypeChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var type = types.First(t => FormatStreetTypeChoice(t) == selected);
        var id = type.Id.Value;

        AnsiConsole.MarkupLine($"\nModificando: [bold cyan]{Markup.Escape(type.Name.Value)}[/] [dim](id {id})[/]");

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            "Nuevo nombre:",
            type.Name.Value,
            allowEmpty: false);

        if (newName is null)
        {
            Pause();
            return;
        }

        if (string.Equals(newName.Trim(), type.Name.Value, StringComparison.OrdinalIgnoreCase))
        {
            AnsiConsole.MarkupLine("[grey]Sin cambios en el nombre.[/]");
            Pause();
            return;
        }

        try
        {
            await _service.UpdateAsync(id, newName.Trim());
            AnsiConsole.MarkupLine("\n[green]✅ Registro actualizado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var types = (await _service.GetAllAsync()).OrderBy(t => t.Name.Value).ToList();
        if (types.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay tipos de vía disponibles.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n[red]Seleccione el tipo de vía a eliminar[/]")
                .PageSize(12)
                .AddChoices(types.Select(FormatStreetTypeChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var type = types.First(t => FormatStreetTypeChoice(t) == selected);
        if (AnsiConsole.Confirm($"[red]¿Está seguro de eliminar {Markup.Escape(type.Name.Value)}? Esta acción es irreversible.[/]"))
        {
            await _service.DeleteAsync(type.Id.Value);
            AnsiConsole.MarkupLine("\n[green]✅ Operación procesada con éxito.[/]");
        }
        Pause();
    }

    private void ShowTable(IEnumerable<GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Aggregate.StreetType> types, string title)
    {
        // Tabla sencilla porque aquí solo necesitamos ID y nombre.
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Nombre[/]");

        foreach (var t in types)
        {
            table.AddRow(t.Id.Value.ToString(), t.Name.Value);
        }

        AnsiConsole.Write(table);
    }

    private static string FormatStreetTypeChoice(GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Aggregate.StreetType type)
    {
        return $"{type.Id.Value} · {Markup.Escape(type.Name.Value)}";
    }

    private void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }
}
