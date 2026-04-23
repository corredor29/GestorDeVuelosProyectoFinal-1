using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Infrastructure.Repository;

// Repositorio del módulo de correos de personas.
// Aquí traducimos entre la entidad de EF y el aggregate de dominio PersonEmail.
public sealed class PersonEmailsRepository : IPersonEmailsRepository
{
    private readonly AppDbContext _context;

    public PersonEmailsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PersonEmail?> GetByIdAsync(PersonEmailsId id)
    {
        var entity = await _context.PersonEmails
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<PersonEmail>> GetByPersonIdAsync(int personId)
    {
        // Ordenamos primero el principal para que el listado sea más útil en consola.
        var entities = await _context.PersonEmails
            .AsNoTracking()
            .Where(x => x.PersonId == personId)
            .OrderByDescending(x => x.IsPrimary)
            .ThenBy(x => x.EmailUser)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<PersonEmail?> GetPrimaryByPersonIdAsync(int personId)
    {
        var entity = await _context.PersonEmails
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PersonId == personId && x.IsPrimary);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<PersonEmail>> GetAllAsync()
    {
        var entities = await _context.PersonEmails
            .AsNoTracking()
            .OrderBy(x => x.PersonId)
            .ThenByDescending(x => x.IsPrimary)
            .ThenBy(x => x.EmailUser)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(PersonEmail personEmail)
    {
        await _context.PersonEmails.AddAsync(MapToEntity(personEmail));
    }

    public async Task UpdateAsync(PersonEmail personEmail)
    {
        // Igual que en otros repositorios, actualizamos la entidad rastreada
        // en vez de adjuntar una nueva.
        var entity = await _context.PersonEmails
            .FirstOrDefaultAsync(x => x.Id == personEmail.Id.Value);

        if (entity is null)
            throw new InvalidOperationException($"Person email with id '{personEmail.Id.Value}' not found.");

        entity.PersonId = personEmail.PersonId;
        entity.EmailUser = personEmail.UserEmail.Value;
        entity.EmailDomainId = personEmail.EmailDomainId.Value;
        entity.IsPrimary = personEmail.IsPrimary.Value;
    }

    public async Task DeleteAsync(PersonEmailsId id)
    {
        var entity = await _context.PersonEmails
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null)
            // Aquí preferimos fallar explícitamente para que admin sepa que el registro no existe.
            throw new InvalidOperationException($"Person email with id '{id.Value}' not found.");

        _context.PersonEmails.Remove(entity);
    }

    private static PersonEmail MapToDomain(PersonEmailEntity entity)
    {
        return PersonEmail.FromPrimitives(
            entity.Id,
            entity.PersonId,
            entity.EmailUser,
            entity.EmailDomainId,
            entity.IsPrimary);
    }

    private static PersonEmailEntity MapToEntity(PersonEmail personEmail)
    {
        return new PersonEmailEntity
        {
            Id = personEmail.Id.Value,
            PersonId = personEmail.PersonId,
            EmailUser = personEmail.UserEmail.Value,
            EmailDomainId = personEmail.EmailDomainId.Value,
            IsPrimary = personEmail.IsPrimary.Value
        };
    }
}
