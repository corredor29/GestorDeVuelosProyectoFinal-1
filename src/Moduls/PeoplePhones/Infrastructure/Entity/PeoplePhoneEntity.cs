using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Infrastructure.Persistence.Entities;

public sealed class PeoplePhoneEntity
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int PhoneCodeId { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public PersonEntity Person { get; set; } = null!;
    public PhoneCodeEntity PhoneCode { get; set; } = null!;
}
