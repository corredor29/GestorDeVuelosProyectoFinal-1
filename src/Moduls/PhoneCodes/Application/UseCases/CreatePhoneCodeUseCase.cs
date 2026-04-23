using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.UseCases;

public sealed class CreatePhoneCodeUseCase
{
    private readonly IPhoneCodesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePhoneCodeUseCase(IPhoneCodesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string countryCode, string countryName)
    {
        var normalizedCode = PhoneCodesCountryCode.Create(countryCode);
        var normalizedName = PhoneCodesCountryName.Create(countryName);

        var duplicateByCode = await _repository.GetByCountryCodeAsync(normalizedCode);
        if (duplicateByCode is not null)
            throw new InvalidOperationException($"Ya existe un prefijo con el código '{normalizedCode.Value}'.");

        var duplicateByName = await _repository.GetByCountryNameAsync(normalizedName);
        if (duplicateByName is not null)
            throw new InvalidOperationException($"Ya existe un prefijo para el país '{normalizedName.Value}'.");

        var phoneCode = PhoneCode.Create(normalizedCode.Value, normalizedName.Value);
        await _repository.SaveAsync(phoneCode);
        await _unitOfWork.SaveChangesAsync();
    }
}
