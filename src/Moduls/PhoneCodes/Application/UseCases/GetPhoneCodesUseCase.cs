using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.UseCases;

public sealed class GetPhoneCodesUseCase
{
    private readonly IPhoneCodesRepository _repository;

    public GetPhoneCodesUseCase(IPhoneCodesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PhoneCode>> ExecuteAllAsync() => _repository.GetAllAsync();

    public Task<PhoneCode?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(PhoneCodesId.Create(id));

    public Task<PhoneCode?> ExecuteByCountryCodeAsync(string countryCode) =>
        _repository.GetByCountryCodeAsync(PhoneCodesCountryCode.Create(countryCode));

    public Task<PhoneCode?> ExecuteByCountryNameAsync(string countryName) =>
        _repository.GetByCountryNameAsync(PhoneCodesCountryName.Create(countryName));
}
