using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Infrastructure.Entity;

public class RolePermissionsEntity
{
    public int Id { get; set; }
    public int Role_Id { get; set; }
    public int Permission_Id { get; set; }
    public SystemRolesEntity Role { get; set; } = null!;
    public PermissionsEntity Permission { get; set; } = null!;

}
