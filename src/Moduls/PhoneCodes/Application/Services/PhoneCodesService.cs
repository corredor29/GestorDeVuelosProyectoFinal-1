using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.Services;

// Fachada del módulo de códigos telefónicos.
// Desde aquí la UI obtiene una entrada simple hacia los casos de uso.
public sealed class PhoneCodesService : IPhoneCodesService
{
    private readonly GetPhoneCodesUseCase _getUseCase;
    private readonly CreatePhoneCodeUseCase _createUseCase;
    private readonly UpdatePhoneCodeUseCase _updateUseCase;
    private readonly DeletePhoneCodeUseCase _deleteUseCase;

    public PhoneCodesService(
        GetPhoneCodesUseCase getUseCase,
        CreatePhoneCodeUseCase createUseCase,
        UpdatePhoneCodeUseCase updateUseCase,
        DeletePhoneCodeUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task<IEnumerable<PhoneCode>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<PhoneCode?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<PhoneCode?> GetByCountryCodeAsync(string countryCode) => _getUseCase.ExecuteByCountryCodeAsync(countryCode);
    public Task<PhoneCode?> GetByCountryNameAsync(string countryName) => _getUseCase.ExecuteByCountryNameAsync(countryName);
    public Task CreateAsync(string countryCode, string countryName) => _createUseCase.ExecuteAsync(countryCode, countryName);
    public Task UpdateAsync(int id, string countryCode, string countryName) => _updateUseCase.ExecuteAsync(id, countryCode, countryName);
    public Task DeleteByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task DeleteByCountryCodeAsync(string countryCode) => _deleteUseCase.ExecuteByCountryCodeAsync(countryCode);
    public Task DeleteByCountryNameAsync(string countryName) => _deleteUseCase.ExecuteByCountryNameAsync(countryName);
}
