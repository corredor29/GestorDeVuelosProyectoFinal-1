namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.ValueObject;

public sealed record PersonPhonesIsPrimary
{
    public bool Value { get; }
    private PersonPhonesIsPrimary(bool value) => Value = value;

    public static PersonPhonesIsPrimary Create(bool value) => new(value);
}
