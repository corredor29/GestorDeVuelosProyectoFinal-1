using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Infrastructure.Persistence.seeders;

public sealed class EmailDomainsSeeder
{
    private readonly IEmailDomainRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    private static readonly string[] Domains =
    {
        "gmail.com",
        "outlook.com",
        "hotmail.com",
        "yahoo.com",
        "icloud.com"
    };

    public EmailDomainsSeeder(IEmailDomainRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        var existing = await _repository.GetAllAsync();
        var existingDomains = existing
            .Select(x => x.Domain.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var domain in Domains)
        {
            if (existingDomains.Contains(domain))
                continue;

            await _repository.SaveAsync(EmailDomain.Create(domain));
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
