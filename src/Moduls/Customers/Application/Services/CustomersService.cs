using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.UseCases;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.Services;

// Servicio del módulo de clientes.
// Mantiene la UI desacoplada de los casos de uso concretos.
public sealed class CustomersService : ICustomersService
{
    private readonly GetCustomersUseCase _get;
    private readonly CreateCustomerUseCase _create;
    private readonly DeleteCustomerUseCase _delete;

    public CustomersService(
        GetCustomersUseCase get,
        CreateCustomerUseCase create,
        DeleteCustomerUseCase delete)
    {
        _get = get;
        _create = create;
        _delete = delete;
    }

    public Task<IEnumerable<Customer>> GetAllAsync() => _get.ExecuteAsync();
    public Task<Customer?> GetByIdAsync(int id) => _get.GetByIdAsync(id);
    public Task<Customer?> GetByPersonIdAsync(int personId) => _get.GetByPersonIdAsync(personId);
    public Task<Customer?> GetByDocumentNumberAsync(string documentNumber) => _get.GetByDocumentNumberAsync(documentNumber);
    public Task CreateAsync(int personId) => _create.ExecuteAsync(personId);
    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}
