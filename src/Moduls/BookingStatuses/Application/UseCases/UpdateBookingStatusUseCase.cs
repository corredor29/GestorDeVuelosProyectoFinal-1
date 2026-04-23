using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.UseCases;

public sealed class UpdateBookingStatusUseCase
{
    private readonly IBookingStatuseRepository _repository;
    public UpdateBookingStatusUseCase(IBookingStatuseRepository repository) => _repository = repository;

    public async Task Execute(int id, string newName)
    {
        var status = await _repository.GetByIdAsync(BookingStatusesId.Create(id));
        if (status == null) throw new KeyNotFoundException("Estado de reserva no encontrado.");

        status.UpdateName(newName);
        await _repository.UpdateAsync(status);
    }
}