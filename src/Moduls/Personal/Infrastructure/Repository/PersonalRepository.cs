using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Repository;

public sealed class PersonalRepository : IPersonalRepository
{
    private readonly AppDbContext _context;

    public PersonalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Staff?> GetByIdAsync(PersonalId id)
    {
        var entity = await _context.Staffs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<Staff?> GetByPersonIdAsync(PeopleId personId)
    {
        var entity = await _context.Staffs.AsNoTracking().FirstOrDefaultAsync(x => x.PersonId == personId.Value);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Staff>> GetByPersonNameAsync(string personName)
    {
        var normalized = personName.Trim().ToLower();
        var entities = await (
            from staff in _context.Staffs.AsNoTracking()
            join person in _context.Persons.AsNoTracking() on staff.PersonId equals person.Id
            where person.FirstName.ToLower().Contains(normalized) || person.LastName.ToLower().Contains(normalized)
            orderby person.FirstName, person.LastName
            select staff
        ).ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Staff>> GetByPositionIdAsync(PersonalPositionsId positionId)
    {
        var entities = await _context.Staffs.AsNoTracking().Where(x => x.PositionId == positionId.Value).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Staff>> GetByPositionNameAsync(string positionName)
    {
        var normalized = positionName.Trim().ToLower();
        var entities = await (
            from staff in _context.Staffs.AsNoTracking()
            join position in _context.PersonalPositions.AsNoTracking() on staff.PositionId equals position.Id
            where position.Name.ToLower().Contains(normalized)
            orderby position.Name
            select staff
        ).ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Staff>> GetByAirlineIdAsync(AirlinesId airlineId)
    {
        var entities = await _context.Staffs.AsNoTracking().Where(x => x.AirlineId == airlineId.Value).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Staff>> GetByAirlineNameAsync(string airlineName)
    {
        var normalized = airlineName.Trim().ToLower();
        var entities = await (
            from staff in _context.Staffs.AsNoTracking()
            join airline in _context.Airlines.AsNoTracking() on staff.AirlineId equals airline.Id
            where airline.Name.ToLower().Contains(normalized)
            orderby airline.Name
            select staff
        ).ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Staff>> GetByAirportIdAsync(AirportsId airportId)
    {
        var entities = await _context.Staffs.AsNoTracking().Where(x => x.AirportId == airportId.Value).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Staff>> GetByAirportNameAsync(string airportName)
    {
        var normalized = airportName.Trim().ToLower();
        var entities = await (
            from staff in _context.Staffs.AsNoTracking()
            join airport in _context.Airports.AsNoTracking() on staff.AirportId equals airport.Id
            where airport.Name.ToLower().Contains(normalized)
            orderby airport.Name
            select staff
        ).ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Staff>> GetByIsActiveAsync(bool isActive)
    {
        var entities = await _context.Staffs.AsNoTracking().Where(x => x.IsActive == isActive).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Staff>> GetAllAsync()
    {
        var entities = await _context.Staffs.AsNoTracking().OrderBy(x => x.Id).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Staff staff)
    {
        await _context.Staffs.AddAsync(MapToEntity(staff));
    }

    public Task UpdateAsync(Staff staff)
    {
        _context.Staffs.Update(MapToEntity(staff));
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(PersonalId id)
    {
        var entity = await _context.Staffs.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is null)
            return;

        await EnsureNoFutureFlightsAsync(entity.Id);
        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    public async Task<int> DeleteByPersonNameAsync(string personName)
    {
        var normalized = personName.Trim().ToLower();
        var entities = await (
            from staff in _context.Staffs
            join person in _context.Persons on staff.PersonId equals person.Id
            where (person.FirstName.ToLower().Contains(normalized) || person.LastName.ToLower().Contains(normalized)) && staff.IsActive
            select staff
        ).ToListAsync();

        await EnsureNoFutureFlightsAsync(entities.Select(x => x.Id));
        foreach (var entity in entities)
        {
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        return entities.Count;
    }

    public async Task<int> DeleteByPositionNameAsync(string positionName)
    {
        var normalized = positionName.Trim().ToLower();
        var entities = await (
            from staff in _context.Staffs
            join position in _context.PersonalPositions on staff.PositionId equals position.Id
            where position.Name.ToLower().Contains(normalized) && staff.IsActive
            select staff
        ).ToListAsync();

        await EnsureNoFutureFlightsAsync(entities.Select(x => x.Id));
        foreach (var entity in entities)
        {
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        return entities.Count;
    }

    public async Task<int> DeleteByAirlineNameAsync(string airlineName)
    {
        var normalized = airlineName.Trim().ToLower();
        var entities = await (
            from staff in _context.Staffs
            join airline in _context.Airlines on staff.AirlineId equals airline.Id
            where airline.Name.ToLower().Contains(normalized) && staff.IsActive
            select staff
        ).ToListAsync();

        await EnsureNoFutureFlightsAsync(entities.Select(x => x.Id));
        foreach (var entity in entities)
        {
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        return entities.Count;
    }

    public async Task<int> DeleteByAirportNameAsync(string airportName)
    {
        var normalized = airportName.Trim().ToLower();
        var entities = await (
            from staff in _context.Staffs
            join airport in _context.Airports on staff.AirportId equals airport.Id
            where airport.Name.ToLower().Contains(normalized) && staff.IsActive
            select staff
        ).ToListAsync();

        await EnsureNoFutureFlightsAsync(entities.Select(x => x.Id));
        foreach (var entity in entities)
        {
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        return entities.Count;
    }

    public async Task ReactivateAsync(PersonalId id)
    {
        var entity = await _context.Staffs.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is null)
            return;

        entity.IsActive = true;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    private async Task EnsureNoFutureFlightsAsync(int staffId)
    {
        await EnsureNoFutureFlightsAsync(new[] { staffId });
    }

    private async Task EnsureNoFutureFlightsAsync(IEnumerable<int> staffIds)
    {
        var staffIdList = staffIds.Distinct().ToList();
        if (staffIdList.Count == 0)
            return;

        var hasFutureFlights = await (
            from assignment in _context.FlightAssignments.AsNoTracking()
            join flight in _context.Flights.AsNoTracking() on assignment.FlightId equals flight.Id
            where staffIdList.Contains(assignment.StaffId) && flight.DepartureAt > DateTime.UtcNow
            select assignment.Id
        ).AnyAsync();

        if (hasFutureFlights)
            throw new InvalidOperationException("No se puede desactivar el empleado porque tiene vuelos futuros asignados.");
    }

    private static Staff MapToDomain(StaffEntity entity)
    {
        return Staff.FromPrimitives(
            entity.Id,
            entity.PersonId,
            entity.PositionId,
            entity.AirlineId,
            entity.AirportId,
            entity.HireDate,
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt);
    }

    private static StaffEntity MapToEntity(Staff aggregate)
    {
        return new StaffEntity
        {
            Id = aggregate.Id?.Value ?? 0,
            PersonId = aggregate.PersonId.Value,
            PositionId = aggregate.PositionId.Value,
            AirlineId = aggregate.AirlineId?.Value,
            AirportId = aggregate.AirportId?.Value,
            HireDate = aggregate.HireDate.Value,
            IsActive = aggregate.IsActive.Value,
            CreatedAt = aggregate.CreatedIn.Value,
            UpdatedAt = aggregate.UpdatedIn.Value
        };
    }
}
