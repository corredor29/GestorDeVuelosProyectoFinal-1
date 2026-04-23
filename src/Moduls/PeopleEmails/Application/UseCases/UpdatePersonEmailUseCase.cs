using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Application.UseCases;

public sealed class UpdatePersonEmailUseCase
{
    private readonly IPersonEmailsRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePersonEmailUseCase(
        IPersonEmailsRepository repository,
        AppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, int personId, string emailUser, int emailDomainId, bool isPrimary)
    {
        var personEmail = await _repository.GetByIdAsync(PersonEmailsId.Create(id))
            ?? throw new InvalidOperationException($"Person email with id '{id}' not found.");

        await EnsureForeignKeysExistAsync(personId, emailDomainId);
        await EnsurePrimaryRuleAsync(personId, isPrimary, id);
        await EnsureDuplicateEmailAsync(personId, emailUser, emailDomainId, id);

        personEmail.Update(personId, emailUser, emailDomainId, isPrimary);

        await _repository.UpdateAsync(personEmail);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task EnsureForeignKeysExistAsync(int personId, int emailDomainId)
    {
        var personExists = await _context.Persons.AsNoTracking().AnyAsync(x => x.Id == personId);
        if (!personExists)
            throw new InvalidOperationException($"The person with id '{personId}' does not exist.");

        var domainExists = await _context.EmailDomains.AsNoTracking().AnyAsync(x => x.Id == emailDomainId);
        if (!domainExists)
            throw new InvalidOperationException($"The email domain with id '{emailDomainId}' does not exist.");
    }

    private async Task EnsurePrimaryRuleAsync(int personId, bool isPrimary, int currentId)
    {
        if (!isPrimary)
            return;

        var primary = await _repository.GetPrimaryByPersonIdAsync(personId);
        if (primary is not null && primary.Id.Value != currentId)
            throw new InvalidOperationException($"Person '{personId}' already has a primary email.");
    }

    private async Task EnsureDuplicateEmailAsync(int personId, string emailUser, int emailDomainId, int currentId)
    {
        var emails = await _repository.GetByPersonIdAsync(personId);
        var normalizedUser = emailUser.Trim().ToLowerInvariant();

        var duplicate = emails.FirstOrDefault(x =>
            x.UserEmail.Value == normalizedUser &&
            x.EmailDomainId.Value == emailDomainId &&
            x.Id.Value != currentId);

        if (duplicate is not null)
            throw new InvalidOperationException("This email already exists for the selected person.");
    }
}
