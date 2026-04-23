using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.UseCases;

public sealed class UpdatePhoneCodeUseCase
{
    private readonly IPhoneCodesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePhoneCodeUseCase(IPhoneCodesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string countryCode, string countryName)
    {
        var phoneCodeId = PhoneCodesId.Create(id);
        var normalizedCode = PhoneCodesCountryCode.Create(countryCode);
        var normalizedName = PhoneCodesCountryName.Create(countryName);

        var existing = await _repository.GetByIdAsync(phoneCodeId);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el prefijo telefónico con ID {id}.");

        var duplicateByCode = await _repository.GetByCountryCodeAsync(normalizedCode);
        if (duplicateByCode is not null && duplicateByCode.Id.Value != id)
            throw new InvalidOperationException($"Ya existe otro registro con el código '{normalizedCode.Value}'.");

        var duplicateByName = await _repository.GetByCountryNameAsync(normalizedName);
        if (duplicateByName is not null && duplicateByName.Id.Value != id)
            throw new InvalidOperationException($"Ya existe otro registro con el país '{normalizedName.Value}'.");

        existing.Update(normalizedCode.Value, normalizedName.Value);
        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync();
    }
}
