using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Repositories;

public interface ISessionsRepository
{
   
    Task<Session?> GetByIdAsync(SessionsId id);
    
    Task<IEnumerable<Session>> GetActiveSessionsByUserIdAsync(UsersId usuarioId);

    Task SaveAsync(Session session);
    Task UpdateAsync(Session session);

}