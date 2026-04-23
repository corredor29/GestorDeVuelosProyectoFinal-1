
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Application.UseCases;

public sealed class CreatePassengerReservationUseCase
{
    private readonly IPassengerReservationsRepository _PassengerReservationsrepository;

    public CreatePassengerReservationUseCase(IPassengerReservationsRepository PassengerReservationsrepository)
    {
        _PassengerReservationsrepository = PassengerReservationsrepository;
    }

    public async Task<PassengerReservation> ExecuteAsync(
        int flightReservationId,
        int passengerId,
        CancellationToken cancellationToken = default)
    {
        var passengerReservation = PassengerReservation.Create(
            flightReservationId,
            passengerId
        );

        await _PassengerReservationsrepository.SaveAsync(passengerReservation, cancellationToken);

        return passengerReservation;
    }
}
