using System;
using GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Domain.Repositories;
// interface q define las operaciones para el acceso a datos definimos contratos 
public interface IReschedulingHistoryRepository
{
    // permite obtener el historial de reporgramciones de las reservas
    Task<IEnumerable<ReschedulingHistor>> GetByBookingIdAsync(int bookingId, CancellationToken cancellationToken= default);
    // metodo que obtiene todos los registros de la entidad 
    Task<IEnumerable<ReschedulingHistor>> GetAllAsync(CancellationToken cancellationToken = default);
    // metodo que se encarga de crear un nuevo registro o actualizando
    Task SaveAsync (ReschedulingHistor histor, CancellationToken cancellationToken = default);
}
