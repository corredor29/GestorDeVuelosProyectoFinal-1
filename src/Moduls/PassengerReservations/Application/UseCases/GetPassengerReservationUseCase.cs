using System;
using System.Threading;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Application.UseCases;

public sealed class GetPassengerReservationUseCase
{
    private readonly IPassengerReservationsRepository _PassengerReservationrepository;

    public GetPassengerReservationUseCase(IPassengerReservationsRepository PassengerReservationrepository)
    {
        _PassengerReservationrepository = PassengerReservationrepository;
    }

    public async Task<PassengerReservation> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var passengerReservationId = PassengerReservationId.Create(id);

        var result = await _PassengerReservationrepository.GetByIdAsync(passengerReservationId, cancellationToken);

        if (result is null)
        {
            throw new KeyNotFoundException($"PassengerReservation with id '{id}' was not found.");
        }

        return result;
    }
}