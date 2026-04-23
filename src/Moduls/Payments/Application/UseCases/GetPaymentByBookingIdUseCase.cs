using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.UseCases;

public sealed class GetPaymentByBookingIdUseCase
{
    private readonly IPaymentsRepository _repository;

    public GetPaymentByBookingIdUseCase(IPaymentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Payment> ExecuteAsync(
        int bookingId,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByBookingIdAsync(BookingId.Create(bookingId));
        if (result is null)
            throw new KeyNotFoundException($"Payment with booking id '{bookingId}' was not found.");

        return result;
    }
}