using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.UseCases;

public sealed class DeleteCustomerUseCase
{
    private readonly ICustomersRepository _repository;
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerUseCase(
        ICustomersRepository repository,
        IBookingsRepository bookingsRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _bookingsRepository = bookingsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        var customerId = CustomersId.Create(id);

        var customer = await _repository.GetByIdAsync(customerId)
            ?? throw new InvalidOperationException($"Customer with id '{id}' not found.");

        // Verificar si tiene reservas asociadas
        var bookings = await _bookingsRepository.GetByClientIdAsync(customerId);

        if (bookings.Any())
            throw new InvalidOperationException(
                $"Cannot delete customer '{id}' because they have {bookings.Count()} associated booking(s).");

        await _repository.DeleteAsync(customerId);
        await _unitOfWork.SaveChangesAsync();
    }
}