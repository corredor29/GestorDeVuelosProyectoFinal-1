using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Aggregate;
namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Application.UseCases;

public sealed class GetAllPassengerReservationUseCase
{
    private readonly IPassengerReservationsRepository _PassengerReservationsrepository;

    public GetAllPassengerReservationUseCase(IPassengerReservationsRepository PassengerReservationsrepository)
    {
        _PassengerReservationsrepository = PassengerReservationsrepository;
    }

    public Task<IReadOnlyCollection<PassengerReservation>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _PassengerReservationsrepository.GetAllAsync(cancellationToken);
    }
}