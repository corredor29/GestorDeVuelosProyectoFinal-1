using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Persistence.seeders;

public sealed class CustomersSeeder
{
    private readonly AppDbContext _context;
    private readonly ICustomersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomersSeeder(AppDbContext context, ICustomersRepository repository, IUnitOfWork unitOfWork)
    {
        _context = context;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        var personIds = await _context.Persons
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .Take(5)
            .ToListAsync();

        foreach (var personId in personIds)
        {
            var existing = await _repository.GetByPersonIdAsync(personId);
            if (existing is not null)
                continue;

            await _repository.SaveAsync(Customer.Create(personId, DateTime.Now));
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
