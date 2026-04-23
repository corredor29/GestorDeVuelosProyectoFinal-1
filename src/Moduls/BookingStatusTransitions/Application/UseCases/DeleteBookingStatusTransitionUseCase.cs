using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Application.UseCases;

public sealed class DeleteBookingStatusTransitionUseCase
{
    private readonly IBookingStatusTransitionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookingStatusTransitionUseCase(
        IBookingStatusTransitionsRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(BookingStatusTransitionsId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No se encontro la transicion con ID {id}.");

        await _repository.DeleteByIdAsync(BookingStatusTransitionsId.Create(id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
