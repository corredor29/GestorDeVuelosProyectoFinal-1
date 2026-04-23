using StatusAggregate = GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Aggregate.BookingStatuses;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.UseCases;

public sealed class CreateBookingStatusUseCase
{
    private readonly IBookingStatuseRepository _repository;
    public CreateBookingStatusUseCase(IBookingStatuseRepository repository) => _repository = repository;

    public async Task Execute(string name)
    {
        var status = StatusAggregate.Create(name);
        await _repository.SaveAsync(status);
    }
}