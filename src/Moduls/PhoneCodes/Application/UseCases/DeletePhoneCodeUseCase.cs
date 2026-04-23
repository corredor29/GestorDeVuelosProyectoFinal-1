using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.UseCases;

public sealed class DeletePhoneCodeUseCase
{
    private readonly IPhoneCodesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePhoneCodeUseCase(IPhoneCodesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var phoneCodeId = PhoneCodesId.Create(id);
        var existing = await _repository.GetByIdAsync(phoneCodeId);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el prefijo telefónico con ID {id}.");

        await _repository.DeleteAsync(phoneCodeId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByCountryCodeAsync(string countryCode)
    {
        var normalizedCode = PhoneCodesCountryCode.Create(countryCode);
        var existing = await _repository.GetByCountryCodeAsync(normalizedCode);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el prefijo telefónico con código '{normalizedCode.Value}'.");

        await _repository.DeleteByCountryCodeAsync(normalizedCode);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByCountryNameAsync(string countryName)
    {
        var normalizedName = PhoneCodesCountryName.Create(countryName);
        var existing = await _repository.GetByCountryNameAsync(normalizedName);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el prefijo telefónico para el país '{normalizedName.Value}'.");

        await _repository.DeleteByCountryNameAsync(normalizedName);
        await _unitOfWork.SaveChangesAsync();
    }
}
