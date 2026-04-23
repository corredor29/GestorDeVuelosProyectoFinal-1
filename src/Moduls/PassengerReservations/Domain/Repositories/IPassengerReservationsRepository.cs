using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightReservations.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Repositories;

public interface IPassengerReservationsRepository
{
    Task<PassengerReservation?> GetByIdAsync(PassengerReservationId id, CancellationToken cancellationToken = default);
    Task<PassengerReservation?> FindByIdAsync(PassengerReservationId id, CancellationToken cancellationToken = default);

    Task<PassengerReservation?> GetByFlightAndPassengerAsync(FlightReservationId flightReservationId, PassengersId passengerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PassengerReservation>> GetByFlightReservationIdAsync(FlightReservationId flightReservationId, CancellationToken cancellationToken = default);
    Task SaveAsync(PassengerReservation passengerReservation, CancellationToken cancellationToken = default);
    Task UpdateAsync(PassengerReservation passengerReservation, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PassengerReservation>> GetAllAsync(CancellationToken cancellationToken = default);

    Task DeleteAsync(PassengerReservationId id, CancellationToken cancellationToken = default);
}