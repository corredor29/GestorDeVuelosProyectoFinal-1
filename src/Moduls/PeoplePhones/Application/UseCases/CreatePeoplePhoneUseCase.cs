using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.UseCases;

public sealed class CreatePeoplePhoneUseCase
{
    private readonly IPeoplePhonesRepository _repository;
    private readonly IPeopleRepository _peopleRepository;
    private readonly IPhoneCodesRepository _phoneCodesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePeoplePhoneUseCase(
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

    public async Task ExecuteAsync(int personId, int phoneCodeId, string phoneNumber, bool isPrimary)
    {
        var person = await _peopleRepository.GetByIdAsync(PeopleId.Create(personId));
        if (person is null)
            throw new InvalidOperationException($"No se encontró la persona con ID {personId}.");

        var phoneCode = await _phoneCodesRepository.GetByIdAsync(PhoneCodesId.Create(phoneCodeId));
        if (phoneCode is null)
            throw new InvalidOperationException($"No se encontró el código telefónico con ID {phoneCodeId}.");

        if (isPrimary)
        {
            var existingPrimary = await _repository.GetPrimaryByPersonIdAsync(PeopleId.Create(personId));
            if (existingPrimary is not null)
                throw new InvalidOperationException("La persona ya tiene un teléfono principal registrado.");
        }

        var personPhone = PersonPhone.Create(personId, phoneCodeId, phoneNumber, isPrimary);
        await _repository.SaveAsync(personPhone);
        await _unitOfWork.SaveChangesAsync();
    }
}
