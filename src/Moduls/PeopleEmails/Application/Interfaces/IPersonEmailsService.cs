using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Application.Interfaces;

public interface IPersonEmailsService
{
    Task<IEnumerable<PersonEmail>> GetAllAsync();
    Task<IEnumerable<PersonEmail>> GetByPersonIdAsync(int personId);
    Task<PersonEmail?> GetByIdAsync(int id);
    Task CreateAsync(int personId, string emailUser, int emailDomainId, bool isPrimary);
    Task UpdateAsync(int id, int personId, string emailUser, int emailDomainId, bool isPrimary);
    Task DeleteAsync(int id);
}
