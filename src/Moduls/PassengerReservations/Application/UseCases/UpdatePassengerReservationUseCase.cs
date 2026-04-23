using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Application.UseCases;

public sealed class UpdatePassengerReservationUseCase
{
    private readonly IPassengerReservationsRepository _PassengerReservationrepository;

    public UpdatePassengerReservationUseCase(IPassengerReservationsRepository PassengerReservationrepository)
    {
        _PassengerReservationrepository = PassengerReservationrepository;
    }

    public async Task<PassengerReservation> ExecuteAsync(
        int id,
        int flightReservationId,
        int passengerId,
        CancellationToken cancellationToken = default)
    {
        var passengerReservationId = PassengerReservationId.Create(id);

        var existing = await _PassengerReservationrepository.GetByIdAsync(passengerReservationId, cancellationToken);

        if (existing is null)
        {
            throw new KeyNotFoundException($"PassengerReservation with id '{id}' was not found.");
        }

        existing.Update(flightReservationId, passengerId);

        await _PassengerReservationrepository.UpdateAsync(existing, cancellationToken);

        return existing;
    }
}
