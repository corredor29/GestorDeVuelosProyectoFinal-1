using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.UseCases;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.Services;

public sealed class EmailDomainsService : IEmailDomainService
{
    private readonly GetEmailDomainsUseCase _getAll;
    private readonly CreateEmailDomainUseCase _create;
    private readonly UpdateEmailDomainUseCase _update;
    private readonly DeleteEmailDomainUseCase _delete;

    public EmailDomainsService(
        GetEmailDomainsUseCase getAll,
        CreateEmailDomainUseCase create,
        UpdateEmailDomainUseCase update,
        DeleteEmailDomainUseCase delete)
    {
        _getAll = getAll;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IEnumerable<EmailDomain>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<EmailDomain?> GetByDomainAsync(string domain)
        => _getAll.GetByDomainAsync(domain);

    public Task CreateAsync(string domain)
        => _create.ExecuteAsync(domain);

    public Task UpdateAsync(string currentDomain, string newDomain)
        => _update.ExecuteAsync(currentDomain, newDomain);

    public Task DeleteAsync(string domain)
        => _delete.ExecuteAsync(domain);
}
