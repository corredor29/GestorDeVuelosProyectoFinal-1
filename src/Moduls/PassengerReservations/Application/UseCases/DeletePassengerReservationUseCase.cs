using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Application.UseCases;

public sealed class DeletePassengerReservationUseCase
{
    private readonly IPassengerReservationsRepository _PassengerReservationrepository;

    public DeletePassengerReservationUseCase(IPassengerReservationsRepository PassengerReservationrepository)
    {
        _PassengerReservationrepository = PassengerReservationrepository;
    }

    public async Task<bool> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var passengerReservationId = PassengerReservationId.Create(id);

        var existing = await _PassengerReservationrepository.GetByIdAsync(passengerReservationId, cancellationToken);

        if (existing is null)
        {
            return false;
        }

        await _PassengerReservationrepository.DeleteAsync(passengerReservationId, cancellationToken);

        return true;
    }
}