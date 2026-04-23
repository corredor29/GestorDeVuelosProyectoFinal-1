using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.UseCases;

public sealed class GetEmailDomainsUseCase
{
    private readonly IEmailDomainRepository _repository;

    public GetEmailDomainsUseCase(IEmailDomainRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<EmailDomain>> ExecuteAsync()
        => _repository.GetAllAsync();

    public Task<EmailDomain?> GetByDomainAsync(string domain)
        => _repository.GetByDomainAsync(domain);
}
