using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Aggregate;

public sealed class Airline
{
    // La aerolínea combina identidad, país de origen y estado activo para soportar bajas lógicas.
    public AirlinesId? Id { get; private set; }
    public AirlinesName Name { get; private set; } = null!;
    public AirlinesIataCode IataCode { get; private set; } = null!;
    public CountryId OriginCountryId { get; private set; } = null!;
    public AirlinesIsActive IsActive { get; private set; } = null!;
    public AirlinesCreatedIn CreatedIn { get; private set; } = null!;
    public AirlinesUpdatedIn UpdatedIn { get; private set; } = null!;

    private Airline() { }

    private Airline(
        AirlinesId? id,
        AirlinesName name,
        AirlinesIataCode iataCode,
        CountryId originCountryId,
        AirlinesIsActive isActive,
        AirlinesCreatedIn createdIn,
        AirlinesUpdatedIn updatedIn)
    {
        Id = id;
        Name = name;
        IataCode = iataCode;
        OriginCountryId = originCountryId;
        IsActive = isActive;
        CreatedIn = createdIn;
        UpdatedIn = updatedIn;
    }

    public static Airline Create(string name, string iataCode, int originCountryId, bool isActive = true)
    {
        // En creación nueva todavía no existe id, así que solo se inicializan los datos de negocio.
        var now = DateTime.UtcNow;

        return new Airline(
            id: null,
            name: AirlinesName.Create(name),
            iataCode: AirlinesIataCode.Create(iataCode),
            originCountryId: CountryId.Create(originCountryId),
            isActive: AirlinesIsActive.Create(isActive),
            createdIn: AirlinesCreatedIn.Create(now),
            updatedIn: AirlinesUpdatedIn.Create(now));
    }

    public static Airline FromPrimitives(
        int id,
        string name,
        string iataCode,
        int originCountryId,
        bool isActive,
        DateTime createdIn,
        DateTime updatedIn)
    {
        // Este factory se usa cuando la entidad ya viene persistida desde la base.
        return new Airline(
            id: AirlinesId.Create(id),
            name: AirlinesName.Create(name),
            iataCode: AirlinesIataCode.Create(iataCode),
            originCountryId: CountryId.Create(originCountryId),
            isActive: AirlinesIsActive.Create(isActive),
            createdIn: AirlinesCreatedIn.Create(createdIn),
            updatedIn: AirlinesUpdatedIn.Create(updatedIn));
    }

    public void Update(string name, string iataCode, int originCountryId, bool isActive)
    {
        // Todo cambio relevante termina actualizando la marca de tiempo.
        Name = AirlinesName.Create(name);
        IataCode = AirlinesIataCode.Create(iataCode);
        OriginCountryId = CountryId.Create(originCountryId);
        IsActive = AirlinesIsActive.Create(isActive);
        Touch();
    }

    public void Deactivate()
    {
        IsActive = AirlinesIsActive.Create(false);
        Touch();
    }

    public void Reactivate()
    {
        IsActive = AirlinesIsActive.Create(true);
        Touch();
    }

    private void Touch()
    {
        UpdatedIn = AirlinesUpdatedIn.Create(DateTime.UtcNow);
    }
}
