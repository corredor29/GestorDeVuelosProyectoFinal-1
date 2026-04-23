using System;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Aggregate;

public sealed class SystemRole
{
    public RolesId Id { get; private set; } = null!;
    public RolesName Name { get; private set; } = null!;
    public RolesDescription Description { get; private set; } = null!;

    private SystemRole() { }

    public static SystemRole Create(int id, string name, string? description)
    {
        return new SystemRole
        {
            Id = RolesId.Create(id),
            Name = RolesName.Create(name),
            Description = RolesDescription.Create(description)
        };
    }
    internal void SetId(int id)
    {
        Id = RolesId.Create(id);
    }
    public void UpdateName(string newName)
    {
        Name = RolesName.Create(newName);
    }

    public void UpdateDescription(string? newDescription)
    {
        Description = RolesDescription.Create(newDescription);
    }
}