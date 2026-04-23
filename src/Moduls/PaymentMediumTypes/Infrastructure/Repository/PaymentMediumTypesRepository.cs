using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Aggregate;
namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Infrastructure.Repository;

public sealed class PaymentMediumTypesRepository : IPaymentMediumTypesRepository
{
    private readonly AppDbContext _context;

    public PaymentMediumTypesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentMediumType?> GetByIdAsync(PaymentMediumTypesId id)
    {
        var entity = await _context.Set<PaymentMediumTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PaymentMediumType?> GetByNameAsync(PaymentMediumTypesName name)
    {
        var entity = await _context.Set<PaymentMediumTypesEntity>()
            .FirstOrDefaultAsync(x => x.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<PaymentMediumType>> GetAllAsync()
    {
        var entities = await _context.Set<PaymentMediumTypesEntity>().ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(PaymentMediumType paymentMediumTypes)
    {
        var entity = MapToEntity(paymentMediumTypes);
        await _context.Set<PaymentMediumTypesEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PaymentMediumType paymentMediumTypes)
    {
        var entity = await _context.Set<PaymentMediumTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == paymentMediumTypes.Id.Value);

        if (entity is null) return;

        entity.Name = paymentMediumTypes.Name.Value;

        _context.Set<PaymentMediumTypesEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PaymentMediumTypesId id)
    {
        var entity = await _context.Set<PaymentMediumTypesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<PaymentMediumTypesEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }
    private static PaymentMediumType MapToDomain(PaymentMediumTypesEntity entity)
        => PaymentMediumType.Create(entity.Id, entity.Name);

    private static PaymentMediumTypesEntity MapToEntity(PaymentMediumType domain)
        => new() { Name = domain.Name.Value };
}