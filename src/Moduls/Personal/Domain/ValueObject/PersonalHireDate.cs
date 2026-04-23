using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;

public sealed class PersonalHireDate
{
    public DateTime Value { get; }
    private PersonalHireDate(DateTime value) => Value = value;
    public static PersonalHireDate Create(DateTime value)
    {
        if (value > DateTime.Now)
            throw new ArgumentException("La fecha de ingreso no puede ser futura.");
        return new PersonalHireDate(value);
    }
}