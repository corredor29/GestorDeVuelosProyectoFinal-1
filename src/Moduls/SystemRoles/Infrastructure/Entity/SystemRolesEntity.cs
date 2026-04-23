using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Infrastructure.Entity;

public class SystemRolesEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<RolePermissionsEntity> RolePermissions { get; set; } = new List<RolePermissionsEntity>();
    public ICollection<UsersEntity> Users { get; set; } = new List<UsersEntity>();
}
