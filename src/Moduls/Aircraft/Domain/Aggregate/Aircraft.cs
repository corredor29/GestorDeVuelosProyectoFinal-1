using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate;

public sealed class Aircraft
{
    public AircraftId Id { get; private set; } = null!;
    public AircraftModelId ModelId { get; private set; } = null!;
    public AirlinesId AirlineId { get; private set; } = null!;
    public AircraftRegistration Registration { get; private set; } = null!;
    public AircraftDateManufacture ManufacturedDate { get; private set; } = null!;
    public AircraftActive IsActive { get; private set; } = null!;

    private Aircraft() { }

    private Aircraft(
        AircraftId id,
        AircraftModelId modelId,
        AirlinesId airlineId,
        AircraftRegistration registration,
        AircraftDateManufacture manufacturedDate,
        AircraftActive isActive)
    {
        Id = id;
        ModelId = modelId;
        AirlineId = airlineId;
        Registration = registration;
        ManufacturedDate = manufacturedDate;
        IsActive = isActive;
    }

    public static Aircraft Create(
        int modelId,
        int airlineId,
        string registration,
        DateTime? manufacturedDate,
        bool isActive)
    {
        return new Aircraft
        {
            ModelId = AircraftModelId.Create(modelId),
            AirlineId = AirlinesId.Create(airlineId),
            Registration = AircraftRegistration.Create(registration),
            ManufacturedDate = AircraftDateManufacture.Create(manufacturedDate),
            IsActive = AircraftActive.Create(isActive)
        };
    }

    public static Aircraft FromPrimitives(
        int id,
        int modelId,
        int airlineId,
        string registration,
        DateTime? manufacturedDate,
        bool isActive)
    {
        return new Aircraft(
            AircraftId.Create(id),
            AircraftModelId.Create(modelId),
            AirlinesId.Create(airlineId),
            AircraftRegistration.Create(registration),
            AircraftDateManufacture.Create(manufacturedDate),
            AircraftActive.Create(isActive));
    }

    public void Update(
        int modelId,
        int airlineId,
        string registration,
        DateTime? manufacturedDate,
        bool isActive)
    {
        ModelId = AircraftModelId.Create(modelId);
        AirlineId = AirlinesId.Create(airlineId);
        Registration = AircraftRegistration.Create(registration);
        ManufacturedDate = AircraftDateManufacture.Create(manufacturedDate);
        IsActive = AircraftActive.Create(isActive);
    }

    public void Deactivate()
    {
        IsActive = AircraftActive.Create(false);
    }
}
