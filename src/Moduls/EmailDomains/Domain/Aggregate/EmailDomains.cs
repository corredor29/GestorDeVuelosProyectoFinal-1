using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.Aggregate;

public sealed class EmailDomain
{
    public EmailDomainsId Id { get; private set; } = null!;
    public EmailDomainName Domain { get; private set; } = null!;

    private EmailDomain() { }

    private EmailDomain(EmailDomainsId id, EmailDomainName domain)
    {
        Id = id;
        Domain = domain;
    }

    public static EmailDomain Create(string domain)
    {
        return new EmailDomain
        {
            Id = EmailDomainsId.Create(0),
            Domain = EmailDomainName.Create(domain)
        };
    }

    public static EmailDomain FromPrimitives(int id, string domain)
    {
        return new EmailDomain(
            EmailDomainsId.Create(id),
            EmailDomainName.Create(domain));
    }

    public void UpdateDomain(string newDomain)
    {
        Domain = EmailDomainName.Create(newDomain);
    }
}
