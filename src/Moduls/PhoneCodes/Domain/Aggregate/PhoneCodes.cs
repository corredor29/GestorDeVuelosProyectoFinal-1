using System;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Aggregate;

public sealed class PhoneCode
{
    public PhoneCodesId Id { get; private set; } = null!;
    public PhoneCodesCountryCode Code { get; private set; } = null!;
    public PhoneCodesCountryName CountryName { get; private set; } = null!;

    private PhoneCode() { }

    private PhoneCode(PhoneCodesId id, PhoneCodesCountryCode code, PhoneCodesCountryName countryName)
    {
        Id = id;
        Code = code;
        CountryName = countryName;
    }

    public static PhoneCode Create(string code, string countryName)
    {
        return new PhoneCode
        {
            Code = PhoneCodesCountryCode.Create(code),
            CountryName = PhoneCodesCountryName.Create(countryName)
        };
    }

    public static PhoneCode FromPrimitives(int id, string code, string countryName)
    {
        return new PhoneCode(
            PhoneCodesId.Create(id),
            PhoneCodesCountryCode.Create(code),
            PhoneCodesCountryName.Create(countryName)
        );
    }

    public void Update(string code, string countryName)
    {
        Code = PhoneCodesCountryCode.Create(code);
        CountryName = PhoneCodesCountryName.Create(countryName);
    }
}
