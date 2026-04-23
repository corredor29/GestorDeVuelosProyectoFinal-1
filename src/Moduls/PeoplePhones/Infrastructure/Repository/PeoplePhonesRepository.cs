using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Infrastructure.Repository;

// Este repositorio maneja la persistencia de teléfonos asociados a personas.
// También resuelve búsquedas cruzadas como "por nombre de persona".
public sealed class PeoplePhonesRepository : IPeoplePhonesRepository
{
    private readonly AppDbContext _context;

    public PeoplePhonesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PersonPhone?> GetByIdAsync(PersonPhonesId id)
    {
        var entity = await _context.PeoplePhones.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<PersonPhone>> GetByPersonIdAsync(PeopleId personId)
    {
        var entities = await _context.PeoplePhones
            .AsNoTracking()
            .Where(x => x.PersonId == personId.Value)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<PersonPhone?> GetPrimaryByPersonIdAsync(PeopleId personId)
    {
        var entity = await _context.PeoplePhones
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PersonId == personId.Value && x.IsPrimary);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<PersonPhone>> GetByPhoneNumberAsync(PersonPhonesPhoneNumber phoneNumber)
    {
        var entities = await _context.PeoplePhones
            .AsNoTracking()
            .Where(x => x.PhoneNumber == phoneNumber.Value)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<PersonPhone>> GetByPhoneCodeIdAsync(PhoneCodesId phoneCodeId)
    {
        var entities = await _context.PeoplePhones
            .AsNoTracking()
            .Where(x => x.PhoneCodeId == phoneCodeId.Value)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<PersonPhone>> GetByPersonNameAsync(string personName)
    {
        var normalized = personName.Trim().ToLower();

        // Aquí necesitamos join con Persons porque el nombre no vive en la tabla de teléfonos.
        var entities = await (
            from phone in _context.PeoplePhones.AsNoTracking()
            join person in _context.Persons.AsNoTracking() on phone.PersonId equals person.Id
            where person.FirstName.ToLower().Contains(normalized) || person.LastName.ToLower().Contains(normalized)
            select phone
        ).ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<PersonPhone>> GetAllAsync()
    {
        var entities = await _context.PeoplePhones.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(PersonPhone personPhone)
    {
        await _context.PeoplePhones.AddAsync(MapToEntity(personPhone));
    }

    public Task UpdateAsync(PersonPhone personPhone)
    {
        // Como el aggregate ya viene completo, aquí se reemplaza el registro por su versión mapeada.
        _context.PeoplePhones.Update(MapToEntity(personPhone));
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(PersonPhonesId id)
    {
        var entity = await _context.PeoplePhones.FindAsync(id.Value);
        if (entity is not null)
            _context.PeoplePhones.Remove(entity);
    }

    public async Task<int> DeleteByPhoneNumberAsync(PersonPhonesPhoneNumber phoneNumber)
    {
        var entities = await _context.PeoplePhones.Where(x => x.PhoneNumber == phoneNumber.Value).ToListAsync();
        _context.PeoplePhones.RemoveRange(entities);
        return entities.Count;
    }

    public async Task<int> DeleteByPhoneCodeIdAsync(PhoneCodesId phoneCodeId)
    {
        var entities = await _context.PeoplePhones.Where(x => x.PhoneCodeId == phoneCodeId.Value).ToListAsync();
        _context.PeoplePhones.RemoveRange(entities);
        return entities.Count;
    }

    public async Task<int> DeleteByPersonNameAsync(string personName)
    {
        var normalized = personName.Trim().ToLower();
        var entities = await (
            from phone in _context.PeoplePhones
            join person in _context.Persons on phone.PersonId equals person.Id
            where person.FirstName.ToLower().Contains(normalized) || person.LastName.ToLower().Contains(normalized)
            select phone
        ).ToListAsync();

        _context.PeoplePhones.RemoveRange(entities);
        return entities.Count;
    }

    private static PersonPhone MapToDomain(PeoplePhoneEntity entity) =>
        PersonPhone.FromPrimitives(entity.Id, entity.PersonId, entity.PhoneCodeId, entity.PhoneNumber, entity.IsPrimary);

    private static PeoplePhoneEntity MapToEntity(PersonPhone aggregate) =>
        new()
        {
            Id = aggregate.Id?.Value ?? 0,
            PersonId = aggregate.PersonId.Value,
            PhoneCodeId = aggregate.PhoneCodeId.Value,
            PhoneNumber = aggregate.PhoneNumber.Value,
            IsPrimary = aggregate.IsPrimary.Value
        };
}
