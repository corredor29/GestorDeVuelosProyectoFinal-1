using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Aggregate;

public sealed class Address
{
    // Propiedades con Backing Fields o Getters de Value Objects
    public AddressesId Id { get; private set; } = null!;
    public RoadTypeId RoadTypeId { get; private set; } = null!;
    public AddressesNameVia StreetName { get; private set; } = null!; // Antes NameVia
    public AddressesNumber Number { get; private set; } = null!;
    public AddressesComplement Complement { get; private set; } = null!;
    public AddressesPostalCode PostalCode { get; private set; } = null!;
    public CityId CityId { get; private set; } = null!;

    // Constructor vacío para EF Core
    private Address() { }

    // Constructor privado para forzar el uso de métodos de creación
    private Address(
        AddressesId id, 
        RoadTypeId roadTypeId, 
        AddressesNameVia streetName, 
        AddressesNumber number, 
        AddressesComplement complement, 
        AddressesPostalCode postalCode, 
        CityId cityId)
    {
        Id = id;
        RoadTypeId = roadTypeId;
        StreetName = streetName;
        Number = number;
        Complement = complement;
        PostalCode = postalCode;
        CityId = cityId;
    }

    public static Address Create(
        int id, 
        int roadTypeId, 
        string streetName, 
        string? number, 
        string? complement, 
        string? postalCode, 
        int cityId)
    {
        return new Address(
            AddressesId.Create(id),
            RoadTypeId.Create(roadTypeId),
            AddressesNameVia.Create(streetName),
            AddressesNumber.Create(number),
            AddressesComplement.Create(complement),
            AddressesPostalCode.Create(postalCode),
            CityId.Create(cityId)
        );
    }

    // --- Métodos de Comportamiento ---

    public void UpdateStreetDetails(int roadTypeId, string streetName, string? number)
    {
        RoadTypeId = RoadTypeId.Create(roadTypeId);
        StreetName = AddressesNameVia.Create(streetName);
        Number = AddressesNumber.Create(number);
    }

    public void UpdateLocation(int cityId, string? postalCode)
    {
        CityId = CityId.Create(cityId);
        PostalCode = AddressesPostalCode.Create(postalCode);
    }

    public void UpdateComplement(string? complement)
    {
        Complement = AddressesComplement.Create(complement);
    }
}