using System.Text.RegularExpressions;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;

public sealed class FlightSeatsCode
{
    public string Value { get; }    
    private FlightSeatsCode(string value) => Value = value;
    public static FlightSeatsCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El código de asiento es obligatorio.");
        
        var code = value.Trim().ToUpperInvariant();
        if (!Regex.IsMatch(code, @"^\d{1,3}[A-Z]$"))
            throw new ArgumentException("El código de asiento debe tener formato fila y letra, por ejemplo 12A.");
            
        return new FlightSeatsCode(code);
    }
}
