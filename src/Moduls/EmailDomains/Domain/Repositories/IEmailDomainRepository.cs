using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Repositories;

public interface IEmailDomainRepository
{
    Task<EmailDomain?> GetByIdAsync(EmailDomainsId id);
    Task<EmailDomain?> GetByDomainAsync(EmailDomainName domain);
    Task<EmailDomain?> GetByDomainAsync(string domain);
    Task<IEnumerable<EmailDomain>> GetAllAsync();
    Task SaveAsync(EmailDomain emailDomain);
    Task UpdateAsync(EmailDomain emailDomain);
    Task DeleteAsync(EmailDomainsId id);
    Task DeleteByDomainAsync(string domain);
}
