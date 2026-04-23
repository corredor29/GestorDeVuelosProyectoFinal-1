using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Infrastructure.Entity;

public sealed class PersonEmailEntity
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string EmailUser { get; set; } = string.Empty;
    public int EmailDomainId { get; set; }
    public bool IsPrimary { get; set; }
    public PersonEntity Person { get; set; } = null!;
    public EmailDomainsEntity EmailDomain { get; set; } = null!;

}
