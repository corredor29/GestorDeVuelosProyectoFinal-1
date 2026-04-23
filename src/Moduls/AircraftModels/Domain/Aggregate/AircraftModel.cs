using GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;

public sealed class AircraftModel
{
    public AircraftModelId Id { get; private set; } = null!;
    public AircraftManufacturersId ManufacturerId { get; private set; } = null!;
    public AircraftModelName ModelName { get; private set; } = null!;
    public AircraftModelMaxCapacity MaxCapacity { get; private set; } = null!;
    public AircraftModelWeight MaxTakeoffWeight { get; private set; } = null!;
    public AircraftModelFuelConsumption FuelConsumption { get; private set; } = null!;
    public AircraftModelCruiseSpeed CruiseSpeed { get; private set; } = null!;
    public AircraftModelCruiseAltitude CruiseAltitude { get; private set; } = null!;

    private AircraftModel() { }

    private AircraftModel(
        AircraftModelId id,
        AircraftManufacturersId manufacturerId,
        AircraftModelName modelName,
        AircraftModelMaxCapacity maxCapacity,
        AircraftModelWeight maxTakeoffWeight,
        AircraftModelFuelConsumption fuelConsumption,
        AircraftModelCruiseSpeed cruiseSpeed,
        AircraftModelCruiseAltitude cruiseAltitude)
    {
        Id = id;
        ManufacturerId = manufacturerId;
        ModelName = modelName;
        MaxCapacity = maxCapacity;
        MaxTakeoffWeight = maxTakeoffWeight;
        FuelConsumption = fuelConsumption;
        CruiseSpeed = cruiseSpeed;
        CruiseAltitude = cruiseAltitude;
    }

    public static AircraftModel Create(
        int manufacturerId,
        string modelName,
        int maxCapacity,
        decimal? maxTakeoffWeight,
        decimal? fuelConsumption,
        int? cruiseSpeed,
        int? cruiseAltitude)
    {
        return new AircraftModel
        {
            ManufacturerId = AircraftManufacturersId.Create(manufacturerId),
            ModelName = AircraftModelName.Create(modelName),
            MaxCapacity = AircraftModelMaxCapacity.Create(maxCapacity),
            MaxTakeoffWeight = AircraftModelWeight.Create(maxTakeoffWeight),
            FuelConsumption = AircraftModelFuelConsumption.Create(fuelConsumption),
            CruiseSpeed = AircraftModelCruiseSpeed.Create(cruiseSpeed),
            CruiseAltitude = AircraftModelCruiseAltitude.Create(cruiseAltitude)
        };
    }

    public static AircraftModel FromPrimitives(
        int id,
        int manufacturerId,
        string modelName,
        int maxCapacity,
        decimal? maxTakeoffWeight,
        decimal? fuelConsumption,
        int? cruiseSpeed,
        int? cruiseAltitude)
    {
        return new AircraftModel(
            AircraftModelId.Create(id),
            AircraftManufacturersId.Create(manufacturerId),
            AircraftModelName.Create(modelName),
            AircraftModelMaxCapacity.Create(maxCapacity),
            AircraftModelWeight.Create(maxTakeoffWeight),
            AircraftModelFuelConsumption.Create(fuelConsumption),
            AircraftModelCruiseSpeed.Create(cruiseSpeed),
            AircraftModelCruiseAltitude.Create(cruiseAltitude));
    }

    public void Update(
        int manufacturerId,
        string modelName,
        int maxCapacity,
        decimal? maxTakeoffWeight,
        decimal? fuelConsumption,
        int? cruiseSpeed,
        int? cruiseAltitude)
    {
        ManufacturerId = AircraftManufacturersId.Create(manufacturerId);
        ModelName = AircraftModelName.Create(modelName);
        MaxCapacity = AircraftModelMaxCapacity.Create(maxCapacity);
        MaxTakeoffWeight = AircraftModelWeight.Create(maxTakeoffWeight);
        FuelConsumption = AircraftModelFuelConsumption.Create(fuelConsumption);
        CruiseSpeed = AircraftModelCruiseSpeed.Create(cruiseSpeed);
        CruiseAltitude = AircraftModelCruiseAltitude.Create(cruiseAltitude);
    }
}
