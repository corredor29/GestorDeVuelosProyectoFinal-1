using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.UI;

public class CardIssuersConsoleUI
{
    private readonly ICardIssuersService _service;

    public CardIssuersConsoleUI(ICardIssuersService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Emisores").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case "Listar emisores de tarjeta":
                    await ListAllAsync();
                    break;
                case "Buscar por ID":
                    await GetByIdAsync();
                    break;
                case "Crear emisor de tarjeta":
                    await CreateAsync();
                    break;
                case "Actualizar emisor de tarjeta":
                    await UpdateAsync();
                    break;
                case "Eliminar emisor de tarjeta":
                    await DeleteAsync();
                    break;
                case "Volver":
                    return;
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Markup("[grey]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }
    }

    private IEnumerable<string> GetMenuOptions()
    {
        var options = new List<string>
        {
            "Listar emisores de tarjeta",
            "Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add("Crear emisor de tarjeta");
            options.Add("Actualizar emisor de tarjeta");
            options.Add("Eliminar emisor de tarjeta");
        }

        options.Add("Volver");
        return options;
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        var list = items.ToList();

        if (!list.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay emisores de tarjeta registrados.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[blue]ID[/]")
            .AddColumn("[blue]Nombre[/]")
            .AddColumn("[blue]Número emisor[/]");

        foreach (var item in list)
            table.AddRow(
                item.Id.Value.ToString(),
                item.Name.Value,
                item.IssuerNumber.Value);

        AnsiConsole.Write(table);
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar emisor de tarjeta por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;

        try
        {
            var item = await _service.GetByIdAsync(id.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Emisor de tarjeta no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("ID",             item.Id.Value.ToString());
            table.AddRow("Nombre",         item.Name.Value);
            table.AddRow("Número emisor",  item.IssuerNumber.Value);

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Crear nuevo emisor de tarjeta[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;

        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Nombre:[/]");
        if (name is null)
            return;

        var issuerNumber = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Número emisor:[/]");
        if (issuerNumber is null)
            return;

        await AnsiConsole.Status()
            .StartAsync("Creando emisor de tarjeta...", async ctx =>
            {
                try
                {
                    var item = await _service.CreateAsync(id.Value, name, issuerNumber);
                    AnsiConsole.MarkupLine($"[green]Emisor '[bold]{item.Name.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow]Actualizar emisor de tarjeta[/]");

        var issuers = (await _service.GetAllAsync()).OrderBy(x => x.Id.Value).ToList();
        if (issuers.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay emisores de tarjeta registrados.[/]");
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el emisor a actualizar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(issuers.Select(i => $"{i.Id.Value} · {Markup.Escape(i.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var existing = issuers.First(i => $"{i.Id.Value} · {Markup.Escape(i.Name.Value)}" == selected);
        var id = existing.Id.Value;

        AnsiConsole.MarkupLine($"[grey]Nombre actual: {existing.Name.Value}[/]");
        AnsiConsole.MarkupLine($"[grey]Número emisor actual: {existing.IssuerNumber.Value}[/]");

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo nombre (Enter para mantener):[/]", string.Empty, allowEmpty: true);
        if (newName is null)
            return;

        var newIssuerNumber = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo número emisor (Enter para mantener):[/]", string.Empty, allowEmpty: true);
        if (newIssuerNumber is null)
            return;

        await AnsiConsole.Status()
            .StartAsync("Actualizando emisor de tarjeta...", async ctx =>
            {
                try
                {
                    var updated = await _service.UpdateAsync(
                        id,
                        string.IsNullOrWhiteSpace(newName)         ? null : newName,
                        string.IsNullOrWhiteSpace(newIssuerNumber) ? null : newIssuerNumber);

                    AnsiConsole.MarkupLine($"[green]Emisor '[bold]{updated.Name.Value}[/]' actualizado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]Eliminar emisor de tarjeta[/]");

        var issuers = (await _service.GetAllAsync()).OrderBy(x => x.Id.Value).ToList();
        if (issuers.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay emisores de tarjeta registrados.[/]");
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el emisor a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(issuers.Select(i => $"{i.Id.Value} · {Markup.Escape(i.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var issuer = issuers.First(i => $"{i.Id.Value} · {Markup.Escape(i.Name.Value)}" == selected);

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el emisor con ID {issuer.Id.Value}?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando emisor de tarjeta...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(issuer.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Emisor de tarjeta eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Emisor de tarjeta no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }
}
