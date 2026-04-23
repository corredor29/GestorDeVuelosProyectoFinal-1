using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Infrastructure.Entity;

public sealed class AircraftManufacturerEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;

    public ICollection<AircraftModelsEntity> Models { get; set; } = new List<AircraftModelsEntity>();
}
