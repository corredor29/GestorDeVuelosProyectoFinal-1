using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.UseCases;

public sealed class GetCustomersUseCase
{
    private readonly ICustomersRepository _repository;

    public GetCustomersUseCase(ICustomersRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Customer>> ExecuteAsync() => _repository.GetAllAsync();
    public Task<Customer?> GetByIdAsync(int id) => _repository.GetByIdAsync(CustomersId.Create(id));
    public Task<Customer?> GetByPersonIdAsync(int personId) => _repository.GetByPersonIdAsync(personId);
    public Task<Customer?> GetByDocumentNumberAsync(string documentNumber) => _repository.GetByDocumentNumberAsync(documentNumber);
}
