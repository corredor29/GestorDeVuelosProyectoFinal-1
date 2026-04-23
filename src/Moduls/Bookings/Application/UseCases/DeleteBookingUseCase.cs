using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.UseCases;

public sealed class DeleteBookingUseCase
{
    private readonly IBookingsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookingUseCase(IBookingsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(BookingId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No existe la reserva con id {id}.");

        await _repository.DeleteByIdAsync(BookingId.Create(id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
    {
        var existing = (await _repository.GetByClientIdAsync(CustomersId.Create(clientId), cancellationToken)).ToList();
        if (existing.Count == 0)
            throw new InvalidOperationException($"No existen reservas para el cliente con id {clientId}.");

        await _repository.DeleteByClientIdAsync(CustomersId.Create(clientId), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
