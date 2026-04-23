using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Repository;

public sealed class PaymentMethodsRepository : IPaymentMethodsRepository
{
    private readonly AppDbContext _context;

    public PaymentMethodsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentMethod?> GetByIdAsync(PaymentMethodsId id)
    {
        var entity = await _context.Set<PaymentMethodsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        var entities = await _context.Set<PaymentMethodsEntity>()
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(PaymentMethod paymentMethod)
    {
        var entity = MapToEntity(paymentMethod);
        await _context.Set<PaymentMethodsEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        paymentMethod.SetId(entity.Id);
    }

    public async Task UpdateAsync(PaymentMethod paymentMethod)
    {
        var entity = await _context.Set<PaymentMethodsEntity>()
            .FirstOrDefaultAsync(x => x.Id == paymentMethod.Id.Value);

        if (entity is null) return;

        entity.DisplayName = paymentMethod.DisplayName.Value;

        _context.Set<PaymentMethodsEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PaymentMethodsId id)
    {
        var entity = await _context.Set<PaymentMethodsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<PaymentMethodsEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    // ── Mappers ──────────────────────────────────────────────────────────────

    private static PaymentMethod MapToDomain(PaymentMethodsEntity entity)
    {
        return PaymentMethod.Create(entity.Id, entity.DisplayName);
    }

    private static PaymentMethodsEntity MapToEntity(PaymentMethod domain)
    {
        return new PaymentMethodsEntity
        {
            DisplayName = domain.DisplayName.Value
        };
    }
}