using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Infrastructure.Repository;

public sealed class EmailDomainsRepository : IEmailDomainRepository
{
    private readonly AppDbContext _context;

    public EmailDomainsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EmailDomain?> GetByIdAsync(EmailDomainsId id)
    {
        var entity = await _context.EmailDomains
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public Task<EmailDomain?> GetByDomainAsync(EmailDomainName domain)
        => GetByDomainAsync(domain.Value);

    public async Task<EmailDomain?> GetByDomainAsync(string domain)
    {
        var normalized = EmailDomainName.Create(domain).Value;

        var entity = await _context.EmailDomains
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Domain == normalized);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<EmailDomain>> GetAllAsync()
    {
        var entities = await _context.EmailDomains
            .AsNoTracking()
            .OrderBy(x => x.Domain)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(EmailDomain emailDomain)
    {
        await _context.EmailDomains.AddAsync(MapToEntity(emailDomain));
    }

    public async Task UpdateAsync(EmailDomain emailDomain)
    {
        var entity = await _context.EmailDomains
            .FirstOrDefaultAsync(x => x.Id == emailDomain.Id.Value);

        if (entity is null)
            throw new InvalidOperationException($"Email domain with id '{emailDomain.Id.Value}' not found.");

        entity.Domain = emailDomain.Domain.Value;
    }

    public async Task DeleteAsync(EmailDomainsId id)
    {
        var entity = await _context.EmailDomains
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null)
            throw new InvalidOperationException($"Email domain with id '{id.Value}' not found.");

        _context.EmailDomains.Remove(entity);
    }

    public async Task DeleteByDomainAsync(string domain)
    {
        var normalized = EmailDomainName.Create(domain).Value;

        var entity = await _context.EmailDomains
            .FirstOrDefaultAsync(x => x.Domain == normalized);

        if (entity is null)
            throw new InvalidOperationException($"Email domain '{normalized}' not found.");

        _context.EmailDomains.Remove(entity);
    }

    private static EmailDomain MapToDomain(EmailDomainsEntity entity)
    {
        return EmailDomain.FromPrimitives(entity.Id, entity.Domain);
    }

    private static EmailDomainsEntity MapToEntity(EmailDomain emailDomain)
    {
        return new EmailDomainsEntity
        {
            Id = emailDomain.Id.Value,
            Domain = emailDomain.Domain.Value
        };
    }
}
