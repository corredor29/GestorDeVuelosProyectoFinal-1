public sealed class CheckinsHasHoldBaggage
{
    public bool Value { get; }
    private CheckinsHasHoldBaggage(bool value) => Value = value;

    public static CheckinsHasHoldBaggage Create(bool value) => new(value);
}