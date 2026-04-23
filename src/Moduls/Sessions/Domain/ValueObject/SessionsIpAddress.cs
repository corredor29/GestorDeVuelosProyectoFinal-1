namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.ValueObject;

public sealed class SessionsIpAddress
{
    public string? Value { get; }
    private SessionsIpAddress(string? value) => Value = value;

    public static SessionsIpAddress Create(string? value)
    {
        if (value != null && value.Length > 45)
            throw new ArgumentException("La dirección IP excede la longitud permitida.");

        return new SessionsIpAddress(value?.Trim());
    }
}