using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Application.UseCases;

public sealed class CancelCheckInUseCase
{
    private readonly IFlightsRepository _flights;
    private readonly IUnitOfWork _unitOfWork;

    public CancelCheckInUseCase(IFlightsRepository flights, IUnitOfWork unitOfWork)
    {
        _flights = flights;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int flightId, CancellationToken cancellationToken = default)
    {
        var flight = await _flights.GetByIdAsync(FlightsId.Create(flightId), cancellationToken);
        if (flight is null)
            throw new InvalidOperationException($"No existe el vuelo con id {flightId}.");

        if (flight.AvailableSeats.Value >= flight.TotalCapacity.Value)
            throw new InvalidOperationException("No se puede liberar mas asientos: ya estan todos disponibles.");

        flight.IncrementAvailableSeats(1);
        flight.TouchUpdatedAt();
        await _flights.UpdateAsync(flight, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
