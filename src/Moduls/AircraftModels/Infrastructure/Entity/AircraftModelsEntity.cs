using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Infrastructure.Entity;

public sealed class AircraftModelsEntity
{
    public int Id { get; set; }
    public int AircraftManufacturerId { get; set; }
    public string ModelName { get; set; } = null!;
    public int MaxCapacity { get; set; }
    public decimal? MaxTakeoffWeightKg { get; set; }
    public decimal? FuelConsumptionKgH { get; set; }
    public int? CruiseSpeedKmh { get; set; }
    public int? CruiseAltitudeFt { get; set; }

    public AircraftManufacturerEntity? Manufacturer { get; set; }
}
