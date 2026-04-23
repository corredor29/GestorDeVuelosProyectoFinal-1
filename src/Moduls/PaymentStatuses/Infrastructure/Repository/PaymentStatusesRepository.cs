using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using DomainAggregate = GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Aggregate.PaymentStatuse;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Infrastructure.Repository;

public sealed class PaymentStatusesRepository : IPaymentStatusesRepository
{
    private readonly AppDbContext _context;

    public PaymentStatusesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DomainAggregate?> GetByIdAsync(PaymentStatuseId id)
    {
        var entity = await _context.Set<PaymentStatusesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<DomainAggregate>> GetAllAsync()
    {
        var entities = await _context.Set<PaymentStatusesEntity>().ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(DomainAggregate paymentStatuses)
    {
        var entity = MapToEntity(paymentStatuses);
        await _context.Set<PaymentStatusesEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DomainAggregate paymentStatuses)
    {
        var entity = await _context.Set<PaymentStatusesEntity>()
            .FirstOrDefaultAsync(x => x.Id == paymentStatuses.Id.Value);

        if (entity is null) return;

        entity.Name = paymentStatuses.Name.Value;

        _context.Set<PaymentStatusesEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PaymentStatuseId id)
    {
        var entity = await _context.Set<PaymentStatusesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<PaymentStatusesEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }


    private static DomainAggregate MapToDomain(PaymentStatusesEntity entity)
        => DomainAggregate.Create(entity.Id, entity.Name);

    private static PaymentStatusesEntity MapToEntity(DomainAggregate domain)
        => new() { Name = domain.Name.Value };
}