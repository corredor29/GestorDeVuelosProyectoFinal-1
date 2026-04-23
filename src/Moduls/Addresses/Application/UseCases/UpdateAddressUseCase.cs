using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.UseCases;

public sealed class UpdateAddressUseCase
{
    private readonly IAddressRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAddressUseCase(IAddressRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, int streetTypeId, string streetName, string? number, string? complement, string? postalCode, int cityId)
    {
        var existing = await _repository.GetByIdAsync(AddressesId.Create(id));
        
        if (existing == null) 
            throw new KeyNotFoundException($"La dirección con ID {id} no existe.");

        // Usamos los métodos de actualización del Agregado que definimos antes
        existing.UpdateStreetDetails(streetTypeId, streetName, number);
        existing.UpdateLocation(cityId, postalCode);
        existing.UpdateComplement(complement);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync();
    }
}