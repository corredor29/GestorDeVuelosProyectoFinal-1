// CityId.cs
public sealed class CityId
{
    public int Value { get; }
    private CityId(int value) => Value = value;
    public static CityId Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("El id de la ciudad no es válido", nameof(value));
        return new CityId(value);
    }
}