using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.UseCases;

public sealed class DeleteCustomerUseCase
{
    private readonly ICustomersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerUseCase(ICustomersRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(CustomersId.Create(id))
            ?? throw new InvalidOperationException($"Customer with id '{id}' not found.");

        // Aquí debe entrar la validación de reservas activas cuando exista
        // una relación real entre reservations y customers en el proyecto.

        await _repository.DeleteAsync(customer.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
