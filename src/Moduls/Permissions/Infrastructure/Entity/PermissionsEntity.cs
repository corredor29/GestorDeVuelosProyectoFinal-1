using System;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Infrastructure.Entity;

public class PermissionsEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<RolePermissionsEntity> RolePermissions { get; set; } = new List<RolePermissionsEntity>();
}
