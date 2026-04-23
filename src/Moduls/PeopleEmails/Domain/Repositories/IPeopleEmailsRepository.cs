using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Repositories;

public interface IPersonEmailsRepository
{
    Task<PersonEmail?> GetByIdAsync(PersonEmailsId id);
    Task<IEnumerable<PersonEmail>> GetByPersonIdAsync(int personId);
    Task<PersonEmail?> GetPrimaryByPersonIdAsync(int personId);
    Task<IEnumerable<PersonEmail>> GetAllAsync();
    Task SaveAsync(PersonEmail personEmail);
    Task UpdateAsync(PersonEmail personEmail);
    Task DeleteAsync(PersonEmailsId id);
}
