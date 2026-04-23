using System;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.ValueObject;

public sealed class SessionsId
{
    public int Value { get; }
    private SessionsId(int value) => Value = value;

    public static SessionsId Create(int value)
    {
        // 0 se usa como marcador temporal cuando la PK es autogenerada por la BD.
        // El repositorio reemplaza el id real vía Session.SetId(...) después de SaveChanges.
        if (value < 0)
            throw new ArgumentException("El id de la sesión no es válido.");
        return new SessionsId(value);
    }
}