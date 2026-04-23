// PassengerReservationsRepository.cs
using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightReservations.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Repository;

public sealed class PassengerReservationsRepository : IPassengerReservationsRepository
{
    private readonly AppDbContext _context;

    public PassengerReservationsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PassengerReservation?> GetByIdAsync(
        PassengerReservationId id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<PassengerReservationsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PassengerReservation?> FindByIdAsync(
        PassengerReservationId id,
        CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<PassengerReservation?> GetByFlightAndPassengerAsync(
        FlightReservationId flightReservationId,
        PassengersId passengerId,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<PassengerReservationsEntity>()
            .FirstOrDefaultAsync(x =>
                x.Flight_Reservation_Id == flightReservationId.Value &&
                x.Passenger_Id == passengerId.Value,
                cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<PassengerReservation>> GetByFlightReservationIdAsync(
        FlightReservationId flightReservationId,
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Set<PassengerReservationsEntity>()
            .Where(x => x.Flight_Reservation_Id == flightReservationId.Value)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<IReadOnlyCollection<PassengerReservation>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Set<PassengerReservationsEntity>()
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task SaveAsync(
        PassengerReservation passengerReservation,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(passengerReservation);
        await _context.Set<PassengerReservationsEntity>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        passengerReservation.SetId(entity.Id);
    }

    public async Task UpdateAsync(
        PassengerReservation passengerReservation,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<PassengerReservationsEntity>()
            .FirstOrDefaultAsync(x => x.Id == passengerReservation.Id.Value, cancellationToken);

        if (entity is null) return;

        entity.Flight_Reservation_Id = passengerReservation.FlightReservationId.Value;
        entity.Passenger_Id = passengerReservation.PassengerId.Value;

        _context.Set<PassengerReservationsEntity>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        PassengerReservationId id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<PassengerReservationsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null) return;

        _context.Set<PassengerReservationsEntity>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }


    private static PassengerReservation MapToDomain(PassengerReservationsEntity entity)
    {
        var domain = PassengerReservation.Create(
            entity.Flight_Reservation_Id,
            entity.Passenger_Id
        );
        domain.SetId(entity.Id);
        return domain;
    }

    private static PassengerReservationsEntity MapToEntity(PassengerReservation domain)
    {
        return new PassengerReservationsEntity
        {
            Flight_Reservation_Id = domain.FlightReservationId.Value,
            Passenger_Id = domain.PassengerId.Value
        };
    }
}