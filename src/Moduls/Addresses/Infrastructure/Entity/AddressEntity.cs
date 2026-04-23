using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Entities;

public sealed class AddressEntity
{
    public int Id { get; set; }
    public int StreetTypeId { get; set; }
    public string StreetName { get; set; } = null!;
    public string? Number { get; set; }
    public string? Complement { get; set; }
    public int CityId { get; set; }
    public int CityId1 { get; set; }
    public int StreetTypeId1 { get; set; }
    public string? PostalCode { get; set; }
    public StreetTypeEntity StreetType { get; set; } = null!;
    public CityEntity City { get; set; } = null!;
    public  ICollection<PersonEntity> Person { get; set; } = new List<PersonEntity>();

}
