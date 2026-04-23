using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.Interfaces;

public interface IAddressService
{
    Task<IEnumerable<Address>> GetAllAsync();
    Task<Address?> GetByIdAsync(int id);
    Task<Address?> GetByStreetAndNumberAsync(string street, string? number);
    Task CreateAsync(int streetTypeId, string streetName, string? number, string? complement, string? postalCode, int cityId);
    Task UpdateAsync(int id, int streetTypeId, string streetName, string? number, string? complement, string? postalCode, int cityId);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteByStreetAndNumberAsync(string street, string? number);
}