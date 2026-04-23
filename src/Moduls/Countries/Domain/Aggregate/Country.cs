using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Aggregate;

public sealed class Country
{
    // El agregado guarda tanto la identidad propia como la referencia al continente al que pertenece.
    public CountryId Id { get; private set; } = null!;
    public CountryName Name { get; private set; } = null!;
    public CountryIsoCode IsoCode { get; private set; } = null!;
    public ContinentsId ContinentId { get; private set; } = null!;

    private Country() { }

    private Country(CountryId id, CountryName name, CountryIsoCode isoCode, ContinentsId continentId)
    {
        Id = id;
        Name = name;
        IsoCode = isoCode;
        ContinentId = continentId;
    }

    public static Country Create(int id, string name, string isoCode, int continentId)
    {
        // El factory deja toda la validación fuerte en los value objects.
        return new Country(
            CountryId.Create(id),
            CountryName.Create(name),
            CountryIsoCode.Create(isoCode),
            ContinentsId.Create(continentId)  
        );
    }

    public void UpdateName(string newName)
    {
        Name = CountryName.Create(newName);
    }

    public void UpdateIsoCode(string newIsoCode)
    {
        IsoCode = CountryIsoCode.Create(newIsoCode);
    }

    public void UpdateContinent(int newContinentId)
    {
        // Mantener el continente como VO evita ids negativos o vacíos metidos a mano.
        ContinentId = ContinentsId.Create(newContinentId);
    }
}
