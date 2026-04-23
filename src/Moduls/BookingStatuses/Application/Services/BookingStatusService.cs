using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.UseCases;
using StatusAggregate = GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Aggregate.BookingStatuses;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.Services;

public sealed class BookingStatusService : IBookingStatusService
{
    private readonly CreateBookingStatusUseCase _createUseCase;
    private readonly UpdateBookingStatusUseCase _updateUseCase;
    private readonly GetBookingStatusUseCase _getUseCase;
    private readonly DeleteBookingStatusUseCase _deleteUseCase;

    public BookingStatusService(
        CreateBookingStatusUseCase createUseCase,
        UpdateBookingStatusUseCase updateUseCase,
        GetBookingStatusUseCase getUseCase,
        DeleteBookingStatusUseCase deleteUseCase)
    {
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _getUseCase = getUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task CreateStatus(string name) => _createUseCase.Execute(name);
    public Task UpdateStatus(int id, string name) => _updateUseCase.Execute(id, name);
    public Task<StatusAggregate?> GetStatusById(int id) => _getUseCase.ExecuteById(id);
    public Task<IEnumerable<StatusAggregate>> GetAllStatuses() => _getUseCase.ExecuteAll();
    public Task DeleteStatus(int id) => _deleteUseCase.Execute(id);
}