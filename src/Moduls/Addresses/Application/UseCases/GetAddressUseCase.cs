using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.UseCases;

public sealed class GetAddressUseCase
{
    private readonly IAddressRepository _repository;

    public GetAddressUseCase(IAddressRepository repository)
    {
        _repository = repository;
    }

    public async Task<Address?> ExecuteByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(AddressesId.Create(id));
    }

    public async Task<Address?> ExecuteByStreetAndNumberAsync(string street, string? number)
    {

        return await _repository.GetByStreetAndNumberAsync(street, number);
    }
}