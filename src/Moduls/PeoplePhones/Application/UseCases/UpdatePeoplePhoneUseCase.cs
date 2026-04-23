using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.UseCases;

public sealed class UpdatePeoplePhoneUseCase
{
    private readonly IPeoplePhonesRepository _repository;
    private readonly IPeopleRepository _peopleRepository;
    private readonly IPhoneCodesRepository _phoneCodesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePeoplePhoneUseCase(
        IPeoplePhonesRepository repository,
        IPeopleRepository peopleRepository,
        IPhoneCodesRepository phoneCodesRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _peopleRepository = peopleRepository;
        _phoneCodesRepository = phoneCodesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, int personId, int phoneCodeId, string phoneNumber, bool isPrimary)
    {
        var personPhoneId = PersonPhonesId.Create(id);
        var existing = await _repository.GetByIdAsync(personPhoneId);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el teléfono de persona con ID {id}.");

        var person = await _peopleRepository.GetByIdAsync(PeopleId.Create(personId));
        if (person is null)
            throw new InvalidOperationException($"No se encontró la persona con ID {personId}.");

        var phoneCode = await _phoneCodesRepository.GetByIdAsync(PhoneCodesId.Create(phoneCodeId));
        if (phoneCode is null)
            throw new InvalidOperationException($"No se encontró el código telefónico con ID {phoneCodeId}.");

        if (isPrimary)
        {
            var existingPrimary = await _repository.GetPrimaryByPersonIdAsync(PeopleId.Create(personId));
            if (existingPrimary is not null && existingPrimary.Id.Value != id)
                throw new InvalidOperationException("La persona ya tiene otro teléfono marcado como principal.");
        }

        existing.Update(personId, phoneCodeId, phoneNumber, isPrimary);
        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync();
    }
}
