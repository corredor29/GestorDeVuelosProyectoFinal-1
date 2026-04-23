using System;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;

public sealed class PersonalCreatedIn
{
    public DateTime Value { get; }
    private PersonalCreatedIn(DateTime value) => Value = value;

    public static PersonalCreatedIn Create(DateTime value)
    {
        if (value == default)
        {
            throw new ArgumentException("La fecha de registro (creado_en) no es una fecha válida.");
        }

        // Permitimos un margen de 1 minuto por retrasos de milisegundos en el servidor
        if (value > DateTime.Now.AddMinutes(1))
        {
            throw new ArgumentException("La fecha de creación no puede ser una fecha futura.");
        }

        return new PersonalCreatedIn(value);
    }
}