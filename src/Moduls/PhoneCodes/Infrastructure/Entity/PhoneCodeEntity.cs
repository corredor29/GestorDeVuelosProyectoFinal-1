using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Persistence.Entities;

public sealed class PhoneCodeEntity
{
    public int Id { get; set; }
    public string CountryCode { get; set; } = null!;
    public string CountryName { get; set; } = null!;
    public  ICollection<PeoplePhoneEntity> PeoplePhone { get; set; } = new List<PeoplePhoneEntity>();

}
