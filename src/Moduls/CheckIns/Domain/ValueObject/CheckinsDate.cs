public sealed class CheckinsDate
{
    public DateTime Value { get; }
    private CheckinsDate(DateTime value) => Value = value;

    public static CheckinsDate Create(DateTime value)
    {
        if (value > DateTime.Now.AddHours(1))
            throw new ArgumentException("La fecha de check-in no puede ser una fecha futura.");
            
        return new CheckinsDate(value);
    }
}