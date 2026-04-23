using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Aggregate;

public sealed class Airport
{
    // El aeropuerto depende de la ciudad y además conserva IATA/ICAO como identificadores operativos.
    public AirportsId? Id { get; private set; }
    public AirportsName Name { get; private set; } = null!;
    public AirportsIataCode IataCode { get; private set; } = null!;
    public AirportsIcaoCode IcaoCode { get; private set; } = null!;
    public CityId CityId { get; private set; } = null!;

    private Airport() { }

    private Airport(
        AirportsId? id,
        AirportsName name,
        AirportsIataCode iataCode,
        AirportsIcaoCode icaoCode,
        CityId cityId)
    {
        Id = id;
        Name = name;
        IataCode = iataCode;
        IcaoCode = icaoCode;
        CityId = cityId;
    }

    public static Airport Create(string name, string iataCode, string? icaoCode, int cityId)
    {
        // En alta nueva se aceptan datos opcionales como ICAO, pero igual pasan por validación.
        return new Airport(
            id: null,
            name: AirportsName.Create(name),
            iataCode: AirportsIataCode.Create(iataCode),
            icaoCode: AirportsIcaoCode.Create(icaoCode),
            cityId: CityId.Create(cityId));
    }

    public static Airport FromPrimitives(int id, string name, string iataCode, string? icaoCode, int cityId)
    {
        return new Airport(
            id: AirportsId.Create(id),
            name: AirportsName.Create(name),
            iataCode: AirportsIataCode.Create(iataCode),
            icaoCode: AirportsIcaoCode.Create(icaoCode),
            cityId: CityId.Create(cityId));
    }

    public void Update(string name, string iataCode, string? icaoCode, int cityId)
    {
        Name = AirportsName.Create(name);
        IataCode = AirportsIataCode.Create(iataCode);
        IcaoCode = AirportsIcaoCode.Create(icaoCode);
        CityId = CityId.Create(cityId);
    }
}
