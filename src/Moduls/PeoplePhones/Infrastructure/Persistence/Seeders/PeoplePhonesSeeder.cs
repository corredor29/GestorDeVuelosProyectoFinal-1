using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Infrastructure.Persistence.Seeders;

public sealed class PeoplePhonesSeeder
{
    private readonly AppDbContext _context;
    private readonly IPeoplePhonesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PeoplePhonesSeeder(AppDbContext context, IPeoplePhonesRepository repository, IUnitOfWork unitOfWork)
    {
        _context = context;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var personIds = await _context.Persons
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(5)
            .ToListAsync(cancellationToken);

        var phoneCodeId = await _context.PhoneCodes
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (personIds.Count == 0 || phoneCodeId is null)
            return;

        foreach (var personId in personIds)
        {
            var existing = (await _repository.GetByPersonIdAsync(PeopleId.Create(personId))).ToList();

            if (existing.Count > 0)
                continue;

            var local = (3000000 + personId).ToString();
            await _repository.SaveAsync(PersonPhone.Create(personId, phoneCodeId.Value, local, isPrimary: true));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
