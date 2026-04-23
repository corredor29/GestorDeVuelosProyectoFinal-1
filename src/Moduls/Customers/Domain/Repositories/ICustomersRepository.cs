using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Repositories;

public interface ICustomersRepository
{
    Task<Customer?> GetByIdAsync(CustomersId id);
    Task<Customer?> GetByPersonIdAsync(int personId);
    Task<Customer?> GetByDocumentNumberAsync(string documentNumber);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task SaveAsync(Customer customer);
    Task DeleteAsync(CustomersId id);
}
