using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.Interfaces;

public interface IPhoneCodesService
{
    Task<IEnumerable<PhoneCode>> GetAllAsync();
    Task<PhoneCode?> GetByIdAsync(int id);
    Task<PhoneCode?> GetByCountryCodeAsync(string countryCode);
    Task<PhoneCode?> GetByCountryNameAsync(string countryName);
    Task CreateAsync(string countryCode, string countryName);
    Task UpdateAsync(int id, string countryCode, string countryName);
    Task DeleteByIdAsync(int id);
    Task DeleteByCountryCodeAsync(string countryCode);
    Task DeleteByCountryNameAsync(string countryName);
}
