using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.UseCases;

public sealed class DeletePeoplePhoneUseCase
{
    private readonly IPeoplePhonesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePeoplePhoneUseCase(IPeoplePhonesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var phoneId = PersonPhonesId.Create(id);
        var existing = await _repository.GetByIdAsync(phoneId);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el teléfono de persona con ID {id}.");

        await _repository.DeleteAsync(phoneId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> ExecuteByPhoneNumberAsync(string phoneNumber)
    {
        var deleted = await _repository.DeleteByPhoneNumberAsync(PersonPhonesPhoneNumber.Create(phoneNumber));
        if (deleted == 0)
            throw new InvalidOperationException("No se encontraron teléfonos con ese número.");

        await _unitOfWork.SaveChangesAsync();
        return deleted;
    }

    public async Task<int> ExecuteByPhoneCodeIdAsync(int phoneCodeId)
    {
        var deleted = await _repository.DeleteByPhoneCodeIdAsync(PhoneCodesId.Create(phoneCodeId));
        if (deleted == 0)
            throw new InvalidOperationException("No se encontraron teléfonos con ese código telefónico.");

        await _unitOfWork.SaveChangesAsync();
        return deleted;
    }

    public async Task<int> ExecuteByPersonNameAsync(string personName)
    {
        var deleted = await _repository.DeleteByPersonNameAsync(personName);
        if (deleted == 0)
            throw new InvalidOperationException("No se encontraron teléfonos para personas con ese nombre.");

        await _unitOfWork.SaveChangesAsync();
        return deleted;
    }
}
