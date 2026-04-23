using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.UseCases;

public sealed class DeleteAddressUseCase
{
    private readonly IAddressRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAddressUseCase(IAddressRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteByIdAsync(int id)
    {
        var success = await _repository.DeleteAsync(AddressesId.Create(id));
        if (success) await _unitOfWork.SaveChangesAsync();
        return success;
    }

    public async Task<bool> ExecuteByStreetAsync(string street, string? number)
    {
        var success = await _repository.DeleteByStreetAndNumberAsync(street, number);
        if (success) await _unitOfWork.SaveChangesAsync();
        return success;
    }
}