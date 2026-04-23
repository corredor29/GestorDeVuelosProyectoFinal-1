using GestorDeVuelosProyectoFinal.Moduls.Continents.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Entities;

public sealed class CountryEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string IsoCode { get; set; } = null!;
    public int ContinentId { get; set; }
    public ContinentEntity Continent { get; set; } = null!;
    public  ICollection<RegionEntity> Regions { get; set; } = new List<RegionEntity>();
    public  ICollection<CountryEntity> Countries { get; set; } = new List<CountryEntity>();

}