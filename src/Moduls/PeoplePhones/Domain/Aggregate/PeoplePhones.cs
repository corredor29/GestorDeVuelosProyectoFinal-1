using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Aggregate;

public sealed class PersonPhone
{
    public PersonPhonesId Id { get; private set; } = null!;
    public PeopleId PersonId { get; private set; } = null!;
    public PhoneCodesId PhoneCodeId { get; private set; } = null!;
    public PersonPhonesPhoneNumber PhoneNumber { get; private set; } = null!;
    public PersonPhonesIsPrimary IsPrimary { get; private set; } = null!;

    private PersonPhone() { }

    private PersonPhone(
        PersonPhonesId id,
        PeopleId personId,
        PhoneCodesId phoneCodeId,
        PersonPhonesPhoneNumber phoneNumber,
        PersonPhonesIsPrimary isPrimary)
    {
        Id = id;
        PersonId = personId;
        PhoneCodeId = phoneCodeId;
        PhoneNumber = phoneNumber;
        IsPrimary = isPrimary;
    }

    public static PersonPhone Create(int personId, int phoneCodeId, string phoneNumber, bool isPrimary)
    {
        return new PersonPhone
        {
            PersonId = PeopleId.Create(personId),
            PhoneCodeId = PhoneCodesId.Create(phoneCodeId),
            PhoneNumber = PersonPhonesPhoneNumber.Create(phoneNumber),
            IsPrimary = PersonPhonesIsPrimary.Create(isPrimary)
        };
    }

    public static PersonPhone FromPrimitives(int id, int personId, int phoneCodeId, string phoneNumber, bool isPrimary)
    {
        return new PersonPhone(
            PersonPhonesId.Create(id),
            PeopleId.Create(personId),
            PhoneCodesId.Create(phoneCodeId),
            PersonPhonesPhoneNumber.Create(phoneNumber),
            PersonPhonesIsPrimary.Create(isPrimary)
        );
    }

    public void Update(int personId, int phoneCodeId, string phoneNumber, bool isPrimary)
    {
        PersonId = PeopleId.Create(personId);
        PhoneCodeId = PhoneCodesId.Create(phoneCodeId);
        PhoneNumber = PersonPhonesPhoneNumber.Create(phoneNumber);
        IsPrimary = PersonPhonesIsPrimary.Create(isPrimary);
    }
}
