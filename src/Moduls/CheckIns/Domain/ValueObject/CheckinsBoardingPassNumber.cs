public sealed class CheckinsBoardingPassNumber
{
    public string Value { get; }
    private CheckinsBoardingPassNumber(string value) => Value = value;

    public static CheckinsBoardingPassNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El número de tarjeta de embarque no puede estar vacío.");
        
        if (value.Length > 20)
            throw new ArgumentException("El número de tarjeta de embarque no puede tener más de 20 caracteres.");

        return new CheckinsBoardingPassNumber(value);
    }
}