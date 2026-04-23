using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.UseCases;

public class ReactiveFlightSeatUseCase
{
    private readonly IFlightSeatsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactiveFlightSeatUseCase(IFlightSeatsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteOccupyAsync(int id)
    {
        var seat = await _repository.GetByIdAsync(FlightSeatsId.Create(id))
            ?? throw new InvalidOperationException("Asiento no encontrado.");
        
        if (seat.IsOccupied.Value) 
            throw new InvalidOperationException("El asiento ya está ocupado.");

        seat.Occupy(); // Método que debes tener en el Aggregate
        await _repository.UpdateAsync(seat);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteReleaseAsync(int id)
    {
        var seat = await _repository.GetByIdAsync(FlightSeatsId.Create(id))
            ?? throw new InvalidOperationException("Asiento no encontrado.");

        seat.Release(); // Método que debes tener en el Aggregate
        await _repository.UpdateAsync(seat);
        await _unitOfWork.SaveChangesAsync();
    }
}