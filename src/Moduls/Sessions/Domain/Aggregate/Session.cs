using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Aggregate;

public sealed class Session
{
    public SessionsId Id { get; private set; } = null!;
    public UsersId UsuarioId { get; private set; } = null!;
    public DateTime IniciadaEn { get; private set; }
    public DateTime? CerradaEn { get; private set; }
    public SessionsIpAddress IpOrigen { get; private set; } = null!;
    public SessionsStatus Activa { get; private set; } = null!;

    private Session() { }

    public static Session Create(
        int id, 
        int usuarioId, 
        string? ipOrigen, 
        DateTime iniciadaEn)
    {
        return new Session
        {
            Id = SessionsId.Create(id),
            UsuarioId = UsersId.Create(usuarioId),
            IpOrigen = SessionsIpAddress.Create(ipOrigen),
            IniciadaEn = iniciadaEn,
            Activa = SessionsStatus.Create(true) 
        };
    }
    internal void SetId(int id) => Id = SessionsId.Create(id);


    public void CloseSession()
    {
        CerradaEn = DateTime.Now;
        Activa = SessionsStatus.Create(false);
    }
}