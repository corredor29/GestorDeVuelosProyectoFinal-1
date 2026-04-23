using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;

public sealed class PersonalUpdatedIn
{
    public DateTime Value { get; }
    private PersonalUpdatedIn(DateTime value) => Value = value;

    public static PersonalUpdatedIn Create(DateTime value)
    {
        if (value == default)
        {
            throw new ArgumentException("La fecha de actualización no puede estar vacía.");
        }

        // Nota: En el Agregado podrías validar que esta fecha sea >= a la de creación
        return new PersonalUpdatedIn(value);
    }
}