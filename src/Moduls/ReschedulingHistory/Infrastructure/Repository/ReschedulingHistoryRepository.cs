using GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Infrastructure.Repository;

// implementa la interfaz IReschedulingHistoryRepository

public sealed class ReschedulingHistoryRepository : IReschedulingHistoryRepository
{
    // da contxto a la entidad
    private readonly AppDbContext _context;
    //  hace un inyeccion de dependencias

    public ReschedulingHistoryRepository(AppDbContext context)
    {
        _context = context;
    }
    // busca en la base de datos los cambios de buelo de una reserva los ordena por facha y los convierte a objetos de dominio 
    public async Task<IEnumerable<ReschedulingHistor>> GetByBookingIdAsync(int bookingId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.ReschedulingHistories
            .AsNoTracking()
            .Where(x => x.BookingId == bookingId)
            .OrderByDescending(x => x.ChangedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }
    // Obtiene todo el historial de reprogramaciones de la base de datos, lo ordena por fecha y lo convierte a objetos del dominio.
    public async Task<IEnumerable<ReschedulingHistor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.ReschedulingHistories
            .AsNoTracking()
            .OrderByDescending(x => x.ChangedAt)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }
    // Obtiene todo el historial de reprogramaciones de la base de datos, lo ordena por fecha y lo convierte a objetos del dominio.
    public async Task SaveAsync(ReschedulingHistor histor, CancellationToken cancellationToken = default)
    {
        await _context.ReschedulingHistories.AddAsync(MapToEntity(histor), cancellationToken);
    }
    // Convierte una entidad de infraestructura 
    // en un objeto del dominio usando el método FromPrimitives.
    // Permite desacoplar la lógica de negocio de la capa de persistencia.
    private static ReschedulingHistor MapToDomain(ReschedulingHistoryEntity e)
        => ReschedulingHistor.FromPrimitives(e.Id, e.BookingId, e.PreviousFlightId, e.NewFlightId, e.ChangedAt, e.Reason);
    // Traduce tu modelo de negocio a un formato que la base de datos puede persistir
    private static ReschedulingHistoryEntity MapToEntity(ReschedulingHistor h)
        => new()
        {
            BookingId        = h.BookingId,
            PreviousFlightId = h.PreviousFlightId,
            NewFlightId      = h.NewFlightId,
            ChangedAt        = h.ChangedAt,
            Reason           = h.Reason
        };
}