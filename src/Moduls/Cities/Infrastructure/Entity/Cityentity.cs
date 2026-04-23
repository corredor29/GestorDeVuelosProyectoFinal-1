using GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;

public sealed class CityEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int RegionId { get; set; }
    public int RegionId1 { get; set; }
    public RegionEntity Region { get; set; } = null!;
    public  ICollection<AddressEntity> Addresses { get; set; } = new List<AddressEntity>();
    public  ICollection<AirportEntity> Airports { get; set; } = new List<AirportEntity>();


}
