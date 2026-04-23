using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Repositories;

public interface IPhoneCodesRepository
{
    Task<PhoneCode?> GetByIdAsync(PhoneCodesId id);
    Task<PhoneCode?> GetByCountryCodeAsync(PhoneCodesCountryCode code);
    Task<PhoneCode?> GetByCountryNameAsync(PhoneCodesCountryName countryName);
    Task<PhoneCode?> GetByCountryCodeStringAsync(string code);
    Task<PhoneCode?> GetByCountryNameStringAsync(string countryName);
    Task<IEnumerable<PhoneCode>> GetAllAsync();
    Task SaveAsync(PhoneCode phoneCode);
    Task UpdateAsync(PhoneCode phoneCode);
    Task DeleteAsync(PhoneCodesId id);
    Task DeleteByCountryCodeAsync(PhoneCodesCountryCode code);
    Task DeleteByCountryNameAsync(PhoneCodesCountryName countryName);
}
