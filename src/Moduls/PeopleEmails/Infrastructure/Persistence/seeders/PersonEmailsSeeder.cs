using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Infrastructure.Persistence.seeders;

public sealed class PersonEmailsSeeder
{
    private readonly AppDbContext _context;
    private readonly IPersonEmailsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PersonEmailsSeeder(AppDbContext context, IPersonEmailsRepository repository, IUnitOfWork unitOfWork)
    {
        _context = context;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        var personIds = await _context.Persons
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(3)
            .ToListAsync();

        var domainIds = await _context.EmailDomains
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(3)
            .ToListAsync();

        if (personIds.Count == 0 || domainIds.Count == 0)
            return;

        foreach (var personId in personIds)
        {
            var existing = (await _repository.GetByPersonIdAsync(personId)).ToList();
            if (existing.Count > 0)
                continue;

            await _repository.SaveAsync(PersonEmail.Create(personId, $"user{personId}", domainIds[0], true));

            if (domainIds.Count > 1)
                await _repository.SaveAsync(PersonEmail.Create(personId, $"alt{personId}", domainIds[1], false));
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
