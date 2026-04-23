using System;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

public sealed record PeopleBirthDate
{
    public DateTime? Value { get; }
    private PeopleBirthDate(DateTime? value) => Value = value;

    public static PeopleBirthDate Create(DateTime? value)
    {
        if (value.HasValue && value.Value > DateTime.Now)
            throw new ArgumentException("La fecha de nacimiento no puede ser en el futuro.");

        return new PeopleBirthDate(value);
    }
}