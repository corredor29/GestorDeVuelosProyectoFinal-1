using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Aggregate;

public sealed class PassengerType
{
    public PassengerTypesId? Id { get; private set; }
    public PassengerTypesName Name { get; private set; } = null!;
    public PassengerMinAge MinAge { get; private set; } = null!;
    public PassengerMaxAge MaxAge { get; private set; } = null!;

    private PassengerType() { }

    private PassengerType(
        PassengerTypesId? id,
        PassengerTypesName name,
        PassengerMinAge minAge,
        PassengerMaxAge maxAge)
    {
        Id = id;
        Name = name;
        MinAge = minAge;
        MaxAge = maxAge;
    }

    public static PassengerType Create(string name, int? minAge, int? maxAge)
    {
        var min = PassengerMinAge.Create(minAge);
        var max = PassengerMaxAge.Create(maxAge);
        EnsureValidRange(name, min.Value, max.Value);
        return new PassengerType(null, PassengerTypesName.Create(name), min, max);
    }

    public static PassengerType FromPrimitives(int id, string name, int? minAge, int? maxAge)
    {
        var min = PassengerMinAge.Create(minAge);
        var max = PassengerMaxAge.Create(maxAge);
        EnsureValidRange(name, min.Value, max.Value);
        return new PassengerType(
            PassengerTypesId.Create(id),
            PassengerTypesName.Create(name),
            min,
            max);
    }

    public void Update(string name, int? minAge, int? maxAge)
    {
        var min = PassengerMinAge.Create(minAge);
        var max = PassengerMaxAge.Create(maxAge);
        EnsureValidRange(name, min.Value, max.Value);
        Name = PassengerTypesName.Create(name);
        MinAge = min;
        MaxAge = max;
    }

    public bool ContainsAge(int ageInYears)
    {
        if (ageInYears < 0)
            return false;

        if (MinAge.Value.HasValue && ageInYears < MinAge.Value.Value)
            return false;

        if (MaxAge.Value.HasValue && ageInYears > MaxAge.Value.Value)
            return false;

        return true;
    }

    private static void EnsureValidRange(string nameForError, int? min, int? max)
    {
        if (min.HasValue && max.HasValue && min.Value > max.Value)
            throw new ArgumentException($"En '{nameForError}': min_age no puede superar max_age.");
    }
}
