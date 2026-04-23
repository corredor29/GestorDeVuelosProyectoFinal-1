using GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.Services;

// Servicio del módulo de direcciones.
// Aquí combinamos repositorio, casos de uso y unit of work para el CRUD principal.
public sealed class AddressService : IAddressService
{
    private readonly IAddressRepository _repository;
    private readonly CreateAddressUseCase _createUseCase;
    private readonly UpdateAddressUseCase _updateUseCase;
    private readonly IUnitOfWork _unitOfWork;

    public AddressService(
        IAddressRepository repository, 
        CreateAddressUseCase createUseCase, 
        UpdateAddressUseCase updateUseCase, 
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Address>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Address?> GetByIdAsync(int id) => 
        await _repository.GetByIdAsync(AddressesId.Create(id));

    public async Task<Address?> GetByStreetAndNumberAsync(string street, string? number) => 
        await _repository.GetByStreetAndNumberAsync(street, number);

    public async Task CreateAsync(int streetTypeId, string streetName, string? number, string? complement, string? postalCode, int cityId) => 
        await _createUseCase.ExecuteAsync(streetTypeId, streetName, number, complement, postalCode, cityId);

    public async Task UpdateAsync(int id, int streetTypeId, string streetName, string? number, string? complement, string? postalCode, int cityId) => 
        await _updateUseCase.ExecuteAsync(id, streetTypeId, streetName, number, complement, postalCode, cityId);

    public async Task<bool> DeleteAsync(int id)
    {
        var success = await _repository.DeleteAsync(AddressesId.Create(id));
        // Solo confirmamos cambios si realmente se eliminó algo.
        if (success) await _unitOfWork.SaveChangesAsync();
        return success;
    }

    public async Task<bool> DeleteByStreetAndNumberAsync(string street, string? number)
    {
        var success = await _repository.DeleteByStreetAndNumberAsync(street, number);
        if (success) await _unitOfWork.SaveChangesAsync();
        return success;
    }
}
