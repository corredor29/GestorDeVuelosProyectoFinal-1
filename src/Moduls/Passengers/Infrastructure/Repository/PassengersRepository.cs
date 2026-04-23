using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using PassengerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate.Passengers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Repository;

public sealed class PassengersRepository : IPassengerRepository
{
    private readonly AppDbContext _context;

    public PassengersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PassengerAggregate?> GetByIdAsync(PassengersId id)
    {
        var entity = await _context.Passengers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PassengerAggregate?> GetByPersonIdAsync(PassengersPersonId personId)
    {
        var entity = await _context.Passengers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PersonId == personId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<PassengerAggregate>> GetAllAsync()
    {
        var entities = await _context.Passengers
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(PassengerAggregate passengers)
    {
        var entity = MapToEntity(passengers);
        await _context.Passengers.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PassengersId id)
    {
        var entity = await _context.Passengers.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is not null)
        {
            _context.Passengers.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    // Mapeos Privados
    private static PassengerAggregate MapToDomain(PassengersEntity entity)
    {
        return PassengerAggregate.FromPrimitives(
            entity.Id,
            entity.PersonId,
            entity.PassengerTypeId
        );
    }

    private static PassengersEntity MapToEntity(PassengerAggregate aggregate)
    {
        return new PassengersEntity
        {
            // El Id no se envía en el Save si es autoincrement (0 por defecto)
            PersonId = aggregate.PersonId.Value,
            PassengerTypeId = aggregate.PassengerTypeId.Value
        };
    }
}