using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.UseCases;

public sealed class DeleteFlightSeatUseCase
{
    private readonly IFlightSeatsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFlightSeatUseCase(IFlightSeatsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(FlightSeatsId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el asiento con ID {id}.");

        await _repository.DeleteByIdAsync(FlightSeatsId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }
}
