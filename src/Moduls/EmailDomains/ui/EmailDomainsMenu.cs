using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.ui;

public sealed class EmailDomainsMenu : IModuleUI
{
    private readonly IEmailDomainService _service;

    public string Key => "emaildomains";
    public string Title => "Dominios de correo";

    public EmailDomainsMenu(IEmailDomainService service)
    {
        _service = service;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Gestión de dominios de correo [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar y Enter para seleccionar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar dominios de correo",
                        "Crear dominio de correo",
                        "Actualizar dominio de correo",
                        "Eliminar dominio de correo",
                        ConsoleMenuHelpers.VolverAlMenu));

            switch (option)
            {
                case "Listar dominios de correo": await ListAsync(); break;
                case "Crear dominio de correo": await CreateAsync(); break;
                case "Actualizar dominio de correo": await UpdateAsync(); break;
                case "Eliminar dominio de correo": await DeleteAsync(); break;
                default: return;
            }
        }
    }

    private async Task ListAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Todos los dominios de correo[/]").LeftJustified());

        var domains = (await _service.GetAllAsync()).ToList();

        if (domains.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay dominios de correo registrados.[/]");
        }
        else
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Grey)
                .AddColumn(new TableColumn("[bold grey]ID[/]").Centered())
                .AddColumn(new TableColumn("[bold grey]Dominio[/]"));

            foreach (var item in domains)
                table.AddRow(item.Id.Value.ToString(), $"[white]{item.Domain.Value}[/]");

            AnsiConsole.WriteLine();
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"\n[grey]Total: {domains.Count} dominio(s)[/]");
        }

        Pause();
    }

    private async Task CreateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Crear dominio de correo[/]").LeftJustified());
        AnsiConsole.WriteLine();

        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Crear dominio de correo"))
        {
            Pause();
            return;
        }

        var domainRaw = PromptDomainOrBack("Dominio de correo:");
        if (domainRaw is null)
        {
            AnsiConsole.MarkupLine("[yellow]Operación cancelada (volver).[/]");
            Pause();
            return;
        }

        var domain = domainRaw.ToLowerInvariant();

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel($"[bold]{domain}[/]")
            .Header("[grey]Dominio a crear[/]")
            .BorderColor(Color.Grey));

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("\n¿Confirmar creación?");
        if (confirm == ConsoleMenuHelpers.SaveChoice.VolverAlMenu)
        {
            Pause();
            return;
        }

        if (confirm == ConsoleMenuHelpers.SaveChoice.Guardar)
        {
            try
            {
                await _service.CreateAsync(domain);
                AnsiConsole.MarkupLine("\n[green]Dominio de correo creado correctamente.[/]");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
            }
        }
        else
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");

        Pause();
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Actualizar dominio de correo[/]").LeftJustified());

        var domains = (await _service.GetAllAsync()).ToList();
        if (domains.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay dominios de correo para actualizar.[/]");
            Pause();
            return;
        }

        var choices = domains.Select(d => d.Domain.Value).ToList();
        choices.Add(ConsoleMenuHelpers.VolverAlMenu);

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Selecciona el dominio a actualizar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(choices));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Actualizar dominio"))
        {
            Pause();
            return;
        }

        var newDomainRaw = PromptDomainOrBack($"Nuevo dominio para {selected}:");
        if (newDomainRaw is null)
        {
            AnsiConsole.MarkupLine("[yellow]Operación cancelada (volver).[/]");
            Pause();
            return;
        }

        var newDomain = newDomainRaw.ToLowerInvariant();

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel($"[strikethrough grey]{selected}[/]  [grey]->[/]  [bold]{newDomain}[/]")
            .Header("[grey]Cambio a aplicar[/]")
            .BorderColor(Color.Grey));

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("\n¿Confirmar actualización?");
        if (confirm == ConsoleMenuHelpers.SaveChoice.VolverAlMenu)
        {
            Pause();
            return;
        }

        if (confirm == ConsoleMenuHelpers.SaveChoice.Guardar)
        {
            try
            {
                await _service.UpdateAsync(selected, newDomain);
                AnsiConsole.MarkupLine("\n[green]Dominio de correo actualizado correctamente.[/]");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
            }
        }
        else
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");

        Pause();
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Eliminar dominio de correo[/]").LeftJustified());

        var domains = (await _service.GetAllAsync()).ToList();
        if (domains.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay dominios de correo para eliminar.[/]");
            Pause();
            return;
        }

        var choices = domains.Select(d => d.Domain.Value).ToList();
        choices.Add(ConsoleMenuHelpers.VolverAlMenu);

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Selecciona el dominio a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(choices));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel($"[bold red]{selected}[/]")
            .Header("[red]A punto de eliminar[/]")
            .BorderColor(Color.Red));

        var confirm = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n¿Confirma eliminación?")
                .HighlightStyle(new Style(foreground: Color.Red))
                .AddChoices("Sí, eliminar", "No, cancelar", ConsoleMenuHelpers.VolverAlMenu));

        if (confirm == ConsoleMenuHelpers.VolverAlMenu || confirm == "No, cancelar")
        {
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");
            Pause();
            return;
        }

        try
        {
            await _service.DeleteAsync(selected);
            AnsiConsole.MarkupLine("\n[green]Dominio de correo eliminado correctamente.[/]");
        }
        catch (InvalidOperationException ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private static string? PromptDomainOrBack(string label)
    {
        return ConsoleMenuHelpers.PromptRequiredStringOrBack(
            $"[deepskyblue1]{label}[/]",
            raw =>
            {
                var clean = raw.Trim().ToLowerInvariant();
                if (clean.Length > 100)
                    return "El dominio no puede superar 100 caracteres.";
                if (!clean.Contains('.'))
                    return "El dominio debe incluir un punto. Ejemplo: gmail.com";
                if (clean.Contains('@') || clean.Contains(' '))
                    return "El dominio no debe contener '@' ni espacios.";
                return null;
            });
    }

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(
            new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]")
                .AllowEmpty());
    }
}
