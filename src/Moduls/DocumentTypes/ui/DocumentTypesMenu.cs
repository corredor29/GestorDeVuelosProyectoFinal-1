using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.ui;

// Menú del catálogo de tipos de documento.
// Suele parecer pequeño, pero varios módulos dependen de este catálogo para funcionar bien.
public sealed class DocumentTypesMenu : IModuleUI
{
    private readonly IDocumentTypesService _service;

    public string Key => "document_types";
    public string Title => "📄  Gestión de Tipos de Documento";

    public DocumentTypesMenu(IDocumentTypesService service)
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
                        "1. Listar todos los tipos de documento",
                        "2. Buscar tipo por ID",
                        "3. Buscar tipo por Nombre",
                        "4. Buscar tipo por Código",
                        "5. Registrar nuevo Tipo",
                        "6. Actualizar Tipo",
                        "7. Eliminar Tipo",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0"))
                break;

            switch (option[0])
            {
                case '1': await ListAllAsync(); break;
                case '2': await SearchByIdAsync(); break;
                case '3': await SearchByNameAsync(); break;
                case '4': await SearchByCodeAsync(); break;
                case '5': await CreateAsync(); break;
                case '6': await UpdateAsync(); break;
                case '7': await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        ShowTable(items, "Todos los Tipos de Documento");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] del tipo de documento:");
        if (id is null)
            return;
        var item = await _service.GetByIdAsync(id.Value);

        if (item == null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún tipo de documento con ese ID.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para ID: {id}");

        Pause();
    }

    private async Task SearchByNameAsync()
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]nombre[/] del tipo de documento:");
        if (name is null)
            return;
        var item = await _service.GetByNameAsync(name);

        if (item == null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún tipo de documento con ese nombre.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para: {name}");

        Pause();
    }

    private async Task SearchByCodeAsync()
    {
        var code = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]código[/] del tipo de documento:");
        if (code is null)
            return;
        var item = await _service.GetByCodeAsync(code);

        if (item == null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún tipo de documento con ese código.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para código: {code}");

        Pause();
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar Nuevo Tipo de Documento[/]");
        // Pedimos nombre y código porque ambos se usan luego en distintos puntos del sistema.
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del tipo de documento:");
        if (name is null)
            return;
        var code = ConsoleMenuHelpers.PromptRequiredStringOrBack("Código del documento:");
        if (code is null)
            return;

        if (AnsiConsole.Confirm("¿Desea guardar los cambios?"))
        {
            try
            {
                await _service.CreateAsync(name, code);
                AnsiConsole.MarkupLine("[green]✅ Tipo de documento registrado correctamente.[/]");
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
        // Como este catálogo es corto, escoger desde lista es más cómodo que escribir un ID manual.
        var item = await PromptDocumentTypeSelectionAsync("[yellow]Seleccione el tipo de documento a modificar:[/]");

        if (item is null)
        {
            Pause();
            return;
        }

        AnsiConsole.MarkupLine($"Modificando: [bold]{item.Name.Value}[/] ([blue]{item.Code.Value}[/])");
        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo nombre:", item.Name.Value, allowEmpty: false);
        if (newName is null)
        {
            Pause();
            return;
        }
        var newCode = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo código:", item.Code.Value, allowEmpty: false);
        if (newCode is null)
        {
            Pause();
            return;
        }

        try
        {
            await _service.UpdateAsync(item.Id.Value, newName, newCode);
            AnsiConsole.MarkupLine("[green]✅ Tipo de documento actualizado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var item = await PromptDocumentTypeSelectionAsync("[red]Seleccione el tipo de documento a eliminar:[/]");
        if (item is null)
        {
            Pause();
            return;
        }

        try
        {
            if (AnsiConsole.Confirm($"[red]¿Está seguro de eliminar {Markup.Escape(item.Name.Value)} ({Markup.Escape(item.Code.Value)})? Esta acción es irreversible.[/]"))
            {
                await _service.DeleteByIdAsync(item.Id.Value);
                AnsiConsole.MarkupLine("[green]✅ Operación procesada.[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private static void ShowTable(IEnumerable<DocumentType> items, string title)
    {
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Nombre[/]")
            .AddColumn("[blue]Código[/]");

        foreach (var item in items)
        {
            table.AddRow(item.Id.Value.ToString(), item.Name.Value, item.Code.Value);
        }

        AnsiConsole.Write(table);
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }

    private async Task<DocumentType?> PromptDocumentTypeSelectionAsync(string title)
    {
        var items = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay tipos de documento registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatDocumentTypeChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatDocumentTypeChoice(x) == selected);
    }

    private static string FormatDocumentTypeChoice(DocumentType item)
    {
        return $"{item.Id.Value} · {Markup.Escape(item.Name.Value)} ({Markup.Escape(item.Code.Value)})";
    }
}
