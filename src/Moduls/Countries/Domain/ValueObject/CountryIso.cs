using System.Text.RegularExpressions;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;

public sealed record CountryIsoCode
{
    public string Value { get; }
    private CountryIsoCode(string value) => Value = value;

    public static CountryIsoCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El código ISO no puede estar vacío.");


        var cleaned = value.Trim().ToUpper();
        
        if (!Regex.IsMatch(cleaned, @"^[A-Z]{2,3}$"))
            throw new ArgumentException("El código ISO debe tener 2 o 3 letras mayúsculas.");

        return new CountryIsoCode(cleaned);
    }
}