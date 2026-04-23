using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Repositories;

public interface IPaymentsRepository
{
    Task<Aggregate.Payment?> GetByIdAsync(PaymentsId id);
    Task<IEnumerable<Aggregate.Payment>> GetAllAsync();
    Task SaveAsync(Aggregate.Payment payments);
    Task DeleteAsync(PaymentsId id);
    Task<Payment?> GetByBookingIdAsync(BookingId bookingId);
    Task UpdateAsync(Payment payment);
}
