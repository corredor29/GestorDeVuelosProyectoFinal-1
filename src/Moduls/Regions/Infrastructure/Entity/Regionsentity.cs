using GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Entities;

public sealed class RegionEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int CountryId { get; set; }
    public CountryEntity Country { get; set; } = null!;
    public  ICollection<CityEntity> Cities { get; set; } = new List<CityEntity>();

}