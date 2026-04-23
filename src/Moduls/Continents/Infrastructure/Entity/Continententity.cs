using GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Infrastructure.Persistence.Entities;

public sealed class ContinentEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public  ICollection<CountryEntity> Countries { get; set; } = new List<CountryEntity>();

}