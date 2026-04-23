using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.Interfaces;

public interface IEmailDomainService
{
    Task<IEnumerable<EmailDomain>> GetAllAsync();
    Task<EmailDomain?> GetByDomainAsync(string domain);
    Task CreateAsync(string domain);
    Task UpdateAsync(string currentDomain, string newDomain);
    Task DeleteAsync(string domain);
}
