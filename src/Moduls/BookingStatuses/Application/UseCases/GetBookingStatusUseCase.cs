using StatusAggregate = GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Aggregate.BookingStatuses;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.UseCases;

public sealed class GetBookingStatusUseCase
{
    private readonly IBookingStatuseRepository _repository;
    public GetBookingStatusUseCase(IBookingStatuseRepository repository) => _repository = repository;

    public async Task<StatusAggregate?> ExecuteById(int id) 
        => await _repository.GetByIdAsync(BookingStatusesId.Create(id));

    public async Task<IEnumerable<StatusAggregate>> ExecuteAll() 
        => await _repository.GetAllAsync();
}
