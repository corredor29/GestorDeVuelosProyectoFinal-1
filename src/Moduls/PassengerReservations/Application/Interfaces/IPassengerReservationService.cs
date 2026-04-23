using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Aggregate;
namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Application.Interfaces;

public interface IPassengerReservationService
{
    Task<PassengerReservation> CreateAsync(int flightReservationId, int passengerId, CancellationToken cancellationToken = default);
    Task<PassengerReservation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PassengerReservation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PassengerReservation> UpdateAsync(int id, int flightReservationId, int passengerId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
