using GestorDeVuelosProyectoFinal.Moduls.EmailDomains.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Aggregate;

public sealed class PersonEmail
{
    public PersonEmailsId Id { get; private set; } = null!;
    public int PersonId { get; private set; }
    public PersonEmailsUser UserEmail { get; private set; } = null!;
    public EmailDomainsId EmailDomainId { get; private set; } = null!;
    public PersonEmailsIsPrimary IsPrimary { get; private set; } = null!;

    private PersonEmail() { }

    public static PersonEmail Create(int personId, string userEmail, int emailDomainId, bool isPrimary)
    {
        return new PersonEmail
        {
            Id = PersonEmailsId.Create(0),
            PersonId = personId,
            UserEmail = PersonEmailsUser.Create(userEmail),
            EmailDomainId = EmailDomainsId.Create(emailDomainId),
            IsPrimary = PersonEmailsIsPrimary.Create(isPrimary)
        };
    }

    public static PersonEmail FromPrimitives(int id, int personId, string userEmail, int emailDomainId, bool isPrimary)
    {
        return new PersonEmail
        {
            Id = PersonEmailsId.Create(id),
            PersonId = personId,
            UserEmail = PersonEmailsUser.Create(userEmail),
            EmailDomainId = EmailDomainsId.Create(emailDomainId),
            IsPrimary = PersonEmailsIsPrimary.Create(isPrimary)
        };
    }

    public void Update(int personId, string userEmail, int emailDomainId, bool isPrimary)
    {
        PersonId = personId;
        UserEmail = PersonEmailsUser.Create(userEmail);
        EmailDomainId = EmailDomainsId.Create(emailDomainId);
        IsPrimary = PersonEmailsIsPrimary.Create(isPrimary);
    }
}
