using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Repositories;

public interface IAddressRepository
{
    Task SaveAsync(Address address, CancellationToken cancellationToken = default);
    Task UpdateAsync(Address address, CancellationToken cancellationToken = default);
    Task<Address?> GetByIdAsync(AddressesId id, CancellationToken cancellationToken = default);
    Task<Address?> GetByStreetAndNumberAsync(string streetName, string? number, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Address>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Address>> GetByCityAsync(CityId cityId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Address>> GetByStreetTypeAsync(int streetTypeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Address>> GetByPostalCodeAsync(AddressesPostalCode postalCode, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(AddressesId id, CancellationToken cancellationToken = default);
    Task<bool> DeleteByStreetAndNumberAsync(string streetName, string? number, CancellationToken cancellationToken = default);
}