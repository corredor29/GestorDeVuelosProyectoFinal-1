namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Infrastructure.Entity;

public class SessionsEntity
{
    public int Id { get; set; }
    public int User_Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string? IpAddress { get; set; }
    public bool IsActive { get; set; }
}