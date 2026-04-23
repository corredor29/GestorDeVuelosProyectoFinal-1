namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.ValueObject;

public sealed class SessionsStatus
{
    public bool IsActive { get; }
    private SessionsStatus(bool isActive) => IsActive = isActive;

    public static SessionsStatus Create(bool isActive) => new SessionsStatus(isActive);
}