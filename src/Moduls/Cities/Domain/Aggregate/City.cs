using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Aggregate;

public sealed class City
{
    // La ciudad es simple, pero sigue dependiendo de la región para quedar bien ubicada.
    public CityId Id { get; private set; } = null!;
    public CityName Name { get; private set; } = null!;
    public RegionId RegionId { get; private set; } = null!;

    // EF usa este constructor cuando materializa filas desde la base.
    private City() { }

    // El constructor real queda privado para que toda creación pase por el mismo punto.
    private City(CityId id, CityName name, RegionId regionId)
    {
        Id = id;
        Name = name;
        RegionId = regionId;
    }

    public static City Create(int id, string name, int regionId)
    {
        return new City(
            CityId.Create(id),
            CityName.Create(name),
            RegionId.Create(regionId)
        );
    }

    public void UpdateName(string newName)
    {
        Name = CityName.Create(newName);
    }

    public void UpdateRegion(int newRegionId)
    {
        RegionId = RegionId.Create(newRegionId);
    }
}
