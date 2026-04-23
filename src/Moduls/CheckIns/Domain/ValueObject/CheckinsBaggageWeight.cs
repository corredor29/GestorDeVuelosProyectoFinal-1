public sealed class CheckinsBaggageWeight
{
    public decimal Value { get; }
    private CheckinsBaggageWeight(decimal value) => Value = value;

    public static CheckinsBaggageWeight Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El peso del equipaje no puede ser un valor negativo.");
            
        return new CheckinsBaggageWeight(value);
    }
}