using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.Interfaces;

public interface ICustomersService
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByPersonIdAsync(int personId);
    Task<Customer?> GetByDocumentNumberAsync(string documentNumber);
    Task CreateAsync(int personId);
    Task DeleteAsync(int id);
}
