using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Application.Interfaces;

public interface IPersonService
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(int id);
    Task<IEnumerable<Person>> GetByFirstNameAsync(string firstName);
    Task<IEnumerable<Person>> GetByLastNameAsync(string lastName);
    Task<IEnumerable<Person>> GetByDocumentNumberAsync(string documentNumber);
    Task CreateAsync(int documentTypeId, string documentNumber, string firstName, string lastName, DateTime? birthDate, char? gender, int? addressId);
    Task UpdateAsync(int id, int documentTypeId, string documentNumber, string firstName, string lastName, DateTime? birthDate, char? gender, int? addressId);
    Task DeleteByIdAsync(int id);
    Task<int> DeleteByFirstNameAsync(string firstName);
    Task<int> DeleteByLastNameAsync(string lastName);
    Task<int> DeleteByDocumentNumberAsync(string documentNumber);
}
