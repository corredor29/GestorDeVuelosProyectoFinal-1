using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Repository;

public sealed class PersonalPositionsRepository : IPersonalPositionsRepository
{
    private readonly AppDbContext _context;

    public PersonalPositionsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PersonalPosition?> GetByIdAsync(PersonalPositionsId id)
    {
        var entity = await _context.PersonalPositions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<PersonalPosition?> GetByNameAsync(PersonalPositionsName name)
    {
        var normalized = name.Value.Trim().ToLower();
        var entity = await _context.PersonalPositions.AsNoTracking().FirstOrDefaultAsync(x => x.Name.ToLower() == normalized);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<PersonalPosition>> GetAllAsync()
    {
        var entities = await _context.PersonalPositions.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(PersonalPosition position)
    {
        await _context.PersonalPositions.AddAsync(MapToEntity(position));
    }

    public Task UpdateAsync(PersonalPosition position)
    {
        _context.PersonalPositions.Update(MapToEntity(position));
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(PersonalPositionsId id)
    {
        var entity = await _context.PersonalPositions.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is null)
            return;

        await EnsureNoEmployeesAsync(entity.Id);
        _context.PersonalPositions.Remove(entity);
    }

    public async Task DeleteByNameAsync(PersonalPositionsName name)
    {
        var normalized = name.Value.Trim().ToLower();
        var entity = await _context.PersonalPositions.FirstOrDefaultAsync(x => x.Name.ToLower() == normalized);
        if (entity is null)
            return;

        await EnsureNoEmployeesAsync(entity.Id);
        _context.PersonalPositions.Remove(entity);
    }

    private async Task EnsureNoEmployeesAsync(int positionId)
    {
        var hasEmployees = await _context.Staffs.AsNoTracking().AnyAsync(x => x.PositionId == positionId);
        if (hasEmployees)
            throw new InvalidOperationException("No se puede eliminar el cargo porque tiene empleados asociados.");
    }

    private static PersonalPosition MapToDomain(PersonalPositionEntity entity)
    {
        return PersonalPosition.FromPrimitives(entity.Id, entity.Name);
    }

    private static PersonalPositionEntity MapToEntity(PersonalPosition aggregate)
    {
        return new PersonalPositionEntity
        {
            Id = aggregate.Id?.Value ?? 0,
            Name = aggregate.Name.Value
        };
    }
}
