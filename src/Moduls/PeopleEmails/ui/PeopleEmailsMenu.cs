using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.People.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.ui;

// Menú para administrar correos asociados a personas.
// La idea es que el admin pueda ver, crear, actualizar y eliminar correos sin trabajar con IDs a ciegas.
public sealed class PeopleEmailsMenu : IModuleUI
{
    private readonly IPersonEmailsService _emails;
    private readonly IEmailDomainService _domains;
    private readonly IPersonService _people;

    public string Key => "peopleemails";
    public string Title => "📧  Correos de personas";

    public PeopleEmailsMenu(IPersonEmailsService emails, IEmailDomainService domains, IPersonService people)
    {
        _emails = emails;
        _domains = domains;
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
                        "1. Listar todos los correos",
                        "2. Listar correos por persona",
                        "3. Registrar correo de persona",
                        "4. Actualizar correo de persona",
                        "5. Eliminar correo de persona",
                        ConsoleMenuHelpers.VolverAlMenu));

            switch (option)
            {
                case "1. Listar todos los correos": await ListAllAsync(); break;
                case "2. Listar correos por persona": await ListByPersonAsync(); break;
                case "3. Registrar correo de persona": await CreateAsync(); break;
                case "4. Actualizar correo de persona": await UpdateAsync(); break;
                case "5. Eliminar correo de persona": await DeleteAsync(); break;
                default: return;
            }
        }
    }

    private async Task ListAllAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Todos los correos de personas[/]").LeftJustified());
        var emails = (await _emails.GetAllAsync()).ToList();
        await RenderTableAsync(emails);
        Pause();
    }

    private async Task ListByPersonAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Correos por persona[/]").LeftJustified());
        var personId = await PromptPersonIdAsync();
        if (personId is null)
        {
            Pause();
            return;
        }

        var emails = (await _emails.GetByPersonIdAsync(personId.Value)).ToList();
        await RenderTableAsync(emails);
        Pause();
    }

    private async Task CreateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Registrar correo de persona[/]").LeftJustified());

        // Primero se escoge la persona y luego se arma el correo con usuario + dominio.
        var personId = await PromptPersonIdAsync();
        if (personId is null)
        {
            AnsiConsole.MarkupLine("[yellow]Operación cancelada.[/]");
            Pause();
            return;
        }

        var emailUser = PromptEmailUserLocalPart();
        var domainId = await PromptEmailDomainIdAsync();
        if (domainId is null)
        {
            AnsiConsole.MarkupLine("[yellow]Operación cancelada.[/]");
            Pause();
            return;
        }

        var isPrimary = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("¿Es el correo principal?")
                .AddChoices("Sí", "No")) == "Sí";

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("¿Guardar el correo?");
        if (confirm != ConsoleMenuHelpers.SaveChoice.Guardar)
        {
            Pause();
            return;
        }

        try
        {
            await _emails.CreateAsync(personId.Value, emailUser, domainId.Value, isPrimary);
            AnsiConsole.MarkupLine("\n[green]✅ Correo registrado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Actualizar correo de persona[/]").LeftJustified());

        var emails = (await _emails.GetAllAsync()).ToList();
        if (emails.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay correos registrados.[/]");
            Pause();
            return;
        }

        var current = await PromptEmailSelectionAsync("Seleccione el correo a modificar:");
        if (current is null)
            return;

        var personId = await PromptPersonIdAsync(current.PersonId);
        if (personId is null)
        {
            Pause();
            return;
        }

        var emailUser = PromptEmailUserLocalPart(current.UserEmail.Value);
        var domainId = await PromptEmailDomainIdAsync(current.EmailDomainId.Value);
        if (domainId is null)
        {
            AnsiConsole.MarkupLine("[yellow]Operación cancelada.[/]");
            Pause();
            return;
        }

        var isPrimary = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("¿Es el correo principal?")
                .AddChoices("Sí", "No")) == "Sí";

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("¿Guardar los cambios?");
        if (confirm != ConsoleMenuHelpers.SaveChoice.Guardar)
        {
            Pause();
            return;
        }

        try
        {
            await _emails.UpdateAsync(current.Id.Value, personId.Value, emailUser, domainId.Value, isPrimary);
            AnsiConsole.MarkupLine("\n[green]✅ Correo actualizado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold red]Eliminar correo de persona[/]").LeftJustified());

        var emails = (await _emails.GetAllAsync()).ToList();
        if (emails.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay correos para eliminar.[/]");
            Pause();
            return;
        }

        var current = await PromptEmailSelectionAsync("Seleccione el correo a eliminar:");
        if (current is null)
            return;

        if (!AnsiConsole.Confirm("[red]¿Confirma la eliminación?[/]"))
        {
            Pause();
            return;
        }

        try
        {
            await _emails.DeleteAsync(current.Id.Value);
            AnsiConsole.MarkupLine("\n[green]✅ Correo eliminado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task RenderTableAsync(IReadOnlyCollection<PersonEmail> emails)
    {
        if (emails.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay correos de personas registrados.[/]");
            return;
        }

        var domainMap = (await _domains.GetAllAsync())
            .ToDictionary(d => d.Id.Value, d => d.Domain.Value);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold]ID[/]")
            .AddColumn("[bold]Persona[/]")
            .AddColumn("[bold]Usuario[/]")
            .AddColumn("[bold]Dominio[/]")
            .AddColumn("[bold]Principal[/]");

        // El dominio se resuelve aparte para que el listado muestre el correo completo
        // de forma más entendible.
        foreach (var item in emails.OrderBy(x => x.PersonId).ThenByDescending(x => x.IsPrimary.Value))
        {
            var dom = domainMap.TryGetValue(item.EmailDomainId.Value, out var d) ? d : "?";
            table.AddRow(
                item.Id.Value.ToString(),
                item.PersonId.ToString(),
                item.UserEmail.Value,
                dom,
                item.IsPrimary.Value ? "[green]Sí[/]" : "[grey]No[/]");
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
    }

    private static string PromptEmailUserLocalPart(string? defaultValue = null)
    {
        var prompt = new TextPrompt<string>("Parte local del correo [dim](antes de @, sin espacios)[/]:")
            .Validate(value =>
            {
                var clean = value.Trim().ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(clean))
                    return ValidationResult.Error("[red]El usuario del correo es obligatorio.[/]");
                if (clean.Contains('@') || clean.Contains(' '))
                    return ValidationResult.Error("[red]No incluya «@» ni espacios.[/]");
                return ValidationResult.Success();
            });

        if (!string.IsNullOrEmpty(defaultValue))
            prompt.DefaultValue(defaultValue);

        return AnsiConsole.Prompt(prompt).Trim().ToLowerInvariant();
    }

    private async Task<int?> PromptEmailDomainIdAsync(int? currentDomainId = null)
    {
        var all = (await _domains.GetAllAsync()).OrderBy(d => d.Domain.Value, StringComparer.OrdinalIgnoreCase).ToList();
        if (all.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay dominios de correo configurados.[/]");
            return null;
        }

        var choices = all
            .Select(d => FormatDomainChoice(d))
            .ToList();

        choices.Add(ConsoleMenuHelpers.VolverSinGuardar);

        var title = currentDomainId is null
            ? "Seleccione el [yellow]dominio[/] de correo:"
            : $"Seleccione el [yellow]dominio[/] [dim](actual id: {currentDomainId})[/]:";

        var sel = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .PageSize(15)
                .AddChoices(choices));

        if (sel == ConsoleMenuHelpers.VolverSinGuardar)
            return null;

        var picked = all.First(d => FormatDomainChoice(d) == sel);
        return picked.Id.Value;
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

        var choices = people
            .Select(FormatPersonChoice)
            .ToList();

        choices.Add(ConsoleMenuHelpers.VolverSinGuardar);

        var title = currentPersonId is null
            ? "Seleccione la [yellow]persona[/] para registrar el correo:"
            : $"Seleccione la [yellow]persona[/] [dim](actual id: {currentPersonId})[/]:";

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(choices));

        if (selected == ConsoleMenuHelpers.VolverSinGuardar)
            return null;

        return people.First(p => FormatPersonChoice(p) == selected).Id.Value;
    }

    private static string FormatDomainChoice(EmailDomain d) =>
        $"{d.Id.Value} · {Markup.Escape(d.Domain.Value)}";

    private static string FormatPersonChoice(Person person) =>
        $"{person.Id.Value} · {Markup.Escape(person.FirstName.Value)} {Markup.Escape(person.LastNames.Value)} · Doc {Markup.Escape(person.DocumentNumber.Value)}";

    private async Task<PersonEmail?> PromptEmailSelectionAsync(string title)
    {
        var emails = (await _emails.GetAllAsync())
            .OrderBy(x => x.PersonId)
            .ThenBy(x => x.UserEmail.Value)
            .ToList();
        if (emails.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay correos registrados.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(emails.Select(FormatEmailChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return emails.First(x => FormatEmailChoice(x) == selected);
    }

    private static string FormatEmailChoice(PersonEmail item) =>
        $"{item.Id.Value} · Persona {item.PersonId} · {Markup.Escape(item.UserEmail.Value)}";

    private static int PromptPositiveInt(string label, int? defaultValue = null)
    {
        var p = new TextPrompt<int>($"[deepskyblue1]{label}[/]")
            .Validate(value => value > 0
                ? ValidationResult.Success()
                : ValidationResult.Error("[red]El valor debe ser mayor que cero.[/]"));

        if (defaultValue is not null)
            p.DefaultValue(defaultValue.Value);

        return AnsiConsole.Prompt(p);
    }

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Pulse Enter para continuar...[/]").AllowEmpty());
    }
}
