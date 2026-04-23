using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;

public interface IUsersRepository
{
    Task<User?> GetByIdAsync(UsersId id);
    Task<User?> GetByUsernameAsync(UsersUsername username);
    Task<User?> GetByPersonIdAsync(PeopleId personId);
    Task<IEnumerable<User>> GetByRoleIdAsync(RolesId roleId);
    Task<IEnumerable<User>> GetActiveUsersAsync();
    Task<IEnumerable<User>> GetInactiveUsersAsync();
    Task<IEnumerable<User>> GetAllAsync();
    Task SaveAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(UsersId id);
}
