using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.UseCases;

public sealed class DeleteBookingFlightUseCase
{
    private readonly IBookingFlightsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookingFlightUseCase(IBookingFlightsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(BookingFlightsId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No existe el booking_flight con id {id}.");

        await _repository.DeleteByIdAsync(BookingFlightsId.Create(id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
