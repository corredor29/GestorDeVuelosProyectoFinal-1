using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;

public sealed class User
{
    public UsersId Id { get; private set; } = null!;
    public UsersUsername Username { get; private set; } = null!;
    public UsersPasswordHash PasswordHash { get; private set; } = null!;
    public PeopleId? PersonId { get; private set; }  = null!;
    public RolesId RolId { get; private set; } = null!;
    public UsersIsActive IsActive { get; private set; } = null!;
    public DateTime? LastAccess { get; private set; }

    private User() { }

    public static User Create(
        int id,
        string username,
        string passwordHash,
        int rolId,
        int? personId = null,
        bool isActive = true)
    {
        return new User
        {
            Id = UsersId.Create(id),
            Username = UsersUsername.Create(username),
            PasswordHash = UsersPasswordHash.Create(passwordHash),
            RolId = RolesId.Create(rolId),
            PersonId = personId.HasValue ? PeopleId.Create(personId.Value) : null, // ← corregido
            IsActive = UsersIsActive.Create(isActive)
        };
    }

    internal void SetId(int id) => Id = UsersId.Create(id);

    public void Activate() => IsActive = UsersIsActive.Create(true);
    public void Deactivate() => IsActive = UsersIsActive.Create(false);
    public void UpdateLastAccess() => LastAccess = DateTime.UtcNow;
    public void UpdatePassword(string newHash) => PasswordHash = UsersPasswordHash.Create(newHash);
    public void UpdateRole(int newRoleId) => RolId = RolesId.Create(newRoleId);
}