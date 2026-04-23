using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Infrastructure.Entity;

public class UsersEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int? Person_Id { get; set; }
    public int Role_Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastAccess { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public PersonEntity? Person { get; set; }
    public SystemRolesEntity Role { get; set; } = null!;

}
