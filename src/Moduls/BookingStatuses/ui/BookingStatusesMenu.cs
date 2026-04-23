using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.ui;

public class BookingStatusesMenu
{
    private readonly IBookingStatusService _service;

    public BookingStatusesMenu(IBookingStatusService service)
    {
        _service = service;
    }

    public async Task Show()
    {
        var exit = false;
        while (!exit)
        {
            AnsiConsole.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]GESTIÓN DE ESTADOS DE RESERVA[/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Listar Estados",
                        "Crear Nuevo Estado",
                        "Actualizar Estado",
                        "Eliminar Estado",
                        "Buscar por ID",
                        "Volver al Menú Principal"
                    }));

            switch (option)
            {
                case "Listar Estados":
                    await ListStatuses();
                    break;
                case "Crear Nuevo Estado":
                    await CreateStatus();
                    break;
                case "Actualizar Estado":
                    await UpdateStatus();
                    break;
                case "Eliminar Estado":
                    await DeleteStatus();
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

    private async Task ListStatuses()
    {
        var statuses = await _service.GetAllStatuses();
        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[blue]ID[/]");
        table.AddColumn("[blue]Nombre del Estado[/]");

        foreach (var s in statuses)
        {
            table.AddRow(s.Id.Value.ToString(), s.Name.Value);
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[grey]Presione cualquier tecla para continuar...[/]");
        Console.ReadKey();
    }

    private async Task CreateStatus()
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]nombre[/] del nuevo estado:");
        if (name is null)
            return;

        try
        {
            await _service.CreateStatus(name);
            AnsiConsole.MarkupLine("[bold green]¡Estado creado correctamente![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {ex.Message}");
        }
        Thread.Sleep(2000);
    }

    private async Task UpdateStatus()
    {
        var statuses = (await _service.GetAllStatuses()).OrderBy(x => x.Id.Value).ToList();
        if (statuses.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay estados registrados.[/]");
            Thread.Sleep(1500);
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el estado a editar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(statuses.Select(s => $"{s.Id.Value} · {Markup.Escape(s.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var status = statuses.First(s => $"{s.Id.Value} · {Markup.Escape(s.Name.Value)}" == selected);
        var id = status.Id.Value;
        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Ingrese el [green]nuevo nombre[/]:", status.Name.Value);
        if (newName is null)
            return;

        try
        {
            await _service.UpdateStatus(id, newName);
            AnsiConsole.MarkupLine("[bold green]¡Estado actualizado correctamente![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {ex.Message}");
        }
        Thread.Sleep(2000);
    }

    private async Task DeleteStatus()
    {
        var statuses = (await _service.GetAllStatuses()).OrderBy(x => x.Id.Value).ToList();
        if (statuses.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay estados registrados.[/]");
            Thread.Sleep(1500);
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Seleccione el estado a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(statuses.Select(s => $"{s.Id.Value} · {Markup.Escape(s.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var status = statuses.First(s => $"{s.Id.Value} · {Markup.Escape(s.Name.Value)}" == selected);
        if (AnsiConsole.Confirm($"¿Está seguro de eliminar el estado con ID {status.Id.Value}?"))
        {
            await _service.DeleteStatus(status.Id.Value);
            AnsiConsole.MarkupLine("[bold yellow]Proceso de eliminación completado.[/]");
        }
        Thread.Sleep(1500);
    }

    private async Task SearchById()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [blue]ID[/] a buscar:");
        if (id is null)
            return;

        var s = await _service.GetStatusById(id.Value);

        if (s == null)
        {
            AnsiConsole.MarkupLine("[red]Estado no encontrado.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[green]ID:[/] {s.Id.Value}");
            AnsiConsole.MarkupLine($"[green]Nombre:[/] {s.Name.Value}");
        }
        AnsiConsole.MarkupLine("\n[grey]Presione cualquier tecla para continuar...[/]");
        Console.ReadKey();
    }
}
