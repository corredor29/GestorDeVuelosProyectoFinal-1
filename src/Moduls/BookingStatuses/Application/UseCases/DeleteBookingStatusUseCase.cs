using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.UseCases;

public sealed class DeleteBookingStatusUseCase
{
    private readonly IBookingStatuseRepository _repository;
    public DeleteBookingStatusUseCase(IBookingStatuseRepository repository) => _repository = repository;

    public async Task Execute(int id) => await _repository.DeleteAsync(BookingStatusesId.Create(id));
}