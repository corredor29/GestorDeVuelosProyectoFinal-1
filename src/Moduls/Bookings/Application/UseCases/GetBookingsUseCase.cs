using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.UseCases;

public sealed class GetBookingsUseCase
{
    private readonly IBookingsRepository _repository;

    public GetBookingsUseCase(IBookingsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(BookingId.Create(id), cancellationToken);

    public Task<IEnumerable<Booking>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
        => _repository.GetByClientIdAsync(CustomersId.Create(clientId), cancellationToken);

    public Task<IEnumerable<Booking>> GetByStatusIdAsync(int statusId, CancellationToken cancellationToken = default)
        => _repository.GetByStatusIdAsync(BookingStatusesId.Create(statusId), cancellationToken);

    public Task<IEnumerable<Booking>> GetByBookedAtRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        if (to < from)
            throw new ArgumentException("La fecha final no puede ser menor a la fecha inicial.");

        return _repository.GetByBookedAtRangeAsync(from, to, cancellationToken);
    }
}
