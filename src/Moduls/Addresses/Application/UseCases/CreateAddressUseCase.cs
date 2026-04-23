using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.UseCases;

public sealed class CreateAddressUseCase
{
    private readonly IAddressRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAddressUseCase(IAddressRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int streetTypeId, string streetName, string? number, string? complement, string? postalCode, int cityId)
    {
        // El ID 0 permite que MySQL genere el AUTO_INCREMENT
        var address = Address.Create(0, streetTypeId, streetName, number, complement, postalCode, cityId);
        await _repository.SaveAsync(address);
        await _unitOfWork.SaveChangesAsync();
    }
}