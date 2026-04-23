using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.UseCases;

public sealed class CreateCustomerUseCase
{
    private readonly ICustomersRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerUseCase(
        ICustomersRepository repository,
        AppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int personId)
    {
        var personExists = await _context.Persons
            .AsNoTracking()
            .AnyAsync(x => x.Id == personId);

        if (!personExists)
            throw new InvalidOperationException($"The person with id '{personId}' does not exist.");

        var existing = await _repository.GetByPersonIdAsync(personId);
        if (existing is not null)
            throw new InvalidOperationException($"Person '{personId}' is already registered as a customer.");

        var customer = Customer.Create(personId, DateTime.Now);

        await _repository.SaveAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }
}
